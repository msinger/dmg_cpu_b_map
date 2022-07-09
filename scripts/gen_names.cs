// Generates a bunch of random names that can be used as cell labels
// in the SVG file. It optionally scans an existing file to prevent
// already existing names from being printed.

using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

static class Program
{
	static int Main(string[] args)
	{
		HashSet<string> l = new HashSet<string>();

		for (int i = 0; i < args.Length; i++)
		{
			if (args[i].Length == 4 && args[i].All(char.IsLetter))
			{
				l.Add(args[i].ToUpper());
				continue;
			}

			Console.Error.WriteLine("Scanning document " + args[i] + "...");
			XmlDocument doc = new XmlDocument();
			doc.Load(args[i]);

			XmlNodeList textElements = doc.GetElementsByTagName("text");

			int c = 0;
			foreach (XmlNode text in textElements)
			{
				string t = text.InnerText.Trim().ToUpper();
				if (t.Length != 4)
					continue;
				if (!t.All(char.IsLetter))
					continue;
				l.Add(t);
				c++;
			}

			Console.Error.WriteLine("Added " + c + " names from document.");
		}

		Console.Error.WriteLine("Now have " + l.Count + " names total (not counting duplicates).");

		// "Y" is used instead of "I", becuase "I" can easily be confused with "l" or "1".
		char[] vovels = new char[] { 'A', 'E', 'O', 'U', 'Y' };

		// "Y" is not in here, because it is used as a vovel.
		// "Q" is not allowed.
		char[] consonants = new char[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M',
		                                 'N', 'P', 'R', 'S', 'T', 'V', 'W', 'X', 'Z' };

		SortedSet<char> letters = new SortedSet<char>(vovels);
		letters.UnionWith(consonants);

		foreach (char c1 in letters)
		{
			int tries = 4000;
			for (int i = 0; i < 10; i++)
			{
				char c2, c3, c4;

			retry:
				if (--tries <= 0)
				{
					Console.Write("  <giving up>");
					break;
				}

				if (vovels.Contains(c1))
				{
					c2 = RandomChar(consonants);
					c3 = RandomChar(vovels);
					c4 = RandomChar(consonants);
				}
				else
				{
					c2 = RandomChar(vovels);
					c3 = RandomChar(consonants);
					c4 = RandomChar(vovels);
				}

				string n = "" + c1 + c2 + c3 + c4;

				if (l.Contains(n))
					goto retry;

				Console.Write("  " + n);
			}

			Console.WriteLine();
		}

		return 0;
	}

	static Random rnd = new Random();
	static char RandomChar(IList<char> chars)
	{
		return chars[rnd.Next(chars.Count)];
	}
}
