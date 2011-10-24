using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace UMR.Saniteri.DataFactory
{
    public class ObjectReaderWriter
    {
        public void writeObject(object instance, string fileName)
        {
            using (var writer = new FileStream(fileName, FileMode.Create))
            {
                var ser = new DataContractSerializer(instance.GetType());
                ser.WriteObject(writer, instance);
                writer.Close();
            }
        }

        public T readObject<T>(string fileName)
        {
            try
            {
                if (!File.Exists(fileName)) return default(T);
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    var quatas = new XmlDictionaryReaderQuotas();
                    quatas.MaxDepth = int.MaxValue;
                    using (var reader = XmlDictionaryReader.CreateTextReader(fs, quatas))
                    {
                        var ser = new DataContractSerializer(typeof(T));
                        var data = ser.ReadObject(reader, true);
                        reader.Close();
                        fs.Close();
                        return (T)data;
                    }
                }
            }
            catch { return default(T); }
        }
    }
}
