using FlexPrint_Console.DB;
using FlexPrint_Console.Enum;
using FlexPrint_Console.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlexPrint_Console.Manager
{
	public class PrinterManager : IPrinterManager

	{
		private List<LaserPrinter> laserPrintersList = new List<LaserPrinter>();
		private List<InkjetPrinter> inkjetPrintersList = new List<InkjetPrinter>();

		private IConfiguration _configuration;

		public PrinterManager(IConfiguration configuration)
		{
			_configuration = configuration;
			LoadDataFromDatabase();
		}
		public void LoadDataFromDatabase()
		{
			using (var context = new PrinterDbContext(_configuration))
			{
				var laserPrinters = context.LaserPrinters.ToList();
				var inkjetPrinters = context.InkjetPrinters.ToList();

				if (laserPrinters.Any())
				{
					foreach (var printer in laserPrinters)
					{
						laserPrintersList.Add(printer);
					}
				}
				else
				{
					Console.WriteLine("Dont have LaserPrinter in base");
				}

				if (inkjetPrinters.Any())
				{
					foreach (var printer in inkjetPrinters)
					{
						inkjetPrintersList.Add(printer);
					}
				}
				else
				{
					Console.WriteLine("Dont have InkjetPrinter in base");
				}
			}
		}


		public void AddPrinter<T>(T printer) where T : Printer
		{
			try
			{

				if (printer is LaserPrinter)
				{
					printer.ProductCode = GenerateProductCode();
					laserPrintersList.Add((LaserPrinter)(object)printer);
					using (var context = new PrinterDbContext(_configuration))
					{
						context.LaserPrinters.Add((LaserPrinter)(object)printer);
						context.SaveChanges();
					}
				}
				else if (printer is InkjetPrinter)
				{
					printer.ProductCode = GenerateProductCode();
					inkjetPrintersList.Add((InkjetPrinter)(object)printer);
					using (var context = new PrinterDbContext(_configuration))
					{
						context.InkjetPrinters.Add((InkjetPrinter)(object)printer);
						context.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error with Add Printer: {ex.Message}");
				
			}
		}

		private string GenerateProductCode()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			Random random = new Random();
			string productCode;

			do
			{
				productCode = new string(Enumerable.Repeat(chars, 6)
				  .Select(s => s[random.Next(s.Length)]).ToArray());
			} while (laserPrintersList.Any(p => p.ProductCode == productCode) || inkjetPrintersList.Any(p => p.ProductCode == productCode));

			return productCode;
		}

		public void EditPrinter<T>(string productCode, T newPrinterData) where T : Printer
		{
			try
			{
				using (var context = new PrinterDbContext(_configuration))
				{
					Printer printerToUpdate = null;

					if (newPrinterData is LaserPrinter)
					{
						var index = laserPrintersList.FindIndex(p => p.ProductCode == productCode);
						if (index != -1)
						{
							laserPrintersList[index] = (LaserPrinter)(object)newPrinterData;
							printerToUpdate = laserPrintersList[index];
						}
					}
					else if (newPrinterData is InkjetPrinter)
					{
						var index = inkjetPrintersList.FindIndex(p => p.ProductCode == productCode);
						if (index != -1)
						{
							inkjetPrintersList[index] = (InkjetPrinter)(object)newPrinterData;
							printerToUpdate = inkjetPrintersList[index];
						}
					}

					if (printerToUpdate != null)
					{
						var existingPrinter = context.Printers.FirstOrDefault(p => p.ProductCode == productCode);
						if (existingPrinter != null)
						{
							existingPrinter.Model = printerToUpdate.Model;
							existingPrinter.Manufacturer = printerToUpdate.Manufacturer;
							existingPrinter.Price = printerToUpdate.Price;
							existingPrinter.Purpose = printerToUpdate.Purpose;
							existingPrinter.PrinterSize = printerToUpdate.PrinterSize;

							// Check the type of printer and update specific properties accordingly
							if (printerToUpdate is LaserPrinter)
							{
								((LaserPrinter)printerToUpdate).LaserType = ((LaserPrinter)(object)newPrinterData).LaserType;
							}
							else if (printerToUpdate is InkjetPrinter)
							{
								((InkjetPrinter)printerToUpdate).Duplex = ((InkjetPrinter)(object)newPrinterData).Duplex;
							}

							context.SaveChanges();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error with Edit Printer: {ex.Message}");
			}
		}

		public void RemovePrinter(string productCode)
		{
			
			var laserPrinterToRemove = laserPrintersList.FirstOrDefault(p => p.ProductCode == productCode);
			if (laserPrinterToRemove != null)
			{
				
					laserPrintersList.Remove(laserPrinterToRemove);
				using (var context = new PrinterDbContext(_configuration))
				{
					context.LaserPrinters.Remove(laserPrinterToRemove);
					context.SaveChanges();
				}
			}
			var inkjetPrinterToRemove = inkjetPrintersList.FirstOrDefault(p => p.ProductCode == productCode);
			if (inkjetPrinterToRemove != null)
			{
				inkjetPrintersList.Remove(inkjetPrinterToRemove);
				using (var context = new PrinterDbContext(_configuration))
				{
					context.InkjetPrinters.Remove(inkjetPrinterToRemove);
					context.SaveChanges();
				}
			}
			else
			{
				Console.WriteLine("Remove : A printer with the same code was not found");
			}

		}

		public List<Printer> SortPrintersByPrice(List<Printer> allPrinters)
		{
			if(allPrinters.Count==0)
			{
                Console.WriteLine("You can't sort emptiness");
                return allPrinters;
			}
			var sortedPrinters = new List<Printer>(allPrinters.OrderBy(p => p.Price));

			Console.WriteLine("Printers sorted by price:");
			foreach (var printer in sortedPrinters)
			{
				Console.WriteLine($"Model: {printer.Model}, Price: {printer.Price}");
			}
			return sortedPrinters;
		}

		public List<Printer> GetLaserPrinters(List<Printer> allPrinters)
		{
			var laserPrinters = new List<Printer>();

			foreach (var printer in allPrinters)
			{
				if (printer is LaserPrinter)
				{
					laserPrinters.Add(printer);
				}
			}
			if(laserPrinters.Count==0)
			{
                Console.WriteLine("Laser printers not found");
            }
			return laserPrinters;
		}

		public List<Printer> GetInkjetPrinters(List<Printer> allPrinters)
		{
			var inkjetPrinters = new List<Printer>();

			foreach (var printer in allPrinters)
			{
				if (printer is InkjetPrinter)
				{
					inkjetPrinters.Add(printer);
				}
			}
			if (inkjetPrinters.Count == 0)
			{
				Console.WriteLine("Inkjet printers not found");
			}
			return inkjetPrinters;
		}

		public List<Printer> GetPrintersByManufacturer(string manufacturer, List<Printer> allPrinters)
		{
			var printersByManufacturer = new List<Printer>();
          

            foreach (var printer in allPrinters)
			{
				if (printer.Manufacturer == manufacturer)
				{
					printersByManufacturer.Add(printer);
				}
			}
			if (printersByManufacturer.Count==0)
			{
				Console.WriteLine("No printers with this manufacturer were found") ;
			}
			else
			{
				Console.WriteLine($"List of printers with manufacturer {manufacturer}:") ;
            }
			return printersByManufacturer;
		}

		public List<Printer> GetHomePrinters(List<Printer> allPrinters)
		{
			var homePrinters = new List<Printer>();

			foreach (var printer in allPrinters)
			{
				if (printer.Purpose == PrinterPurpose.Home)
				{
					homePrinters.Add(printer);
				}
			}
			if (homePrinters.Count == 0)
			{
				Console.WriteLine("No home printers found");
			}
			return homePrinters;
		}

		public List<Printer> GetOfficePrinters(List<Printer> allPrinters)
		{
			var officePrinters = new List<Printer>();

			foreach (var printer in allPrinters)
			{
				if (printer.Purpose == PrinterPurpose.Office)
				{
					officePrinters.Add(printer);
				}
			}
			if(officePrinters.Count==0)
			{
				Console.WriteLine("No office printers found");
			}
	
			return officePrinters;
		}

		public List<Printer> GetPrinters()
		{
			var printers = new List<Printer>();
			foreach (var printer in inkjetPrintersList)
			{

				printers.Add(printer);

			}
			foreach (var printer in laserPrintersList)
			{
				printers.Add(printer);
			}
			if(printers.Count==0)
			{
                Console.WriteLine("Printers not found ");
            }

			return printers;
		}
	}

		
}
