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

			var dbContext = new PrinterDbContext(configuration);
			dbContext.Database.EnsureCreated();


			IPrinterManager printerManager = new PrinterManager(configuration);

			LaserPrinter l1 = new LaserPrinter()
			{
				LaserType = LaserPrinterType.Gas,
				PrinterSize = MaxPrinterSize.A4,
				Manufacturer = "HP",
				Model = "ui53",
				Price = 16000,
				Purpose = PrinterPurpose.Home,
			};
			printerManager.EditPrinter("M0QBPZ", l1);


		}
	}
}