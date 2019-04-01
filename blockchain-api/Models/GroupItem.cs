using System;
using System.Collections.Generic;

namespace blockchainapi.Models
{
    public class GroupItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<string> users { get; set; }
    }
}
