using System;
using System.Collections.Generic;
using GoldenAnvil.Utility;

namespace FarTrader.Tools
{
	internal class ImperialNameGenerator : INameGenerator
	{
		public static ImperialNameGenerator Default = new ImperialNameGenerator();

		public string GetName(Random rng)
		{
			string name = "";
			List<int> syllablesPerWord = new List<int>();

			int firstWordSyllables = rng.NextRoll(1, 6) + 1;
			if (firstWordSyllables >= 4)
			{
				int secondWordSyllables = firstWordSyllables - rng.NextRoll(1, 6) - 1;
				if (secondWordSyllables > 0)
				{
					if (secondWordSyllables < 2)
						secondWordSyllables = 2;
					if (secondWordSyllables > firstWordSyllables - 2)
						secondWordSyllables = firstWordSyllables - 2;
					syllablesPerWord.Add(secondWordSyllables);
					firstWordSyllables -= secondWordSyllables;
				}
				else if (firstWordSyllables > 5)
					firstWordSyllables = 5;
			}
			syllablesPerWord.Insert(0, firstWordSyllables);

			foreach (int syllables in syllablesPerWord)
			{
				string word = "";
				bool useBasic = true;

				for (int i = 0; i < syllables; i++)
					word += GetSyllable(rng, ref useBasic);

				if (name.Length != 0)
					name += " ";
				name += char.ToUpper(word[0]) + word.Substring(1, word.Length - 1);
			}

			return name;
		}

		private static string GetInitialConsonant(Random rng)
		{
			int value = rng.NextRoll(1, 216) - 1;
			if (value < 39)
				return "k";
			if (value < 78)
				return "g";
			if (value < 99)
				return "m";
			if (value < 120)
				return "d";
			if (value < 141)
				return "l";
			if (value < 162)
				return "sh";
			if (value < 180)
				return "kh";
			if (value < 190)
				return "n";
			if (value < 200)
				return "s";
			if (value < 204)
				return "p";
			if (value < 208)
				return "b";
			if (value < 212)
				return "z";
			return "r";
		}

		private static string GetFinalConsonant(Random rng)
		{
			int value = rng.NextRoll(1, 216) - 1;
			if (value < 76)
				return "r";
			if (value < 102)
				return "n";
			if (value < 138)
				return "m";
			if (value < 165)
				return "sh";
			if (value < 180)
				return "g";
			if (value < 191)
				return "s";
			if (value < 204)
				return "d";
			if (value < 210)
				return "p";
			return "k";
		}

		private static string GetVowel(Random rng)
		{
			int value = rng.NextRoll(1, 216) - 1;
			if (value < 67)
				return "a";
			if (value < 84)
				return "e";
			if (value < 143)
				return "i";
			if (value < 184)
				return "u";
			if (value < 192)
				return "aa";
			if (value < 208)
				return "ii";
			return "uu";
		}

		private static string GetSyllable(Random rng, ref bool useBasic)
		{
			string syllable = "";
			int value = rng.NextRoll(1, 216) - 1;
			if (useBasic)
			{
				if (value < 6)
				{
					syllable += GetVowel(rng);
					useBasic = true;
				}
				else if (value < 21)
				{
					syllable += GetInitialConsonant(rng);
					syllable += GetVowel(rng);
					useBasic = true;
				}
				else if (value < 29)
				{
					syllable += GetVowel(rng);
					syllable += GetFinalConsonant(rng);
					useBasic = false;
				}
				else
				{
					syllable += GetInitialConsonant(rng);
					syllable += GetVowel(rng);
					syllable += GetFinalConsonant(rng);
					useBasic = false;
				}
			}
			else
			{
				if (value < 21)
				{
					syllable += GetInitialConsonant(rng);
					syllable += GetVowel(rng);
					useBasic = true;
				}
				else
				{
					syllable += GetInitialConsonant(rng);
					syllable += GetVowel(rng);
					syllable += GetFinalConsonant(rng);
					useBasic = false;
				}
			}
			return syllable;
		}
	}
}
