using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class SubGroup
    {
        public string Name { get; }
        public string Link { get; }

        public List<Detail> details;

        public SubGroup(string name, string link, string sg_code)
        {
            Name = name;
            Link = ConvertLink(link, sg_code);
            details = new List<Detail>();
        }

        public void SetDetails(List<Detail> details)
        {
            //this.details.AddRange(details);
        }

        public int InsertIntoDB(string group_name)
        {
            string str_comand = "INSERT INTO subgroups (name, group_name)" +
                "VALUES (@name, @group_name)";
            SqlCommand sqlComand = new SqlCommand(str_comand, Controller.sqlConnection);

            sqlComand.Parameters.AddWithValue("name", this.Name);
            sqlComand.Parameters.AddWithValue("group_name", group_name);

            // Для исключения ошибки, связанной с дублированием данных, но...
            try
            {
                sqlComand.ExecuteNonQuery();
            } // в дальнейшем этоту обработку планирую перенести в хранимую процедуру или в триггер
            catch (Exception ex) { }
            int res = -1;
            str_comand = $"SELECT id FROM subgroups WHERE name = N'{this.Name}' AND group_name = N'{group_name}'";
            sqlComand = new SqlCommand(str_comand, Controller.sqlConnection);
            res = Convert.ToInt32(sqlComand.ExecuteScalar());
            //sqlComand.ExecuteScalar();
            return res;
        }

        private string ConvertLink(string inp, string sg_code)
        {
            var m1 = Regex.Match(inp, "(.*?)function=(.*?)&market=(.*?)");

            string func_1 = m1.Groups[2].Value; // +"function=" // на всякий случай
            string func_2 = "getParts";


            Regex r = new Regex(func_1);

            inp = r.Replace(inp, func_2) + "&subgroup=" + sg_code;

            //string pattern = @"\s+";
            //string target = " ";
            //Regex regex = new Regex(pattern);
            //string result = regex.Replace(text, target);

            return inp;
        }
    }
}
