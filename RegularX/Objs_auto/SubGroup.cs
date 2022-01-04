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

        public List<Detail> Details
        {
            get
            {
                return details;
            }
            private set
            {
                details = value;
            }
        }

        List<Detail> details;

        public SubGroup(string name, string link)
        {
            Name = name;
            Link = Controller.core_lnk + link;
            Details = new List<Detail>();
        }

        public void SetDetails(List<Detail> details)
        {
            //this.details.AddRange(details);
        }

        public void InsertIntoDB(string parrent_id)
        {
            //Insert into SubGroups(groups_id, name, link)

            //foreach(var it in this.details)
            //{
            //    it.InsertToDB(parrent_id);
            //}

        }
    }
}
