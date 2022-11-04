using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using pyfa.auth.libs;

namespace pyfa.auth.libs
{
    public class lDbConn
    {
        private lConvert lc = new lConvert();
   
  
        public string conStringLog()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            var provider = config.GetSection("SqlProvider:provider").Value.ToString();
            var strname = config.GetSection("constringName:pyfahome").Value.ToString();
            var configDB = config.GetSection("DbContextSettings:" + provider + ":" + strname).Value.ToString();
            return configDB;
        }

        public string domainGetApi(string api)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("APISettings:" + api).Value.ToString();
        }

        public string domainGetTokenCredential(string param)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return config.GetSection("TokenAuthentication:" + param).Value.ToString();
        }

        public string domainGetApi2(string api)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            return "" + config.GetSection("APISettings:" + api).Value.ToString();
        }

        

        public string GetNotifSetting(string data)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:" + data).Value.ToString();
        }

        public string constringName(string cstr)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("constringName:" + cstr).Value.ToString();
        }

        #region -- connnection string by database --
        public string sqlprovider()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("SqlProvider:provider").Value.ToString();
        }


        public string constringList(string dbprv, string strname)
        {

            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            //var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());

            var configDB = config.GetSection("DbContextSettings:" + dbprv + ":" + strname).Value.ToString();

            //var repPass = configDB.Replace("{pass}", configPass);
            return "" + configDB;
        }
      


        #endregion


    }
}
