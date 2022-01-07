using System;
using System.Collections.Generic;
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


        // Нужно дописать конверторы
        // И переделать логику, т.к. оказалось, что у авто, выпущенных в разные годы
        // по разному спроектированы таблицы с комплектацией
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

        public void SetGroups()
        {

        }

        public void InsertIntoDB()
        {

        }

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

            //string pattern = @"\s+";
            //string target = " ";
            //Regex regex = new Regex(pattern);
            //string result = regex.Replace(text, target);

            return Controller.core_lnk + inp;
        }

        private string FormatNumber(int number)
        {
            string output = ("000" + Convert.ToString(number));
            output = output.Substring(output.Length - 3, 3);
            return output;
        } 

    }
}
