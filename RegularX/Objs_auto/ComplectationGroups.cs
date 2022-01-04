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

        public List<SubGroup> Subgroups
        {
            get
            {
                return subgroups;
            }

            private set
            {
                subgroups = value;
            }
        }

        private List<SubGroup> subgroups;

        public ComplectationGroups(string name, string link)
        {
            GroupName = name;
            GroupLink = Controller.core_lnk + link;
            Subgroups = new List<SubGroup>();
        }

        public void SetSubgroups(List<SubGroup> subgroups)
        {
            this.subgroups = subgroups;
        }

        public void InsertIntoDB(string parrent_id)
        {
            //Insert into SubGroups(groups_id, name, link)
        }

    }
}
