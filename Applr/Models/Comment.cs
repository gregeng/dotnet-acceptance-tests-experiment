using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Applr.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [ForeignKey("Post")]
        [DisplayName("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public string Content { get; set; }
    }

    public class CommentDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
    }
}