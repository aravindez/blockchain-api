using System;
using System.Collections.Generic;

namespace blockchainapi.Models
{
    public class ChainItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int created_by { get; set; }
        public int initData { get; set; }
        public List<int> users { get; set; }
        public List<string> groups { get; set; }
    }
}
