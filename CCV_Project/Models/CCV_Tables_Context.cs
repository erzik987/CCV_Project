using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CCV_Project.Models
{
    public class CCV_Tables_Context : DbContext
    {
        public CCV_Tables_Context() : base("CCV_DB")
        {

        }

        public  DbSet<UserAcount> UserAcounts { get; set; }
        public DbSet<StoreHouse> StoreHouses { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
    }
}
