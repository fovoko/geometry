using System;
using System.Text.Json;
using Gems.SqliteDb.Conversions;
using Gems.SqliteDb.Models;
using Microsoft.EntityFrameworkCore;

namespace Gems.SqliteDb.Db
{
	public class FigureDbContext : DbContext
	{
		public DbSet<FigureDto> Figures { get; set; }

		public FigureDbContext() 
		{
		}

		public FigureDbContext( DbContextOptions<FigureDbContext> options ) : base( options )
		{
		}

		protected override void OnConfiguring( DbContextOptionsBuilder options )
		{
			if ( !options.IsConfigured )
			{
				options.UseSqlite( "Data Source=geometry.db" );
			}
		}

		protected override void OnModelCreating( ModelBuilder modelBuilder )
		{
            base.OnModelCreating( modelBuilder );
            modelBuilder.Entity<FigureDto>()
                .Property( b => b.Geometry )
                .HasJsonConversion();
		}
	}
}