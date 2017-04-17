using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pmos
{
    public static class IOExtensions
    {
        public static List<TEntity> ParseTextFile<TEntity>(this Stream stream, char delimiter = '\t')
            where TEntity : class, new()
        {


            stream.Position = 0;

            List<TEntity> items = new List<TEntity>();

            using (StreamReader r = new StreamReader(stream))
            {

                string[] header = null;

                while(!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    string[] record = null;

                    if (header == null)
                        header = line.Split(delimiter);
                    else
                    {

                        TEntity entity = new TEntity();

                        record = line.Split(delimiter);

                        for(int i = 0; i <= record.Length; i++)
                        {
                            PropertyInfo p = typeof(TEntity).GetProperties().FirstOrDefault(x => x.Name.Equals(record[i], StringComparison.OrdinalIgnoreCase));

                            if (!p.CanWrite)
                                throw new Exception(string.Format("Property {0} is readonly", p.Name));

                            if(p != null)
                            {
                                string value = record[1];

                                try
                                {
                                    p.SetValue(entity, value, null);
                                }
                                catch(Exception ex)
                                {
                                    throw new Exception(string.Format("Could not write value {0} to property {1}", value, p.Name), ex);
                                }
                            }
                        }

                        items.Add(entity);
                    }

                }

                return items;
            }

        }
    }
}
