using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XinTuo.Finance.Models
{
    public class MEasyuiTree
    {
        public MEasyuiTree()
        {
            attributes = new System.Collections.Hashtable();
            children = new List<MEasyuiTree>();
        }

        public int id
        {
            get; set;
        }

        public string text
        {
            get; set;
        }

        public string state
        {
            get; set;
        }

        public bool @checked
        {
            get;set;
        }

        public System.Collections.Hashtable attributes
        {
            get;set;
        }

        public List<MEasyuiTree> children
        {
            get;set;
        }
    }
}