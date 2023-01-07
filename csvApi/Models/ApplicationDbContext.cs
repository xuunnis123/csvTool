using System;
using Microsoft.EntityFrameworkCore.SqlServer;

using Microsoft.EntityFrameworkCore;

namespace csvApi.Models
{
	public class ApplicationDbContext : DbContext
    {


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		//      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//      {
		//          optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
		//      }
		public DbSet<Visitor> Visitors { get; set; }
	}
}

