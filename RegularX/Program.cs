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
            using (Controller.sqlConnection = new System.Data.SqlClient.SqlConnection(Controller.con_str))
            {
                Controller.sqlConnection.Open();
                var a = Controller.GetModels();
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        // Для очистки данных полученных с сайта от "Краказябры"
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
