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
    public static class SerializationExtensions
    {
        public static TEntity Load<TEntity>(string path, bool encrypt, TEntity @default = null)
            where TEntity : class
        {
            TEntity item = null;

            if (File.Exists(path))
            {
                XmlSerializer x = new XmlSerializer(typeof(TEntity));

                if (encrypt)
                {

                    Encryption e = new Encryption();

                    string encrypted_content = File.ReadAllText(path);

                    string clear_content = e.DecryptString(encrypted_content);

                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(clear_content);

                    MemoryStream stream = new MemoryStream(bytes);


                    item = (TEntity)x.Deserialize(stream);
                }
                else
                {
                    using (StreamReader stream = new StreamReader(path))
                    {
                        item = (TEntity)x.Deserialize(stream);
                    }
                }


                return item;
            }
            else
            {
                if(@default == null)
                    return Activator.CreateInstance<TEntity>();
                else
                {
                    @default.Save(path, encrypt);

                    return @default;
                }
            }


        }

        public static void Save<TEntity>(this TEntity item, string path, bool decrypt = true)
        {

            XmlSerializer x = new XmlSerializer(typeof(TEntity));

            

            if (File.Exists(path))
                File.Delete(path);

            if (decrypt)
            {
                Encryption e = new Encryption();

                MemoryStream stream = new MemoryStream();

                x.Serialize(stream, item);

                string clear_text = System.Text.Encoding.UTF8.GetString(stream.ToArray());

                string encrypted_text = e.EncryptToString(clear_text);

                File.WriteAllText(path, encrypted_text);

            }
            else
            {
                using (StreamWriter stream = new StreamWriter(path))
                {
                    x.Serialize(stream, item);
                }
            }

        }
       

        public static byte[] GetBytes(this Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }

        }

        #region JSON

        public static string AjaxSerialize(this object item)
        {
            
            PropertyInfo[] serializable = item.GetType().GetProperties()
                .Where(x => x.GetCustomAttribute<XmlIgnoreAttribute>() == null).ToArray();

            string[] pairs = serializable.Select(x => string.Format("{0}={1}", x.Name, (x.GetValue(item, null) == null) ? "" : x.GetValue(item, null).ToString().Replace("&", "%26"))).ToArray();

            string query = string.Join("&", pairs);

            return query;

        }

        #endregion
    }
}
