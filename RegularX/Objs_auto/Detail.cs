using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class Detail
    {
        public string Tree_Code { get; }
        public string Tree { get; }
        // todo: проверить допусимость int-a
        public string Code { get; }
        public int Count { get; }
        public string Period { get; }
        public string Info { get; }
        // todo: проверить допусимость int-a
        public string Link { get; }
        public string OldLink { get; }

        public Detail(string code, int count, string info, string tree_code, string tree, string period, string link, string oldLink = "")
        {
            Code = code; Count = count; Info = info; Tree_Code = tree_code;
            Tree = tree;  Period = period; Link = Link; OldLink = OldLink; Link = link;
        }



    }
}
