// Converts the coordinates of all the cell labels from the SVG file
// to the coordinates on the Leaflet map.

using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

static class Program
{
	static int Main(string[] args)
	{
		XmlDocument doc = new XmlDocument();
		doc.Load(args[0]);

		Dictionary<string, Tuple<double, double>> map = new Dictionary<string, Tuple<double, double>>();
		Dictionary<string, Tuple<double, double>> map2 = new Dictionary<string, Tuple<double, double>>();

		XmlNodeList textElements = doc.GetElementsByTagName("text");

		// The texts that are rotated need their axes swapped in conversion below.
		// Don't know how to identify those in the SVG file, that's why they are hardcoded here:
		string[] rotated = {
			"abol", "atez", "apov", "arev", "afas", "abuz", "awod", "agut",
			"ajax", "alur", "avor", "afer", "asol", "afar", "alyp", "adar",
			"adyk", "arov", "apuk", "afep", "alef", "atyp", "afur", "atal",
			"anos", "avet", "arys", "azof",
			"boma", "boga", "byju", "baly", "bele", "buto", "baze", "belo",
			"bane", "beja", "buke", "basu", "bugo", "bate", "bufa", "byly",
			"bolo", "byda", "beru", "bapy", "bude", "beva", "beko", "bavy",
			"byry", "belu", "bedo", "bowa", "buvu", "byxo", "buty",
			"caro", "caly", "cyde", "cylo", "cafa", "coty", "cave", "culy",
			"core", "cuba", "cugy", "cohy", "cufu", "cage", "coba", "caby",
			"daku", "dovu", "dame", "dyly", "dola", "deho", "dawe", "dojo",
			"daso", "daju", "dyge", "dyra", "detu", "dybo", "dela", "degu",
			"dude", "dumo", "docu", "dawa",
			"ejab", "evok", "ehuj", "elok", "erod", "efab", "efak", "edel",
			"eder", "etak", "eguv", "efef", "epyt", "elys", "edyl", "etaf",
			"eluv",
			"yaza", "yula",
			"zapa", "zapy", "zyro", "zubu", "zefu", "zaje", "zete", "zole",
			"zuko", "zovy", "zyga", "zyky", "zyba", "zuvy", "zufy", "zera",
			"zoke", "zabu", "zage", "zyra", "zery", "zado", "zufa", "zadu",
			"zoro",
			"wadu", "wola", "woku", "wafa", "wyxy", "wexe", "wazu", "wuta",
			"woly", "wale"
		};

		foreach (XmlNode text in textElements)
		{
			string t = text.InnerText.Trim().ToLower();
			if (t.Length != 4)
				continue;
			if (!t.All(char.IsLetter))
				continue;
			XmlNode x = text.Attributes.GetNamedItem("x");
			XmlNode y = text.Attributes.GetNamedItem("y");
			if (x == null || y == null)
				continue;
			if (rotated.Contains(t))
				map[t] = new Tuple<double, double>(double.Parse(y.InnerText), -double.Parse(x.InnerText));
			else
				map[t] = new Tuple<double, double>(double.Parse(x.InnerText), double.Parse(y.InnerText));
		}

		foreach (KeyValuePair<string, Tuple<double, double>> kvp in map)
			map2[kvp.Key] = new Tuple<double, double>(
				(kvp.Value.Item1 + 1873.976) * 256.0 / 3703.32,
				-(3684.57 - (1710.541 - kvp.Value.Item2)) * 254.63 / 3684.57
			);

		foreach (KeyValuePair<string, Tuple<double, double>> kvp in map2)
			Console.WriteLine("\t\t" + kvp.Key + ": [" + kvp.Value.Item2.ToString("f1") + ", " +
			                                             kvp.Value.Item1.ToString("f1") + "],");

		return 0;
	}
}
