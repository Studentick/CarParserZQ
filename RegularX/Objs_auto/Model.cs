using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class Model
    {
        const string PreLink = "www.ilcats.ru";
        // objs.Groups[4].Value = "28B510" - onlyString
        public string ModelCode { get; }
        public string Name { get; }
        public string Link { get; }
        public string Period { get; }
        public string Complectation { get; }

        public List<Complectation> complectationsList;

        public Model(string modelCode, string name, string link, string period, string compl)
        {
            ModelCode = modelCode;
            Name = name;
            Period = Controller.ConvertPeriod(period);
            Link = ConvertLink(link);
            //Link = Controller.core_lnk + link;
            Complectation = compl;
            complectationsList = new List<Complectation>();
        }

        public void SetComplList(List<Complectation> complectationList)
        {
            this.complectationsList.AddRange(complectationList);
        }

        public void Print()
        {
            Console.WriteLine($"Mpdel code: {ModelCode}\nName: {Name}\nPeriod: {Period}\nLink: {Link}\n");
        }

        private string ConvertLink(string inp)
        {
            string model = ""; string startDate = ""; string endDate = "";
            var m1 = Regex.Match(inp, "(.*?)&market=(.*?)&(.*?)$");

            string market = m1.Groups[2].Value;
            string postfix_1 = m1.Groups[3].Value;

            var _postfix = new Regex(postfix_1);

            model = "model=" + ModelCode;
            startDate = "startDate=" + Period.Substring(3, 4) + Period.Substring(0, 2);
            try { endDate = "&endDate=" + Period.Substring(11, 4) + Period.Substring(8, 2); } catch (Exception) { endDate = ""; }

            string postfix_2 = $"{model}&{startDate}{endDate}";

            // На всякий случай:
            // postfix_2 = postfix_2 +( endDate != "" ? /*"&" +*/ endDate : "");

            inp = _postfix.Replace(inp, postfix_2);

            return Controller.core_lnk + inp;
        }

        private string ConvertPeriod(string inp)
        {
            return "";
        }

        public void SetComplectations()
        {

        }

        public void InsertIntoDB()
        {
            string str_comand = "INSERT INTO models (model_code, f_period, model_name, complectation_cipher)" +
                "VALUES (@model_code, @f_period, @model_name, @complectation_cipher)";
            SqlCommand sqlComand = new SqlCommand(str_comand, Controller.sqlConnection);

            sqlComand.Parameters.AddWithValue("model_code", this.ModelCode);
            sqlComand.Parameters.AddWithValue("f_period", this.Period);
            sqlComand.Parameters.AddWithValue("model_name", this.Name);
            sqlComand.Parameters.AddWithValue("complectation_cipher", this.Complectation);

            
            try
            {
                sqlComand.ExecuteNonQuery();
            } // В дальнейшем этоту обработку планирую перенести в хранимую процедуру или в триггер
            catch (Exception ex) { }
        }

    }
}
