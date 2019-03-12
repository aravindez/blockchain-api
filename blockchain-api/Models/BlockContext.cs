using System;
using Microsoft.EntityFrameworkCore;

namespace blockchainapi.Models
{
    public class BlockContext : DbContext
    {
        public BlockContext(DbContextOptions<BlockContext> options)
            : base(options)
        {
        }

        public DbSet<BlockItem> BlockItems { get; set; }
    }
}
