using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Model(string modelCode, string name, string link, string period, string compl)
        {
            ModelCode = modelCode;
            Name = name;
            Link = link;
            Period = period;
        }

        public void Print()
        {
            Console.WriteLine($"Mpdel code: {ModelCode}\nName: {Name}\nPeriod: {Period}\nLink: {Link}\n");
        }

        private string ConvertLink(string inp)
        {
            return "";
        }

        private string ConvertPeriod(string inp)
        {
            return "";
        }


    }
}
