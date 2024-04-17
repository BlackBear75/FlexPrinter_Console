using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexPrint_Console.Model
{
	public class InkjetPrinter : Printer
	{
		public bool Duplex { get; set; }

		public override decimal CalculatePurchaseCost()
		{
			
			Console.WriteLine("Congratulations, you have received a cartridge as a gift!\n" +
				$"{Price} UAH due");
			return base.CalculatePurchaseCost();
		}

		public override string ToString()
		{
			return $"InkjetPrinter Printer:\n Manufacturer: {Manufacturer}\n Model: " +
				$" {Model}\n Product Code: {ProductCode}\n Price: {Price} UAH\n Purpose:{Purpose}\n " +
				$"PrinterSize: {PrinterSize}\n Duplex: {Duplex}\n";
		}
	}
}
