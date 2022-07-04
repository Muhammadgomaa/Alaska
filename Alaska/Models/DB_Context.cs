using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Alaska.Models
{
    public partial class DB_Context : DbContext
    {
        public DB_Context()
            : base("name=DB_Context")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Invoice_Details> Invoice_Details { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Refund_Details> Refund_Details { get; set; }
        public virtual DbSet<Shipping_Details> Shipping_Details { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Refund_Details)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Shipping_Details)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
