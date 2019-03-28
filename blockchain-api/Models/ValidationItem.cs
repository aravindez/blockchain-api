using System;
namespace blockchainapi.Models
{
    public class ValidationItem
    {
        public int user_id { get; set; }
        public int block_id { get; set; }
        public int isValid { get; set; }
    }
}