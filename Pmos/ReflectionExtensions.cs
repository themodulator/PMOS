using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pmos
{
    public static class ReflectionExtensions
    {

        #region Assembly

        public static string Copyright(this Assembly assembly)
        {
            return assembly.GetAssemblyAttribute<AssemblyCopyrightAttribute>("Copyright");
        }

        public static string Title(this Assembly assembly)
        {
            return assembly.GetAssemblyAttribute<AssemblyTitleAttribute>("Title");
        }

        public static string Company(this Assembly assembly)
        {
            return assembly.GetAssemblyAttribute<AssemblyCompanyAttribute>("Company");
        }

        public static string GetAssemblyAttribute<TAttribute>(this Assembly assembly, string property)
         where TAttribute : Attribute
        {
            object[] attribs = assembly.GetCustomAttributes(typeof(TAttribute), true);

            string value = "";

            if (attribs.Length > 0)
            {

                PropertyInfo p = typeof(TAttribute).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.OrdinalIgnoreCase));

                if (p != null)
                {

                    TAttribute attribute = ((TAttribute)attribs[0]);

                    object v = p.GetValue(attribute, null);

                    value = (v == null) ? "" : v.ToString();

                }

            }

            return value;
        }

        #endregion

        #region Property Info

        public static Dictionary<TAttribute, PropertyInfo> GetAttributes<TAttribute>(this Type itemType)
            where TAttribute : Attribute
        {
            IEnumerable<PropertyInfo> pp = itemType.GetProperties().Where(x => x.GetCustomAttribute<TAttribute>() != null);

            Dictionary<TAttribute, PropertyInfo> dic = new Dictionary<TAttribute, PropertyInfo>();

            foreach(PropertyInfo p in pp)
            {
                TAttribute a = p.GetCustomAttribute<TAttribute>();

                dic.Add(a, p);
            }

            return dic;
        }

        public static PropertyInfo GetPropertyFromExpression<T, TProperty>(this Expression<Func<T, TProperty>> field)
        {
            MemberExpression Exp = null;

            //this line is necessary, because sometimes the expression comes in as Convert(originalexpression)
            if (field.Body is UnaryExpression)
            {
                var UnExp = (UnaryExpression)field.Body;
                if (UnExp.Operand is MemberExpression)
                {
                    Exp = (MemberExpression)UnExp.Operand;
                }
                else
                    throw new ArgumentException();
            }
            else if (field.Body is MemberExpression)
            {
                Exp = (MemberExpression)field.Body;
            }
            else
            {
                throw new ArgumentException();
            }

            return (PropertyInfo)Exp.Member;
        }

        public static Dictionary<PropertyInfo, TAttribute> GetPropertyAttributePairs<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            PropertyInfo[] properties = type.GetProperties().Where(x => x.GetCustomAttribute<TAttribute>() != null).ToArray();

            Dictionary<PropertyInfo, TAttribute> pairs = new Dictionary<PropertyInfo, TAttribute>();

            foreach (PropertyInfo p in properties)
            {
                pairs.Add(p, p.GetCustomAttribute<TAttribute>());
            }

            return pairs;
        }

        #endregion


        #region Table Attributes

        public static PropertyInfo GetForeignKey(this PropertyInfo p, System.Type itemType)
        {
            ForeignKeyAttribute fk_attr = p.GetCustomAttribute<ForeignKeyAttribute>();

            if (fk_attr == null)
                return null;

            PropertyInfo fk = itemType.GetProperties().FirstOrDefault(x => x.Name.Equals(fk_attr.Name));

            return fk;

        }

        public static bool IsForiegnKey(this PropertyInfo p, System.Type itemType)
        {
            PropertyInfo fk = itemType.GetProperties().FirstOrDefault(x => x.GetCustomAttribute<ForeignKeyAttribute>() != null && x.GetCustomAttribute<ForeignKeyAttribute>().Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

            return (fk != null);
        }

        #endregion

    }
}
