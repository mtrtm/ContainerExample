using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FruitCalc
{
	class Program
	{
		static void Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			//could do the same thing with args but in docker-compose and debugging args can be tricky, so just override from our config
			args = configuration.GetSection("Cart1").Value.Split(" ");

			//eval incoming params. Could do error handling but that's probably overkill for a quick example
			var cartEntries = new List<CartEntry>();
			for (int i = 0; i < args.Length; i += 4)
			{
				var currentEntry = new CartEntry
				{
					Name = args[i],
					Price = Convert.ToDecimal(args[i + 1]),
					Count = Convert.ToInt32(args[i + 2]),
					Discount = Convert.ToDecimal(args[i + 3])
				};

				cartEntries.Add(currentEntry);
				Console.WriteLine($"adding {currentEntry.Count} {currentEntry.Name} @ ${currentEntry.Price} with a {currentEntry.Discount * 100}% discount");
			}
			Console.WriteLine(string.Empty);

			//calc total of cart
			decimal total = Decimal.Zero;
			foreach (CartEntry entry in cartEntries)
			{
				var discountedPrice = entry.Price - (entry.Price * entry.Discount);
				total += (discountedPrice * entry.Count);
			}

			Console.WriteLine($"Total in cart: {total:C}");
		}
	}
}
