using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSM
{
    class Macros
    {

        public static string SOURCE_PATH = "\\\\192.168.0.110\\C$\\Users\\Admin\\Documents\\";
        public static string DESTI_PATH = "C:/Users/Admin/Documents/";
        public static string DB_BCK_PATH = "C:/DB/";
        public static string HOTKEY_FILE = "hot.xml";
        public static string HOTKEY_BCK_FILE = "hot_bak.xml";
        public static string CONFIG_FILE = "pos.xml";
        public static string CONFIG_BCK_FILE = "pos_bak.xml";
        public static string DB_FILE = "posDB.bak";
        public static string DB_BCK_FILE = "posDB_bak.bak";
        public static string LOCAL_DB_CONN = "Data Source=LAPTOP-EMJVR3G8;Initial Catalog=posDB;Integrated Security=True";
        public static string CLOUD_DB_CONN = "Data Source=gticloudmaster.cx2ay4llxkng.us-east-2.rds.amazonaws.com,1433;Initial Catalog=dbBackOffice;User ID = gticloud; Password = gticloud";
    }
}
