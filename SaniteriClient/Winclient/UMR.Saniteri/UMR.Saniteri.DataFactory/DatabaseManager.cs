using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.DataFactory
{
    public class DatabaseManager
    {
        public enum Providers { SQLServer, SQLLite };
        public static IServer server { get; private set; }
        
        static string modelName { get; set; }
        public static string dataFile { get; private set; }

        static DatabaseManager()
        {
            modelName = "SaniteriDataModel";
            dataFile = "SaniteriMain";
            server = new SQLDatabase(modelName, dataFile);
        }
    }
}
