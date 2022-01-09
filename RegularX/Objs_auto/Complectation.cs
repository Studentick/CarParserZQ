using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class Complectation
    {
        public string Model { get; }
        public string Modification { get; }
        public string Link{ get; }
        public string Period { get; }
        public Dictionary<string, string> complectParams;
        public List<ComplectationGroups> complectationGroups;

        public Complectation(string model, string modification, 
            string link, string period, Dictionary<string, string> complectParams, int number)
        {
            Model = model;
            Modification = modification;
            Period = Controller.ConvertPeriod(period);
            Link = ConvertLink(link, number);
            this.complectParams = complectParams;
            this.complectationGroups = new List<ComplectationGroups>();
        }

        // Перевод параметров автомобилей в JSON
        public string DictToJson(Dictionary<string, string> dict)
        {
            string json = "{";
            foreach (var it in dict)
            {
                json += $"\"{it.Key}\": \"{it.Value}\", ";
            }
            json = json.Remove(json.Length - 2);
            json += "}";
            return json;
        }

        // Внесение строк в БД о комплектации
        public void InsertIntoDB()
        {
            string json_params = DictToJson(complectParams);

            string str_comand = "INSERT INTO complectations (complectation_code, model_code, f_period, params)" +
                "VALUES (@complectation_code, @model_code, @f_period, @params)";
            SqlCommand sqlComand = new SqlCommand(str_comand, Controller.sqlConnection);

            sqlComand.Parameters.AddWithValue("complectation_code", this.Modification);
            sqlComand.Parameters.AddWithValue("model_code", this.Model);
            sqlComand.Parameters.AddWithValue("f_period", this.Period);
            sqlComand.Parameters.AddWithValue("params", json_params);

            try
            {
                sqlComand.ExecuteNonQuery();
            } // В дальнейшем этоту обработку планирую перенести в хранимую процедуру или в триггер
            catch (Exception ex) { }
        }

        // Снятие защиты с сылки
        private string ConvertLink(string inp, int number)
        {
            var m1 = Regex.Match(inp, "(.*?)&market=(.*?)&model=(.*?)&modification=(.*?)&complectation=(.*?)$");

            string market_1 = m1.Groups[2].Value;
            string model_1 = m1.Groups[3].Value;
            string modification_1 = m1.Groups[4].Value;
            string complectation_1 = m1.Groups[5].Value;

            string postfix_1 = $"&model={model_1}&modification={modification_1}&complectation={complectation_1}";
            string postfix_2 = $"&model={this.Model}&modification={this.Modification}&complectation={FormatNumber(number)}";

            Regex r = new Regex(postfix_1);
            inp = r.Replace(inp, postfix_2);

            return Controller.core_lnk + inp;
        }

        // Преобразование кода в текстовый формат 3-знакового числа
        private string FormatNumber(int number)
        {
            string output = ("000" + Convert.ToString(number));
            output = output.Substring(output.Length - 3, 3);
            return output;
        } 

    }
}
