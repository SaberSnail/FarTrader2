using System;

namespace FarTrader.DataModels
{
	internal static class SystemDataUtility
	{
		public static double GetCapitolScore(PhysicalSystemData physicalData, SocialSystemData socialData)
		{
			double score = 0;

			score += Math.Log10(socialData.Population);

			if (socialData.IsInterdicted)
				score -= 100;

			return score;
		}
	}
}
