using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class ComplectationGroups
    {
        public string GroupName { get; }
        public string GroupLink { get; }

        public ComplectationGroups(string name, string link)
        {
            GroupName = name;
            GroupLink = Controller.core_lnk + link;
        }


    }
}
