using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class SubGroup
    {
        public string Name { get; }
        public string Link { get; }

        public SubGroup(string name, string link)
        {
            Name = name;
            Link = link;
        }
    }
}
