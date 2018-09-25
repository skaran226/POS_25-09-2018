using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSM
{
    class Startup
    {
        
        public static string org_pos_config = Macros.DESTI_PATH+Macros.CONFIG_FILE;
        public static string bak_pos_config = Macros.DESTI_PATH + Macros.CONFIG_BCK_FILE;
        public static string org_hot = Macros.DESTI_PATH+Macros.HOTKEY_FILE;
        public static string bak_hot = Macros.DESTI_PATH + Macros.HOTKEY_BCK_FILE;

        public static void SetUp() {

            Backup.CheckFile( bak_pos_config,  org_pos_config);
            Backup.CheckFile( bak_hot,  org_hot);
            Setup.SetSerialProperties();
            Setup.LoadHotList();
            CallbackThreads.TimerCallbackThreading();
          
        }
    }
}
