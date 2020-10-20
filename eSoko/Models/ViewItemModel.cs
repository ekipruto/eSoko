using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSoko.Models
{
    public class ViewItemModel
    {
        public int proid { get; set; }
        public string proname { get; set; }
        public string proimage { get; set; }
        public string prodescr { get; set; }
        public Nullable<int> proprice { get; set; }
        public string procontact { get; set; }
        public Nullable<int> profkcatid { get; set; }
        public Nullable<int> profkuserid { get; set; }
        public int catid { get; set; }
        public string catname { get; set; }
        public string username { get; set; }
        public string userimage { get; set; }
        public string usercontact { get; set; }
    }
}