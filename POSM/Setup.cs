using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Collections;

namespace POSM
{
   public class Setup
    {
       public static string path = Macros.DESTI_PATH;
       public static string org_filename = Macros.CONFIG_FILE;
       public static string bak_filename = Macros.CONFIG_BCK_FILE;
       public static int port_no;
       public static int baudrate;
       public static int databits;
       public static string parity = "";
       public static string stopbits = "";
       public static string handshake = "";

       public static ArrayList hotList = new ArrayList();
       public static string bak_hot_file=Macros.HOTKEY_FILE;

      public static XmlDocument xml = new XmlDocument();



       public static void SetSerialProperties(){
           xml.Load(path+bak_filename);
           XmlNodeList list = xml.SelectNodes("poslist");

           foreach (XmlNode node in list)
           {


               XmlElement element = (XmlElement)node;

               XmlNode root = element.GetElementsByTagName("pos")[0];


               port_no = Convert.ToInt32(root["port"]["portnumber"].InnerText);
               baudrate = Convert.ToInt32(root["port"]["baudrate"].InnerText);
               databits = Convert.ToInt32(root["port"]["databits"].InnerText);
               parity = root["port"]["parity"].InnerText;
               stopbits = root["port"]["stopbits"].InnerText;
               handshake = root["port"]["handshake"].InnerText;


               

           }
       }

       public static void LoadHotList(){

           xml.Load(path + bak_hot_file);
           XmlNodeList list = xml.SelectNodes("hot");

           foreach (XmlNode node in list)
           {


               XmlElement element = (XmlElement)node;

               XmlNode root = element.GetElementsByTagName("keys")[0];

               foreach (XmlNode nd in root.ChildNodes) {

                   hotList.Add(nd.InnerText);
                    
               }




           }
       
            // here load hot keys in arraylist
       }

       
    }
}
