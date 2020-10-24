﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameConsuleWebStore.Models;

namespace GameConsuleWebStore.Data
{
    public class GameConsuleWebStoreContext : DbContext
    {
        public GameConsuleWebStoreContext (DbContextOptions<GameConsuleWebStoreContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>().HasKey(p => new { p.ProductId, p.OrderId });
            modelBuilder.Entity<ProductOrder>().HasOne(p => p.Product).WithMany(p => p.ProductOrders).HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<ProductOrder>().HasOne(p => p.Order).WithMany(p => p.ProductOrders).HasForeignKey(p => p.OrderId);

        }

        public DbSet<GameConsuleWebStore.Models.Product> Product { get; set; }

        public DbSet<GameConsuleWebStore.Models.User> User { get; set; }

        public DbSet<GameConsuleWebStore.Models.Order> Order { get; set; }

        public DbSet<GameConsuleWebStore.Models.ProductOrder> ProductOrder { get; set; }
        public DbSet<GameConsuleWebStore.Models.StoreAddress> StoreAddress { get; set; }
    }
}
