﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class ComplectationGroups
    {
        public string GroupName { get; }
        public string GroupLink { get; }
        public string Modification { get; }
        
        public List<SubGroup> subgroups;
                                                                 // complectation
        public ComplectationGroups(string name, string link, string modification, int group_code)
        {
            GroupName = name;
            Modification = modification;
            GroupLink = ConvertLink(link, group_code);
            subgroups = new List<SubGroup>();
        }

        public void SetSubgroups(List<SubGroup> subgroups)
        {
            this.subgroups = subgroups;
        }

        public void InsertIntoDB(string parrent_id)
        {
            //Insert into SubGroups(groups_id, name, link)
        }

        private string ConvertLink(string inp, int group_code)
        {
            var m1 = Regex.Match(inp, "(.*?)function=(.*?)&market=(.*?)");

            string func_1 = m1.Groups[2].Value; // +"function=" // на всякий случай
            string func_2 = "getSubGroups";

           
            Regex r = new Regex(func_1);

            inp = r.Replace(inp, func_2) + "&group=" + Convert.ToString(group_code);

            //string pattern = @"\s+";
            //string target = " ";
            //Regex regex = new Regex(pattern);
            //string result = regex.Replace(text, target);

            return inp;
        }

        public void InsertIntoDB()
        {

        }

    }
}
