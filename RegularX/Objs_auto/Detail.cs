using RegularX.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularX.Objs_auto
{
    class Detail
    {
        public string Tree_Code { get; }
        public string Tree { get; }
        // todo: проверить допусимость int-a
        public string Code { get; private set; }
        public string OldCode { get; private set; }
        public int Count { get; }
        public string Period { get; }
        public string Info { get; }
        // todo: проверить допусимость int-a
        public string Link { get; private set; }
        public string OldLink { get; private set; }

        public Detail(string code, int count, string info, string tree_code, string tree, string period)
        {
            Count = count; Info = info; Tree_Code = tree_code;
            Tree = tree;  Period = period; OldLink = OldLink;
            ConvertCode(code);
        }

        

        protected void ConvertCode(string raw_code)
        {
            string pattern = "replaceNumber";

            bool a = raw_code.Contains(pattern);
            if (a)
            {
                var code = Regex.Match(raw_code,
                    //1                                 2       3   4                                           5           6       7   8
                    "(.*?)<div class='number'><a href='(.*?)'(.*?)>(.*?)</a></div><div class='replaceNumber'>(.*?)<a href='(.*?)'(.*?)>(.*?)</a></div>"
                    );
                Code = code.Groups[4].Value;
                OldCode = code.Groups[8].Value;
                Link = code.Groups[6].Value;
                OldLink = code.Groups[2].Value;
            }
            else
            {
                var code = Regex.Match(raw_code,
                    "(.*?)<div class='number'><a href='(.*?)'(.*?)>(.*?)</a></div>"
                    );
                Code = code.Groups[4].Value;
                Link = code.Groups[2].Value;
            }
        }



    }
}
