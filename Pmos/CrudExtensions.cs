using Microsoft.AspNet.Identity;
using Pmos.Poco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pmos
{
    public static class CrudExtensions
    {

        #region Create

        public static object DbInsert<TDbContext>(this TDbContext db, Type type, object entity)
            where TDbContext : DbContext
        {

            IIdentifiablePoco poco = entity as IIdentifiablePoco;


            if (string.IsNullOrEmpty(((IIdentifiablePoco)entity).Id) || ((IIdentifiablePoco)entity).Id.Equals(Guid.Empty.ToString()))
                ((IIdentifiablePoco)entity).Id = Guid.NewGuid().ToString();

            if (entity as IRecordAuthorship != null)
                ((IRecordAuthorship)entity).SetAuthorInfo(true);

            DbSet dbset = db.Set(type);

            dbset.Add(entity);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex.BuildCrudError();
            }

            return entity;

        }

        public static TEntity DbInsert<TDbContext, TEntity>(this TDbContext db, TEntity entity)
            where TDbContext : DbContext
            where TEntity : class, IIdentifiablePoco
        {

            if (entity == null)
                throw new Exception(string.Format("Error creating null item {0}", typeof(TEntity).Name));


            if (string.IsNullOrEmpty(entity.Id) || entity.Id.Equals(Guid.Empty.ToString()))
                entity.Id = Guid.NewGuid().ToString();

            if (entity as IRecordAuthorship != null)
                ((IRecordAuthorship)entity).SetAuthorInfo(true);

            DbSet<TEntity> dbset = db.Set<TEntity>();

            dbset.Add(entity);

            try
            {
                db.SaveChanges();

                entity = dbset.FirstOrDefault(x => x.Id.Equals(entity.Id, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {

                throw ex.BuildCrudError();
            }

            return entity;

        }

        private static Exception BuildCrudError(this Exception ex)
        {
            Exception vex = null;

            if (ex.GetType() == typeof(DbEntityValidationException))
            {
                DbEntityValidationException e = (DbEntityValidationException)ex;

                vex = new Exception("Validation failed");

                foreach (var e1 in e.EntityValidationErrors)
                {
                    foreach (var e2 in e1.ValidationErrors)
                    {
                        vex.AddException(new Exception("Property: " + e2.PropertyName + "\nError: " + e2.ErrorMessage));
                    }
                }
            }
            else
            {
                vex = new Exception("The operation failed because changes could not be written to the database", ex);
            }

            return vex;

        }

        #endregion

        #region Update

        public static TEntity DbUpdate<TEntity, TDbContext>(this TDbContext db, TEntity entity)
            where TDbContext : DbContext
            where TEntity : class, IIdentifiablePoco
        {

            DbSet<TEntity> dbset = db.Set<TEntity>();

            TEntity fromdb = dbset.Find(entity.Id);

            if(fromdb == null)
                throw new Exception("Operation failed because the entity could not be found in the database");

            if (entity as IRecordAuthorship != null)
                ((IRecordAuthorship)entity).SetAuthorInfo(false);


            fromdb.CrudCopy(entity);


            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception("The operation failed because changes could not be written to the database", ex);
            }

            return fromdb;
        }

        public static TEntity DbUpdate<TEntity, TProperty, TDbContext>(this TDbContext db, TEntity entity, List<Expression<Func<TEntity, TProperty>>> properties)
            where TDbContext : DbContext
            where TEntity : class, IIdentifiablePoco
        {
            DbSet<TEntity> dbset = db.Set<TEntity>();

            TEntity fromdb = dbset.Find(entity.Id);

            if (fromdb == null)
                throw new Exception("Operation failed because the entity could not be found in the database");

            if (entity as IRecordAuthorship != null)
                ((IRecordAuthorship)entity).SetAuthorInfo(false);


            foreach (var p in properties)
            {
                PropertyInfo property = p.GetPropertyFromExpression();

                object v = property.GetValue(entity, null);

                property.SetValue(fromdb, v, null);

            }

            db.SaveChanges();

            TEntity item = dbset.Find(entity.Id);

            return item;

        }

        public static TEntity DbUpdate<TEntity, TDbContext>(this TDbContext db,  TEntity entity, Expression<Func<TEntity, bool>> where)
            where TDbContext : DbContext
            where TEntity : class
        {

            DbSet<TEntity> dbset = db.Set<TEntity>();

            TEntity fromdb = dbset.FirstOrDefault(where);

            if (fromdb == null)
                throw new Exception("Operation failed because the entity could not be found in the database");

            if (entity as IRecordAuthorship != null)
                ((IRecordAuthorship)entity).SetAuthorInfo(false);


            fromdb.CrudCopy(entity);


            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("The operation failed because changes could not be written to the database", ex);
            }

            return fromdb;
        }


        public static object DbUpdate<TDbContext>(this TDbContext db, System.Type type, object entity, string where)
            where TDbContext : DbContext
        {

            DbSet dbset = db.Set(type);

            Task<List<object>> results = dbset.Where(where).ToListAsync();

            object fromdb = results.Result.FirstOrDefault();

            if (fromdb == null)
                throw new Exception("Operation failed because the entity could not be found in the database");

            if (entity as IRecordAuthorship != null)
                ((IRecordAuthorship)entity).SetAuthorInfo(false);


            fromdb.CrudCopy(entity, type.GetProperties());


            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex.BuildCrudError();
            }

            return fromdb;
        }

        public static TEntity CrudCopy<TEntity>(this TEntity fromdb, TEntity entity)
        {

           return fromdb.CrudCopy(entity, typeof(TEntity).GetProperties().ToArray());

        }

        public static TEntity CrudCopy<TEntity>(this TEntity fromdb, TEntity entity, PropertyInfo[] properties)
        {
            List<string> to_ignore = new List<string>();

            string[] ignore_id = typeof(IIdentifiablePoco).GetProperties()
                .Select(x => x.Name.ToLower()).ToArray();

            string[] ignore_author = typeof(IRecordAuthorship).GetProperties()
                .Select(x => x.Name.ToLower()).ToArray();

            to_ignore.AddRange(ignore_id);
            to_ignore.AddRange(ignore_author);

            to_ignore.AddRange(typeof(TEntity).GetProperties()
                .Where(x => x.GetCustomAttribute<DbContextUpdateIgnore>() != null)
                .Select(x => x.Name.ToLower()));

            to_ignore.AddRange(typeof(TEntity).GetProperties()
                .Where(x => x.GetCustomAttribute<NotMappedAttribute>() != null)
                .Select(x => x.Name.ToLower()));


            IEnumerable<ForeignKeyAttribute> fks = typeof(TEntity).GetProperties().Where(x => x.GetCustomAttribute<ForeignKeyAttribute>() != null).Select(x => x.GetCustomAttribute<ForeignKeyAttribute>());

            foreach(ForeignKeyAttribute fk in fks)
            {
                PropertyInfo fk_prop = typeof(TEntity).GetProperties().FirstOrDefault(x => x.Name.Equals(fk.Name));

                if(fk_prop != null & !to_ignore.Any(x => x.Equals(fk_prop.Name)))
                    to_ignore.Add(fk_prop.Name);

            }

            IEnumerable<PropertyInfo> list_props = typeof(TEntity).GetProperties().Where(x => 
                x.PropertyType.IsGenericType &&
                x.PropertyType.GetGenericTypeDefinition() == typeof(List<>)); // | x.PropertyType.IsGenericType);

            to_ignore.AddRange(list_props.Select(x => x.Name.ToLower()));


            Type t = typeof(TEntity);

            PropertyInfo[] all = t.GetProperties();


            PropertyInfo[] updatable = properties
                .Where(x => !to_ignore.Contains(x.Name.ToLower())
                    & x.CanRead & x.CanWrite).ToArray();

            foreach (PropertyInfo p in updatable)
            {
                object v = p.GetValue(entity, null);

                p.SetValue(fromdb, v, null);
            }

            return fromdb;

        }

        #endregion

        #region Delete

        public static void DbDelete<TDbContext, TEntity>(this TDbContext db,  TEntity entity)
            where TDbContext : DbContext
            where TEntity : class, IIdentifiablePoco
        {

            DbSet<TEntity> dbset = db.Set<TEntity>();

            TEntity fromdb = dbset.Find(entity.Id);

            dbset.Remove(fromdb);

            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception("The operation failed because changes could not be written to the database", ex);
            }
        }

        #endregion

        #region Push

        public static TEntity DbPush<TDbContext, TEntity>(this TDbContext db, TEntity entity, bool overwrite = false)
            where TEntity : class, IIdentifiablePoco
            where TDbContext : DbContext
        {

            DbSet<TEntity> dbset = db.Set<TEntity>();

            TEntity e = dbset.Find(entity.Id);

            if (e == null)
                return db.DbInsert(entity);
            else
            {
                if (overwrite)
                    return db.DbUpdate(entity);
                else
                    return e;
            }

        }


        public static TEntity DbPush<TDbContext, TEntity>(this TDbContext db, TEntity entity, Expression<Func<TEntity, bool>> key, bool overwrite = false)
            where TEntity : class, IIdentifiablePoco
            where TDbContext : DbContext
        {

            DbSet<TEntity> dbset = db.Set<TEntity>();

            TEntity e = dbset.FirstOrDefault(key);

            if (e == null)
                return db.DbInsert(entity);
            else
            {
                if (overwrite)
                    return db.DbUpdate(e);
                else
                    return e;
            }

        }

        #endregion

        #region Authorship

        public static TEntity SetupPocoFields<TEntity>(this TEntity entity, bool forInsert)
            where TEntity : class, IIdentifiablePoco, IRecordAuthorship
        {
            if(string.IsNullOrEmpty(entity.Id) || entity.Id.Equals(Guid.Empty.ToString()))
                entity.Id = Guid.NewGuid().ToString();
            else
            {
                Guid g = Guid.Empty;

                if (!Guid.TryParse(entity.Id, out g))
                    throw new Exception("Entity id is not in guid format");
            }

            entity.SetAuthorInfo(forInsert);

            return entity;
        }


        public static void SetAuthorInfo<TEntity>(this TEntity entity, bool forInsert)
            where TEntity : class, IRecordAuthorship
        {

            string author = string.Format("{0}\\{1}", System.Environment.UserDomainName, System.Environment.UserName);

            if (HttpContext.Current != null && HttpContext.Current.User!= null && HttpContext.Current.User.Identity.IsAuthenticated) 
                author = HttpContext.Current.User.Identity.Name;

            else if (HttpContext.Current != null && HttpContext.Current.User != null && !HttpContext.Current.User.Identity.IsAuthenticated) 
                author = "Anonymous";
            
            entity.SetAuthorInfo(forInsert, author);

        }

        public static void SetAuthorInfo<TEntity>(this TEntity entity, bool forInsert, string author)
            where TEntity : class, IRecordAuthorship
        {

            entity.ModifiedBy = author;
            entity.ModifiedOn = DateTime.Now;


            if (forInsert)
            {
                entity.CreatedBy = author;
                entity.CreatedOn = DateTime.Now;
            }
        }

        #endregion

    }
}
