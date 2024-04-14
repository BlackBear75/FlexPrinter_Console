using FlexPrint_Console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexPrint_Console.Manager
{
	public interface IPrinterManager
	{

		void LoadDataFromDatabase();
		void AddPrinter<T>(T printer) where T : Printer;
		void RemovePrinter(string productCode);
		void EditPrinter<T>(string productCode, T newPrinterData) where T : Printer;
		void SortPrintersByPrice();
		List<LaserPrinter> GetLaserPrinters();
		List<InkjetPrinter> GetInkjetPrinters();
		List<Printer> GetPrintersByManufacturer(string manufacturer);
		List<Printer> GetHomePrinters();
		List<Printer> GetOfficePrinters();
	
	}

}
