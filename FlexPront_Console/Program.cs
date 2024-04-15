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




		}

	
	}
}