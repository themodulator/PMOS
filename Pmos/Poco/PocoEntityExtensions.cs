using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using System.Reflection;



namespace Pmos.Poco
{
    public static class PocoEntityExtensions
    {
        public static void GetPocoMeta<TMetaData>(this TMetaData meta, Type t, string colloquial)
            where TMetaData : IPocoEntityMeta
        {
            meta.TypeFullName = t.FullName;

            meta.TypeName = t.Name;

            meta.Colloquial = colloquial;

            meta.Pluralized = meta.Colloquial.Pluralize(false);

            TableAttribute t_attr = t.GetCustomAttribute<TableAttribute>();

            if (t_attr != null)
            {

                meta.Table = t_attr.Name;

                meta.Schema = t_attr.Schema;

            }
        }

        public static IPocoEntityMeta GetPocoMeta<TItem>(this TItem item, string collquial = null)
        {
            PocoMetaData meta = new PocoMetaData();

            if (string.IsNullOrEmpty(collquial))
                collquial = typeof(TItem).Name.Humanize(LetterCasing.Title);

            meta.GetPocoMeta(typeof(TItem), collquial);

            return meta;

        }
    }
}
