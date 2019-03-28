using System;
namespace blockchainapi.Models
{
    public class BlockItem
    {
        public int id { get; set; }
        public string previous_hash { get; set; }
        public string timestamp { get; set; }
        public int created_by { get; set; }
        public string user_name { get; set; }
        public int data { get; set; }
        public string hash { get; set; }
        public int nonce { get; set; }
        public int chain_id { get; set; }
        public int isValid { get; set; }
    }
}
