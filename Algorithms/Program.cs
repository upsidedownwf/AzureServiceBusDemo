using System;
using System.Text;
using System.Collections.Generic;

namespace Algorithms
{
	public class Program
	{
		public static void Main()
		{
			var signals = "?-?";
			var results = signals.Contains("?")? CheckPossibilities(signals):new string[1] { Morses[signals] };
			var x = new StringBuilder();
			foreach (var result in results)
			{
				x.Append(result);
			}
			Console.WriteLine(x.ToString());
		}
		private static Dictionary<string, string> Morses = new Dictionary<string, string>{
		{".", "E"},
		{"-", "T"},
		{"..", "I"},
		{".-", "A"},
		{"-.", "N"},
		{"--", "M"},
		{"...", "S"},
		{"..-", "U"},
		{".-.", "R"},
		{".--", "W"},
		{"-..", "D"},
		{"-.-", "K"},
		{"--.", "G"},
		{"---", "O"}
	};
		static string[] CheckPossibilities(string signals)
		{
			int length = signals.Length;
			var values = new string[] { };
			if (length == 1)
			{
				values = new string[2] { "E", "T" };
			}
			else if (length > 1)
			{
				List<int> indexes = new List<int>();
				var valuesList = new List<string>();
				char[] signalCharacters = signals.ToCharArray();
				for (var x = 0; x < signalCharacters.Length; x++)
				{
					if (signalCharacters[x].ToString() == "?") continue;
					else indexes.Add(x);
				}

				var morsekeys = Morses.Keys;
				foreach (var key in morsekeys)
				{
					if (key.Length != length) continue;
					bool valid = true;
					foreach (var index in indexes)
					{
						var keyCharacters = key.ToCharArray();
						if (signalCharacters[index] == keyCharacters[index]) continue;
						valid = false;
					}
					if (valid) valuesList.Add(Morses[key]);
				}
				values = valuesList.ToArray();
			}
			return values;
		}
	}
}
