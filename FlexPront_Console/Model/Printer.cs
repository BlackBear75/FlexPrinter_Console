

using FlexPrint_Console.Enum;
using System.ComponentModel.DataAnnotations;

namespace FlexPrint_Console.Model
{
	public class Printer
	{
		public Guid Id { get; set; }
		public string? ProductCode { get; set; }
		public string? Model { get; set; }
		public string? Manufacturer { get; set; }
		public PrinterPurpose Purpose { get; set; }
		public MaxPrinterSize PrinterSize { get; set; }
		public decimal Price { get; set; }

		/// <summary>
		/// Метод обчисляє ціну принтера і можливі бонуси
		/// </summary>
		/// <returns></returns>
		public virtual decimal CalculatePurchaseCost()
		{
			return Price;
		}
	}
}
