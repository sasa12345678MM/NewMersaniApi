using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mersani.Utility
{
    public static class Util
    {
        public static string getConnectionString(string _path, string ConnectionStringKey = "OrcleStr")
        {
            try
            {
                if (_path == "")
                {
                    _path = AppDomain.CurrentDomain.BaseDirectory;
                }

                if (File.Exists(_path + "appsettings.json"))
                {
                    var config = new ConfigurationBuilder()
                                  .SetBasePath(Path.GetDirectoryName(_path))
                                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                  .Build();
                    return config[$"ConnectionStrings:{ConnectionStringKey}"];
                }
                else
                {
                    return _path + "appsettings.json";
                }
            }

            catch (Exception exc)
            {
                return _path + "appsettings.json" + exc.Message;
            }
        }

        public static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
        }
    }
}
