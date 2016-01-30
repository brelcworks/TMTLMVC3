



namespace MVCINCV4._1.Models
{
    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
    using MVCINCV4._1.Models;
    public class DBCTX : DbContext
    {
        public DBCTX()
            : base("name=DBCTX")
        {
            Database.SetInitializer<DBCTX>(new DropCreateDatabaseIfModelChanges<DBCTX>());
        }
        public DbSet<user2> user { get; set; }
    }
}