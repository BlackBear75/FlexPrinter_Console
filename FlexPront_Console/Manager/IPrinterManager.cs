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
		/// <summary>
		/// Метод, який завантажує дані про принтери з бази даних.
		/// </summary>
		void LoadDataFromDatabase();

		/// <summary>
		/// Метод для додавання нового принтера до бази даних та внутрішнього списку.
		/// </summary>
		/// <typeparam name="T">Тип принтера, який додається.</typeparam>
		/// <param name="printer">Об'єкт принтера, який додається.</param>
		void AddPrinter<T>(T printer) where T : Printer;

		/// <summary>
		/// Метод для получення всіч принтерів
		/// </summary>
		List<Printer> GetPrinters();
		/// <summary>
		/// Метод для видалення принтера з бази даних та внутрішнього списку за його унікальним кодом.
		/// </summary>
		/// <param name="productCode">Унікальний код продукту, який ідентифікує принтер.</param>
		void RemovePrinter(string productCode);

		/// <summary>
		/// Метод для редагування даних про принтер в базі даних та внутрішньому списку.
		/// </summary>
		/// <typeparam name="T">Тип принтера, який редагується.</typeparam>
		/// <param name="productCode">Унікальний код продукту, який ідентифікує принтер.</param>
		/// <param name="newPrinterData">Нові дані про принтер для заміни.</param>
		void EditPrinter<T>(string productCode, T newPrinterData) where T : Printer;

		/// <summary>
		/// Метод для сортування принтерів за ціною в порядку зростання.
		/// </summary>
		List<Printer> SortPrintersByPrice(List<Printer> allPrinters);

		/// <summary>
		/// Метод для отримання списку лазерних принтерів з бази даних.
		/// </summary>
		/// <returns>Список лазерних принтерів.</returns>
		List<Printer> GetLaserPrinters(List<Printer> allPrinters);

		/// <summary>
		/// Метод для отримання списку струменевих принтерів з бази даних.
		/// </summary>
		/// <returns>Список струменевих принтерів.</returns>
		List<Printer> GetInkjetPrinters(List<Printer> allPrinters);

		/// <summary>
		/// Метод для отримання списку принтерів виробника за його назвою.
		/// </summary>
		/// <param name="manufacturer">Назва виробника принтерів.</param>
		/// <returns>Список принтерів виробника.</returns>
		List<Printer> GetPrintersByManufacturer(string manufacturer, List<Printer> allPrinters);

		/// <summary>
		/// Метод для отримання списку принтерів для дому.
		/// </summary>
		/// <returns>Список принтерів для дому.</returns>
		List<Printer> GetHomePrinters(List<Printer> allPrinters);

		/// <summary>
		/// Метод для отримання списку принтерів для офісу.
		/// </summary>
		/// <returns>Список принтерів для офісу.</returns>
		List<Printer> GetOfficePrinters(List<Printer> allPrinters);
	}


}
