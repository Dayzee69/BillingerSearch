using System;
using System.Windows;

namespace BillingerSearch
{
    class DBConnect
    {

        public string host { get; set; }
        public string port { get; set; }
        public string database { get; set; }
        public string user { get; set; }
        public string password { get; set; }

        public DBConnect()
        {
            try
            {
                string strPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                INIManager manager = new INIManager(strPath + @"\settings.ini");
                host = manager.GetPrivateString("DatabaseConnect", "host");
                port = manager.GetPrivateString("DatabaseConnect", "port");
                database = manager.GetPrivateString("DatabaseConnect", "database");
                user = manager.GetPrivateString("DatabaseConnect", "user");
                password = manager.GetPrivateString("DatabaseConnect", "password");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        

    }
}
