using FlexPrint_Console.DB;
using FlexPrint_Console.Enum;
using FlexPrint_Console.Manager;
using FlexPrint_Console.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace FlexPrint_Console
{
	public class FlexPrint_Console
	{


		static void Main()
		{

			var configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();

			EnsureDatabaseCreated(configuration);

			IPrinterManager printerManager = new PrinterManager(configuration);

			




		}
		static void PrintPrinter_Console(List<Printer> printers)
		{
            foreach (var item in printers)
            {
                Console.WriteLine(item);
            }

        }
		static void EnsureDatabaseCreated(IConfiguration configuration)
		{
			using (var dbContext = new PrinterDbContext(configuration))
			{
				if (!dbContext.Database.CanConnect())
				{
					dbContext.Database.EnsureCreated();
				}
			}
		}

	}
}