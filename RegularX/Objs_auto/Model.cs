using System;
using System.Collections.Generic;
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
            var m = Regex.Match(inp, "(.*?)&market=EU&model=(.*?)&startDate=(.*?)&endDate=(.*?)$");
            string startDate_1 = m.Groups[3].Value;
            string endDate_1 = m.Groups[4].Value;
            string model_1 = m.Groups[2].Value;

            var m1 = Regex.Match(inp, "(.*?)&market=(.*?)&(.*?)$");


            var startDate_2 = new Regex("startDate=" + startDate_1);
            inp = startDate_2.Replace(inp, "startDate=" + Period.Substring(3,4) + Period.Substring(0, 2));

            var endDate_2 = new Regex("endDate=" + endDate_1);
            inp = endDate_2.Replace(inp, "endDate=" + Period.Substring(11, 4) + Period.Substring(8, 2));

            var model_2 = new Regex("model=" + model_1);
            inp = model_2.Replace(inp, "model=" + ModelCode);
            ConvertLink1(inp);
            return Controller.core_lnk + inp;
        }

        private string ConvertLink1(string inp)
        {
            var m1 = Regex.Match(inp, "(.*?)&market=(.*?)&(.*?)$");

            string market = m1.Groups[2].Value;
            string startDate_1 = m1.Groups[3].Value;

            var startDate_2 = new Regex(startDate_1);
            string new_str = "model=" + ModelCode + "&startDate =" + Period.Substring(3, 4) + Period.Substring(0, 2)+
                "endDate=" + Period.Substring(11, 4) + Period.Substring(8, 2);
            inp = startDate_2.Replace(inp, "");

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

        }

    }
}
