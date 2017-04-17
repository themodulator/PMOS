using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pmos
{
    public static class CloningExtension
    {

        public static void CloneFrom<TSource, TTarget>(this TTarget target, TSource source)
        {
            target.CloneFrom(source, source.GetType());
        }

        public static void CloneFrom<TSource, TTarget>(this TTarget target, TSource source, Type type)
        {
            target.CloneFrom(source, new List<Type>(new Type[] { type }));
        }

        public static void CloneFrom<TSource, TTarget>(this TTarget target, TSource source, IEnumerable<Type> types)
        {
            foreach (Type t in types)
            {

               IEnumerable<PropertyInfo>  pp = t.GetProperties().Where(x => x.CanRead & x.CanWrite);

               target.CloneFrom(source, pp);
            }
        }

        public static void CloneFrom<TSource, TTarget>(this TTarget target, TSource source, Expression<Func<TSource, object>>[] properties)
        {

            IEnumerable<PropertyInfo> pp = properties.Select(x => x.GetPropertyFromExpression());

            target.CloneFrom(source, pp);

        }

        public static void CloneFrom<TSource, TTarget>(this TTarget target, TSource source, IEnumerable<PropertyInfo> properties)
        {
            foreach (PropertyInfo p in properties)
            {
                var sp = source.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

                var tp = target.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

                if (sp != null & tp != null)
                {
                    object v = sp.GetValue(source, null);

                    tp.SetValue(target, v, null);
                }

            }

        }
    }
}
