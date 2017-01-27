using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace Applr.Models
{
    public class Post
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Content { get; set; }

        public bool IsBad()
        {
            return true;
        }
    }

    public class PostDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public System.Data.Entity.DbSet<Applr.Models.Comment> Comments { get; set; }
    }
}