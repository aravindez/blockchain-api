﻿using System;
using Microsoft.EntityFrameworkCore;

namespace blockchainapi.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<UserItem> UserItems { get; set; }
    }
}
