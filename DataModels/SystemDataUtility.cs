using System;

namespace FarTrader.DataModels
{
	internal static class SystemDataUtility
	{
		public static double GetCapitolScore(long population, bool isInterdicted)
		{
			double score = 0;

			score += Math.Log10(population);

			if (isInterdicted)
				score -= 100;

			return score;
		}
	}
}
