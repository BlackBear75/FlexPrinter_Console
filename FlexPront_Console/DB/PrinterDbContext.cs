using FlexPrint_Console.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlexPrint_Console.DB
{

	public class PrinterDbContext : DbContext
	{
	
			private readonly IConfiguration _configuration;
		public DbSet<Printer> Printers { get; set; }
		public DbSet<InkjetPrinter> InkjetPrinters { get; set; }
		public DbSet<LaserPrinter> LaserPrinters { get; set; }

		public PrinterDbContext(IConfiguration configuration)
			{
				_configuration = configuration;
			}

			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				if (!optionsBuilder.IsConfigured)
				{
					string connectionString = _configuration.GetConnectionString("DefaultConnection");
					optionsBuilder.UseSqlServer(connectionString);
				}
			}
		
	}
}