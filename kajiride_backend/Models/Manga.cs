using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend.Models
{
	public class Manga
	{
		public long? mangaid;
		public string name;
		public string author;
		public string artist;
		public string publisher;
		public string status;
		public int? volumes;
		public string language;
		public string genre;
		public string image;
		public string description;

		public static int[] VolumesStringToArray(string volumes)
		{
			int[] result = volumes.Split(',').SelectMany(x =>
			{
				if (x.Contains("-"))
				{
					int[] numbers = x.Split('-').Select(y => int.Parse(y)).ToArray();
					return Enumerable.Range(numbers[0], (numbers[1] - numbers[0]) + 1);
				}
				return new int[] { int.Parse(x) };
			}).ToArray();

			return result;
		}
	}
}