using System;
namespace blockchainapi.Models
{
    public class UserItem
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string name { get; set; }
        public int admin { get; set; }
    }
}
