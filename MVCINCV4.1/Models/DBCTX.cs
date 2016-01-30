



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
        public DbSet<TABLE2> STOCK { get; set; }
        public  DbSet<BILL> BILL { get; set; }
        public  DbSet<BILL1> BILL1 { get; set; }
        public  DbSet<CUST> CUST { get; set; }
        public  DbSet<CUSTOMER> CUSTOMER { get; set; }
        public  DbSet<MAINPOPU> MAINPOPU { get; set; }
        public  DbSet<PMR> PMR { get; set; }
        public  DbSet<PURCHSE> PURCHSE { get; set; }
        public  DbSet<PURCHSE1> PURCHSE1 { get; set; }
        public  DbSet<SHEET1> SHEET1 { get; set; }
    }
}