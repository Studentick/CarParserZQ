using RegularX.Objs_auto;
using RegularX.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RegularX
{
    class Program
    {
        
        public static List<Model> models = new List<Model>();
        static void Main(string[] args)
        {

            string url = @"https://teleprogramma.pro/wp-content/uploads/2015/09/c4ca4238a0b923820dcc509a6f75849b16.jpg";
            using (WebClient client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                string catalog = "images";
                Directory.CreateDirectory(catalog);
                client.DownloadFile(url, "test.jpg");
            }

            //var gg = Controller.GetModels();

            //string ss = "Р’С‹Р±РѕСЂ РєРѕРјРїР»РµРєС‚Р°С†РёРё Р°РІС‚РѕРјРѕР±РёР»СЏ";

            //Controller.clock.Restart();
            //Thread.Sleep(5000);
            //Console.WriteLine(); Console.WriteLine(); Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            //Controller.clock.Stop();
            //Console.WriteLine(Controller.clock.ElapsedMilliseconds);


            //ss = Decoder(ss);
            //Controller.CheckConnectionDB();
            string ss = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|CatalogDB.mdf;Integrated Security=True;Connect Timeout=30";
            using (Controller.sqlConnection = new System.Data.SqlClient.SqlConnection(Controller.con_str))
            {
                Controller.sqlConnection.Open();
                var a = Controller.GetModels();
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        public static string MyDecoder(string input)
        {
            string output = "";
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(input);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            output = win1251.GetString(win1251Bytes);
            return output;
        }
    }
}
