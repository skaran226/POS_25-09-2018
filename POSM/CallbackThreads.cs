using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace POSM
{
    class CallbackThreads
    {


        public static bool FileCopySuccess = false;
        public static bool db_backup = true;
        public static bool checkhotkey = true;
        public static bool checkhotkey_cloud = true;
        public static bool deviceconfig = true;
        public static bool deviceconfig_cloud = true;
        public static bool CopyFileExep = false;
        public static bool updatepend_config = false;
        public static bool updatepend_hotkey = false;
        public static bool updatepend_config_cloud = false;
        public static bool updatepend_hotkey_cloud = false;
        public static TimerCallback timerDelegate = null, timerDelegate1 = null, timerDelegate2 = null, timerDelegate_cloud1 = null, timerDelegate_cloud2 = null;
        public static Timer PumpTimer = null, checkHotKey = null, device_config = null, checkHotKey_cloud = null, device_config_cloud = null;



        public static void TimerCallbackThreading()
        {
            timerDelegate = new TimerCallback(Db_Backup);
            PumpTimer = new Timer(timerDelegate, null, 1801000, 1801000); // 1801000, 1801000-> 1hours 1 sec


            timerDelegate_cloud1 = new TimerCallback(checkHotKeyPromotionFromCloud);
            checkHotKey_cloud = new Timer(timerDelegate_cloud1, null, 1740000, 1740000); //29 minutes

            timerDelegate_cloud2 = new TimerCallback(device_config_check_FromCLoud);
            device_config_cloud = new Timer(timerDelegate_cloud2, null, 5076000, 5076000);  //1.41 hours

            /* if (checkhotkey)
             {

                 timerDelegate1 = new TimerCallback(checkHotKeyPromotion);
                 checkHotKey = new Timer(timerDelegate1, null, 2040000, 2040000); //34 minutes
             }*/





            /* if (deviceconfig)
             {

                 timerDelegate2 = new TimerCallback(device_config_check);
                 device_config = new Timer(timerDelegate2, null, 5400000, 5400000);  //1.5 hours
             }*/



        }

        private static void device_config_check_FromCLoud(object state)
        {
            //deviceconfig_cloud = false;
            device_config_cloud.Change(Timeout.Infinite, Timeout.Infinite);
            string query = "select SiteKey,APP,filepath,Updatepending from Configuration where SiteKey=2 and APP='devconfig'";
            string updatepending_cloud = "update Configuration set Updatepending=0 where SiteKey=2 and APP='devconfig'";
            //string updatepending_local = "insert into ConfigurationTables (SiteKey,APP,filepath,Updatepending) values(2,'devconfig','C:/Users/Admin/Documents/pos.xml',1)";

            try
            {

                DB.CloseConn();
                SqlCommand cmd = Cloud_DB.ExecuteReader(query);
                SqlDataReader dbr = cmd.ExecuteReader();


                if (dbr.HasRows)
                {

                    while (dbr.Read())
                    {

                        if (dbr["Updatepending"].Equals(true))
                        {

                            DownloadFile(dbr["filepath"].ToString(), Macros.DESTI_PATH + Macros.CONFIG_FILE);
                            if (CopyFileExep)
                            {

                                //DB.CloseConn();
                                //DB.ExecuteNonQuery(updatepending_local);
                                Cloud_DB.CloseConn();
                                Cloud_DB.ExecuteNonQuery(updatepending_cloud);
                                updatepend_config_cloud = true;
                            }



                        }
                        device_config_cloud.Change(5076000, 5076000);

                    }
                    if (CopyFileExep && updatepend_config_cloud)
                    {
                        FileCopySuccess = true;
                        updatepend_config_cloud = false;
                    }


                }
                else
                {

                    Debug.WriteLine("Not Found data in Config table");
                }
                DB.CloseConn();
                cmd.Dispose();
                dbr.Dispose();


            }
            catch (Exception ex)
            {

                Debug.WriteLine("Not read data in Config table");
            }


        }

        private static void checkHotKeyPromotionFromCloud(object state)
        {
            //checkhotkey_cloud = false;
            checkHotKey_cloud.Change(Timeout.Infinite, Timeout.Infinite);
            string query = "select SiteKey,APP,filepath,Updatepending from Configuration where SiteKey=3 and APP='hotkeylist'";
            string updatepending_cloud = "update Configuration set Updatepending=0 where SiteKey=3 and APP='hotkeylist'";
            // string updatepending_local = "insert into ConfigurationTables (SiteKey,APP,filepath,Updatepending) values(1,'hotkeylist','C:/Users/Admin/Documents/hot.xml',1)";

            try
            {


                SqlCommand cmd = Cloud_DB.ExecuteReader(query);
                SqlDataReader dbr = cmd.ExecuteReader();


                if (dbr.HasRows)
                {

                    while (dbr.Read())
                    {

                        if (dbr["Updatepending"].Equals(true))
                        {

                            DownloadFile(dbr["filepath"].ToString(), Macros.DESTI_PATH + Macros.HOTKEY_FILE);
                            if (CopyFileExep)
                            {
                                // DB.CloseConn();
                                //DB.ExecuteNonQuery(updatepending_local);
                                Cloud_DB.CloseConn();
                                Cloud_DB.ExecuteNonQuery(updatepending_cloud);
                                updatepend_hotkey_cloud = true;
                            }



                        }


                    }
                    if (CopyFileExep && updatepend_hotkey_cloud)
                    {
                        FileCopySuccess = true;
                        updatepend_hotkey_cloud = false;
                    }
                    checkHotKey_cloud.Change(1740000, 1740000);

                }
                else
                {

                    Debug.WriteLine("Not Found data in Config table");
                }

                cmd.Dispose();
                dbr.Dispose();

            }
            catch (Exception ex)
            {

                Debug.WriteLine("Not read data in Config table");
            }

        }






        private static void Db_Backup(object state)
        {
            PumpTimer.Change(Timeout.Infinite, Timeout.Infinite);
            //db_backup = false;

            string sPassQuery = "backup database posDB to disk='" + Macros.DB_BCK_PATH + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond + "_" + Macros.DB_FILE + "'";

            Cloud_DB.CloseConn();
            Cloud_DB.ExecuteNonQuery(sPassQuery);
            Debug.WriteLine("Cloud backup successful!!");

            PumpTimer.Change(1801000, 1801000);
            //db_backup = true;

            //Backup Script

            //
        }

        public static void CopyPaste(string source, string destination)
        {

            try
            {


                File.Copy(source, destination, true);
                Debug.WriteLine("File copy in destination folder");
                CopyFileExep = true;

            }
            catch (Exception ex)
            {

                Debug.WriteLine("can't copy file");
            }

        } //only for local

        public static void DownloadFile(string source, string destination)
        {


            try
            {

                using (WebClient webclient = new WebClient())
                {

                    webclient.DownloadFile(source, destination);
                    CopyFileExep = true;
                }
                Debug.WriteLine("File download successfully");
            }
            catch (Exception ex)
            {

                Debug.WriteLine("can't download file");
            }

        } // only for cloud


        /*private static void device_config_check(object state)
{
    deviceconfig = false;
    string query = "select SiteKey,APP,filepath,Updatepending from ConfigurationTables where SiteKey=2 and APP='devconfig'";
    string updatepending = "update ConfigurationTables set Updatepending=0 where SiteKey=2 and APP='devconfig'";

    try
    {


        SqlCommand cmd = DB.ExecuteReader(query);
        SqlDataReader dbr = cmd.ExecuteReader();


        if (dbr.HasRows)
        {

            while (dbr.Read())
            {

                if (dbr["Updatepending"].Equals(true))
                {

                    //CopyPaste(dbr["filepath"].ToString(), Macros.DESTI_PATH + Macros.CONFIG_FILE);
                            
                        DB.CloseConn();
                        DB.ExecuteNonQuery(updatepending);
                        updatepend_config = true;

                            


                }


            }
            if (updatepend_config)
            {
                FileCopySuccess = true;
                updatepend_config = false;
            }
            deviceconfig = true;

        }
        else
        {

            Debug.WriteLine("Not Found data in Config table");
        }

        cmd.Dispose();
        dbr.Dispose();

    }
    catch (Exception ex)
    {

        Debug.WriteLine("Not read data in Config table");
    }



    //check new device config file  from local db

    //first hit configuration table and get device config xml file accoring site key and app name

    //store xml file from destination folder

    // restart the app
}
*/
        /*  private static void checkHotKeyPromotion(object state)
          {

              checkhotkey = false;
              string query = "select SiteKey,APP,filepath,Updatepending from ConfigurationTables where SiteKey=1 and APP='hotkeylist'";
              string updatepending = "update ConfigurationTables set Updatepending=0 where SiteKey=1 and APP='hotkeylist'";

              try
              {


                  SqlCommand cmd = DB.ExecuteReader(query);
                  SqlDataReader dbr = cmd.ExecuteReader();


                  if (dbr.HasRows)
                  {

                      while (dbr.Read())
                      {

                          if (dbr["Updatepending"].Equals(true))
                          {

                             // CopyPaste(dbr["filepath"].ToString() , Macros.DESTI_PATH + Macros.HOTKEY_FILE);
                            
                                  DB.CloseConn();
                                  DB.ExecuteNonQuery(updatepending);
                                 // CopyFileExep = true;
                                  updatepend_hotkey = true;

                            



                          }


                      }
                      if (updatepend_hotkey)
                      {
                          FileCopySuccess = true;
                          updatepend_hotkey = false;
                      }
                      checkhotkey = true;

                  }
                  else
                  {

                      Debug.WriteLine("Not Found data in Config table");
                  }

                  cmd.Dispose();
                  dbr.Dispose();

              }
              catch (Exception ex)
              {
                  Debug.WriteLine("Not read data in Config table");

              }

              //check new hot keys promotion from local db

              //first hit configuration table and get hot key xml file accoring site key

              //store xml file from destination folder


              //restart the app
          }

          */





    }
}
