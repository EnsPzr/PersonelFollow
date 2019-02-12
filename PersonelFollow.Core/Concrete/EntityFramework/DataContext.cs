using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Concrete.EntityFramework
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<UserInformation> UserInformations { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityFollow> ActivityFollows { get; set; }
    }
}
