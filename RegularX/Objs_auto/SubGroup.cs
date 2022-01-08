using System;
using System.Collections.Generic;
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

        public void InsertIntoDB(/*string parrent_id*/)
        {
            //Insert into SubGroups(groups_id, name, link)

            //foreach(var it in this.details)
            //{
            //    it.InsertToDB(parrent_id);
            //}

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
