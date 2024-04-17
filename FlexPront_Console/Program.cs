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

			//Створюєм сутності

			LaserPrinter l1 = new LaserPrinter()
			{
				LaserType = LaserPrinterType.Fiber,
				PrinterSize = MaxPrinterSize.A5,
				Manufacturer = "Ep",
				Model = "model1",
				Price = 6000,
				Purpose = PrinterPurpose.Home,
			};
			LaserPrinter l2 = new LaserPrinter()
			{
				LaserType = LaserPrinterType.Gas,
				PrinterSize = MaxPrinterSize.A4,
				Manufacturer = "HP",
				Model = "model2",
				Price = 4000,
				Purpose = PrinterPurpose.Office,
			};
			InkjetPrinter i1 = new InkjetPrinter()
			{
				Duplex = true,
				PrinterSize = MaxPrinterSize.A4,
				Manufacturer = "HP",
				Model = "modelI1",
				Price = 5000,
				Purpose = PrinterPurpose.Office,
			};
			InkjetPrinter i2 = new InkjetPrinter()
			{
				Duplex = true,
				PrinterSize = MaxPrinterSize.A5,
				Manufacturer = "Ep",
				Model = "modelI2",
				Price = 5000,
				Purpose = PrinterPurpose.Home,
			};

			//Додаємо до нашої бази 

			printerManager.AddPrinter(l1);
			printerManager.AddPrinter(l2);
			printerManager.AddPrinter(i1);
			printerManager.AddPrinter(i2);

			//Получаєм дані і виводимо
			var printers =	printerManager.GetPrinters();
			PrintPrinter_Console(printers);

			//Використовуєм получені принтери для різних запитів
			var inkjectpr = printerManager.GetInkjetPrinters(printers);
			PrintPrinter_Console(inkjectpr);

			var laserpr = printerManager.GetLaserPrinters(printers);
			PrintPrinter_Console(laserpr);

			//Редагуєм принтер
			InkjetPrinter edit = new InkjetPrinter()
			{
				Duplex = true,
				PrinterSize = MaxPrinterSize.A5,
				Manufacturer = "asd",
				Model = "sad",
				Price = 23,
				Purpose = PrinterPurpose.Office,
			};

			//Тут потрібно задати вірний номер принтера який є у базі тому що кожний номер унікальний і не можна відредагувати принтери з різними типами
			printerManager.EditPrinter("H756EA", edit);
			//Видаляєм принтер
			printerManager.RemovePrinter("H756EA");

			//Сортуєм принтери за ціной
			printerManager.SortPrintersByPrice(printers);

			//Пошук принтерів для дома , офісу , за виробником
			printerManager.GetHomePrinters(printers);
			printerManager.GetOfficePrinters(printers);

			var manufacturepr =	printerManager.GetPrintersByManufacturer("Ep",printers);
			PrintPrinter_Console(manufacturepr);

			//обчислення вартості принтерів
			foreach (var item in printers)
			{
               item.CalculatePurchaseCost();
            }


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