using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POSM
{
    class Backup
    {

       /*public static string path = "C:/";
       public static string file2 = "pos_bak.xml";
       public static string file1 = "pos.xml";*/
       public static FileStream f1;
       public static FileStream f2;


        public static void CheckFile(string orginalFile,string bakFile) {

            f1 = new FileStream(orginalFile, FileMode.Open);
            f2 = new FileStream(bakFile, FileMode.Open);

            bool IsSame = CompareFiles(f1, f2);

            if (IsSame)
            {
                //Console.WriteLine("Same");
                f1.Close();
                f2.Close();

            }
            else
            {
                //Console.WriteLine("file was Different");

                String str = "";
                //File.WriteAllLines(path + f2, File.ReadAllLines(path + f1));
                long len = f1.Length;
                while (len != 0)
                {

                    str += (char)f1.ReadByte();
                    //Console.Write((char)f1.ReadByte());
                    len--;
                }
                f1.Close();
                f2.Close();
                File.WriteAllText(bakFile, str);



            }
                
        }

        private static bool CompareFiles(FileStream f1, FileStream f2)
        {
            int readBytes1;
            int readBytes2;
            if (f1.Length != f2.Length)
            {


                goto sendFalse;
            }

            do
            {
                readBytes1 = f1.ReadByte();
                readBytes2 = f2.ReadByte();
            } while ((readBytes1 == readBytes2) && (readBytes1 != -1));

            if ((readBytes1 - readBytes2) == 0)
            {

                goto sendTrue;


            }
            //f1.Close();


        sendFalse: return false;
        sendTrue: return true;
        }
    }
}
