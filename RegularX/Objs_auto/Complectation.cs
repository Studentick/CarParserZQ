using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class Complectation
    {
        public string Model { get; }
        public string Link{ get; }
        public string Period { get; }
        public string Engine1 { get; }
        public string Body { get; }
        public string Grade { get; }
        public string AtmMtmGearShiftType { get; }
        public string Cab { get; }
        public string TransmissionModel { get; }
        public string LoadingCapacity { get; }
        public string GearTire { get; }
        public string Destination { get; }
        public string FuelInduction { get; }
        public string BuildingCondition { get; }


        // Нужно дописать конверторы
        // И переделать логику, т.к. оказалось, что у авто, выпущенных в разные годы
        // по разному спроектированы таблицы с комплектацией
        public Complectation(string model, string link, string period, string engine1, string body)
        {
            Model = model;
            Link = link;
            Period = period;
            Engine1 = engine1;
            Body = body;
        }


    }
}
