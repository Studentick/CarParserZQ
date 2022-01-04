﻿using System;
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

        public List<Complectation> ComplectationsList
        {
            get
            {
                return complectationsList;
            }

            private set
            {
                complectationsList = value;
            }
        }

        private List<Complectation> complectationsList;

        public Model(string modelCode, string name, string link, string period, string compl)
        {
            ModelCode = modelCode;
            Name = name;
            Link = Controller.core_lnk + link;
            Period = Controller.ConvertPeriod(period);
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
            return "";
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
