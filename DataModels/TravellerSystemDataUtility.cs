using System;

namespace FarTrader.DataModels
{
	internal static class TravellerSystemDataUtility
	{
		public static double CalculateGravity(double density, int diameter)
		{
			return density * (double) diameter / 7930.0;
		}

		public static int GetResourceModifier(TravellerResourceDensity resourceDensity)
		{
			switch (resourceDensity)
			{
			case TravellerResourceDensity.Worthless:
				return -5;
			case TravellerResourceDensity.VeryScant:
				return -4;
			case TravellerResourceDensity.Scant:
				return -3;
			case TravellerResourceDensity.VeryPoor:
				return -2;
			case TravellerResourceDensity.Poor:
				return -1;
			case TravellerResourceDensity.Average:
				return 0;
			case TravellerResourceDensity.Rich:
				return 1;
			case TravellerResourceDensity.VeryRich:
				return 2;
			case TravellerResourceDensity.Abundant:
				return 3;
			case TravellerResourceDensity.VeryAbundant:
				return 4;
			case TravellerResourceDensity.Motherlode:
				return 5;
			default:
				throw new ArgumentOutOfRangeException("resourceDensity", String.Format("Unsupported resource density {0}", resourceDensity));
			}
		}

		public static int CalculateHabitability(TravellerWorldKind worldKind, TravellerAtmosphereKind atmosphereKind, TravellerAtmosphereDensity atmosphereDensity, double hydrographicCoverage, TravellerClimateKind climateKind)
		{
			int habitability = 0;

			if ((worldKind == TravellerWorldKind.Garden) && (atmosphereDensity > TravellerAtmosphereDensity.VeryThin))
			{
				habitability += 3;
				if ((atmosphereDensity == TravellerAtmosphereDensity.Standard) || (atmosphereDensity == TravellerAtmosphereDensity.Dense))
					habitability += 1;
				if (atmosphereKind <= TravellerAtmosphereKind.Normal)
					habitability += 1;
				if ((hydrographicCoverage > 0.0) && (hydrographicCoverage <= 0.3))
					habitability += 1;
				if (hydrographicCoverage > 0.9)
					habitability += 1;
				if ((hydrographicCoverage > 0.3) && (hydrographicCoverage <= 0.9))
					habitability += 2;
				if ((climateKind == TravellerClimateKind.Hot) || (climateKind == TravellerClimateKind.Cold))
					habitability += 1;
				if ((climateKind >= TravellerClimateKind.Chilly) && (climateKind <= TravellerClimateKind.Tropical))
					habitability += 2;
			}

			return habitability;
		}

		public static int CalculateAffinity(int habitability, TravellerResourceDensity resourceDensity)
		{
			return habitability + GetResourceModifier(resourceDensity);
		}

		public static double CalculateMaxPopulation(TravellerAtmosphereKind atmosphereKind, TravellerAtmosphereDensity atmosphereDensity, TravellerClimateKind climateKind, int diameter, double hydrographicCoverage)
		{
			double maxPopulation;

			if ((atmosphereDensity <= TravellerAtmosphereDensity.VeryThin) ||
				(atmosphereDensity >= TravellerAtmosphereDensity.SuperDense) ||
				((atmosphereKind != TravellerAtmosphereKind.Normal) && (atmosphereKind != TravellerAtmosphereKind.Pollutants)))
			{
				maxPopulation = 0;
			}
			else
			{
				maxPopulation = 9;
				if (diameter < 2000)
					maxPopulation -= 2;
				else if (diameter <= 4000)
					maxPopulation -= 1;
				if (hydrographicCoverage <= 0.30 || hydrographicCoverage > 0.9)
					maxPopulation -= 1;
				if (hydrographicCoverage == 0)
					maxPopulation -= 2;
				if (atmosphereDensity == TravellerAtmosphereDensity.Thin || atmosphereDensity == TravellerAtmosphereDensity.Dense)
					maxPopulation -= 1;
				if (atmosphereKind == TravellerAtmosphereKind.Pollutants)
					maxPopulation -= 1;
				if (climateKind == TravellerClimateKind.Frozen || climateKind == TravellerClimateKind.Infernal)
					maxPopulation -= 2;
				else if (climateKind <= TravellerClimateKind.Cold || climateKind >= TravellerClimateKind.Hot)
					maxPopulation -= 1;
			}

			return maxPopulation;
		}

		public static TravellerTradeClassKind CalculateTradeClasses(TravellerAtmosphereKind atmosphereKind, TravellerAtmosphereDensity atmosphereDensity, double hydrographicCoverage, TravellerClimateKind climateKind, double populationRating, int controlRating, TravellerWorldKind worldKind, TravellerGovernmentKind governmentKind)
		{
			TravellerTradeClassKind tradeClasses = TravellerTradeClassKind.None;

			// GURPS 4th Edition
			if (atmosphereDensity >= TravellerAtmosphereDensity.Thin && atmosphereDensity <= TravellerAtmosphereDensity.VeryDense &&
				hydrographicCoverage >= 0.35 && hydrographicCoverage < 0.85 &&
				populationRating >= 5 && populationRating < 8)
				tradeClasses |= TravellerTradeClassKind.Agricultural;

			if (atmosphereDensity <= TravellerAtmosphereDensity.Trace || atmosphereDensity >= TravellerAtmosphereDensity.VeryDense ||
				atmosphereKind == TravellerAtmosphereKind.None || atmosphereKind == TravellerAtmosphereKind.Suffocating ||
				atmosphereKind == TravellerAtmosphereKind.Toxic || atmosphereKind == TravellerAtmosphereKind.Corrosive ||
				atmosphereKind == TravellerAtmosphereKind.Insidious ||
				climateKind <= TravellerClimateKind.Frozen || climateKind >= TravellerClimateKind.Infernal ||
				hydrographicCoverage < 0.05)
				tradeClasses |= TravellerTradeClassKind.Extreme;

			if ((atmosphereDensity <= TravellerAtmosphereDensity.Trace || (atmosphereDensity >= TravellerAtmosphereDensity.VeryThin && atmosphereDensity <= TravellerAtmosphereDensity.VeryDense && atmosphereKind > TravellerAtmosphereKind.Normal)) &&
				(populationRating >= 9))
				tradeClasses |= TravellerTradeClassKind.Industrial;

			if ((atmosphereDensity < TravellerAtmosphereDensity.Trace) || ((atmosphereDensity >= TravellerAtmosphereDensity.Trace) && (atmosphereDensity <= TravellerAtmosphereDensity.VeryThin) && (hydrographicCoverage < 0.35) && (populationRating >= 6)))
				tradeClasses |= TravellerTradeClassKind.NonAgricultural;

			if (populationRating < 7)
				tradeClasses |= TravellerTradeClassKind.NonIndustrial;

			if ((atmosphereDensity >= TravellerAtmosphereDensity.VeryThin) && (atmosphereDensity <= TravellerAtmosphereDensity.Thin) && (hydrographicCoverage >= 0.05) && (hydrographicCoverage <= 0.34))
				tradeClasses |= TravellerTradeClassKind.Poor;

			if ((atmosphereDensity >= TravellerAtmosphereDensity.Standard) && (atmosphereDensity <= TravellerAtmosphereDensity.VeryDense) && (atmosphereKind <= TravellerAtmosphereKind.Normal) && (populationRating >= 6) && (populationRating < 9) && (controlRating >= 2) && (controlRating <= 5))
				tradeClasses |= TravellerTradeClassKind.Rich;
			
			// GURPS 3rd Edition
			if (worldKind == TravellerWorldKind.AsteroidBelt)
				tradeClasses |= TravellerTradeClassKind.AsteroidClass;

			if ((populationRating == 0) || (governmentKind == TravellerGovernmentKind.Anarchy) || (controlRating == 0))
				tradeClasses |= TravellerTradeClassKind.BarrenWorld;

			if ((atmosphereDensity >= TravellerAtmosphereDensity.VeryThin) && (hydrographicCoverage <= 0.04))
				tradeClasses |= TravellerTradeClassKind.DesertWorld;

			if ((worldKind != TravellerWorldKind.Garden) && (worldKind != TravellerWorldKind.PreGarden) && (worldKind != TravellerWorldKind.Glacier) && ((atmosphereKind == TravellerAtmosphereKind.Corrosive) || ((atmosphereKind > TravellerAtmosphereKind.Normal) && (atmosphereKind != TravellerAtmosphereKind.Pollutants) && (atmosphereKind != TravellerAtmosphereKind.InertGases))) && (hydrographicCoverage > 0.05))
				tradeClasses |= TravellerTradeClassKind.ExoticOcean;

			if (populationRating >= 9)
				tradeClasses |= TravellerTradeClassKind.HighPopulation;

			if ((atmosphereDensity <= TravellerAtmosphereDensity.Trace) && (hydrographicCoverage > 0.05))
				tradeClasses |= TravellerTradeClassKind.IceCapped;

			if (populationRating < 4)
				tradeClasses |= TravellerTradeClassKind.LowPopulation;

			if ((atmosphereDensity < TravellerAtmosphereDensity.Trace) || (atmosphereKind == TravellerAtmosphereKind.None))
				tradeClasses |= TravellerTradeClassKind.VacuumWorld;

			if (hydrographicCoverage >= 0.95)
				tradeClasses |= TravellerTradeClassKind.WaterWorld;
			
			// wrap up
			if (tradeClasses.HasFlag(TravellerTradeClassKind.AsteroidClass | TravellerTradeClassKind.DesertWorld | TravellerTradeClassKind.ExoticOcean | TravellerTradeClassKind.IceCapped | TravellerTradeClassKind.VacuumWorld))
				tradeClasses |= TravellerTradeClassKind.Extreme;

			return tradeClasses;
		}

		public static double CalculateWorldTradeNumber(double populationRating, int techLevel, TravellerStarportClass starportClass)
		{
			double worldTradeNumber = populationRating / 2.0;

			if (techLevel <= 2)
				worldTradeNumber -= 0.5;
			else if (techLevel <= 5)
				worldTradeNumber += 0.0;
			else if (techLevel <= 8)
				worldTradeNumber += 0.5;
			else
				worldTradeNumber += 1.0;

			switch (starportClass)
			{
			case TravellerStarportClass.A:
				if (worldTradeNumber >= 5.0)
					worldTradeNumber += 0.0;
				else if (worldTradeNumber >= 3.0)
					worldTradeNumber += 0.5;
				else if (worldTradeNumber >= 1.0)
					worldTradeNumber += 1.0;
				else
					worldTradeNumber += 1.5;
				break;

			case TravellerStarportClass.B:
				if (worldTradeNumber >= 4.0)
					worldTradeNumber += 0.0;
				else if (worldTradeNumber >= 2.0)
					worldTradeNumber += 0.5;
				else
					worldTradeNumber += 1.0;
				break;

			case TravellerStarportClass.C:
				if (worldTradeNumber >= 5.0)
					worldTradeNumber -= 0.5;
				else if (worldTradeNumber >= 3.0)
					worldTradeNumber += 0.0;
				else if (worldTradeNumber >= 1.0)
					worldTradeNumber += 0.5;
				else
					worldTradeNumber += 1.0;
				break;

			case TravellerStarportClass.D:
				if (worldTradeNumber >= 5.0)
					worldTradeNumber -= 1.0;
				else if (worldTradeNumber >= 4.0)
					worldTradeNumber -= 0.5;
				else if (worldTradeNumber >= 2.0)
					worldTradeNumber += 0.0;
				else
					worldTradeNumber += 0.5;
				break;

			case TravellerStarportClass.E:
				if (worldTradeNumber >= 5.0)
					worldTradeNumber -= 1.5;
				else if (worldTradeNumber >= 4.0)
					worldTradeNumber -= 1.0;
				else if (worldTradeNumber >= 3.0)
					worldTradeNumber -= 0.5;
				else if (worldTradeNumber >= 1.0)
					worldTradeNumber += 0.0;
				else
					worldTradeNumber += 0.5;
				break;

			case TravellerStarportClass.X:
				if (worldTradeNumber >= 5.0)
					worldTradeNumber -= 4.0;
				else if (worldTradeNumber >= 4.0)
					worldTradeNumber -= 3.5;
				else if (worldTradeNumber >= 3.0)
					worldTradeNumber -= 3.0;
				else if (worldTradeNumber >= 2.0)
					worldTradeNumber -= 2.5;
				else if (worldTradeNumber >= 1.0)
					worldTradeNumber -= 2.0;
				else
					worldTradeNumber -= 1.5;
				break;
			}

			if (worldTradeNumber < 0.0)
				worldTradeNumber = 0.0;

			return worldTradeNumber;
		}

		public static int CalculatePerCapitaIncome(double populationRating, int techLevel, TravellerTradeClassKind tradeClasses)
		{
			if (populationRating == 0)
				return 0;

			int perCapitaIncome;
			switch (techLevel) {
			case 11:
				perCapitaIncome = 96000;
				break;
			case 10:
				perCapitaIncome = 60000;
				break;
			case 9:
				perCapitaIncome = 38000;
				break;
			case 8:
				perCapitaIncome = 24000;
				break;
			case 7:
				perCapitaIncome = 15000;
				break;
			case 6:
				perCapitaIncome = 9200;
				break;
			case 5:
				perCapitaIncome = 5600;
				break;
			case 4:
				perCapitaIncome = 3600;
				break;
			case 3:
				perCapitaIncome = 2200;
				break;
			case 2:
				perCapitaIncome = 1400;
				break;
			case 1:
				perCapitaIncome = 880;
				break;
			default:
				perCapitaIncome = 560;
				break;
			}

			double modifier = 1.0;
			if (tradeClasses.HasFlag(TravellerTradeClassKind.Rich))
				modifier *= 1.6;
			if (tradeClasses.HasFlag(TravellerTradeClassKind.Industrial))
				modifier *= 1.4;
			if (tradeClasses.HasFlag(TravellerTradeClassKind.Agricultural))
				modifier *= 1.2;
			if (tradeClasses.HasFlag(TravellerTradeClassKind.Extreme))
				modifier *= 0.8;
			if (tradeClasses.HasFlag(TravellerTradeClassKind.NonIndustrial))
				modifier *= 0.8;
			if (tradeClasses.HasFlag(TravellerTradeClassKind.Poor))
				modifier *= 0.8;
			perCapitaIncome = (int) ((double) perCapitaIncome * modifier);

			return perCapitaIncome;
		}

		public static int CalculateCapitolScore(int techLevel, TravellerStarportClass starportClass, double populationRating, TravellerHazardZoneKind hazardZoneKind, TravellerGovernmentKind governmentKind)
		{
			int score = 0;

			// tech level
			if (techLevel > 11)
				score += 6;
			else if (techLevel == 11)
				score += 10;
			else if (techLevel == 10)
				score += 8;
			else
				score += -2 - (10 * (9 - techLevel));

			// starport
			switch (starportClass)
			{
			case TravellerStarportClass.A:
				score += 5;
				break;
			case TravellerStarportClass.B:
				score += 1;
				break;
			case TravellerStarportClass.C:
				score -= 5;
				break;
			case TravellerStarportClass.D:
				score -= 10;
				break;
			case TravellerStarportClass.E:
				score -= 15;
				break;
			default:
				score -= 25;
				break;
			}

			// population
			if (populationRating > 5.0)
				score += (int) populationRating - 5;
			else
				score += 2 * ((int) populationRating - 5);

			// hazard
			switch (hazardZoneKind)
			{
			case TravellerHazardZoneKind.Amber:
				score -= 25;
				break;
			case TravellerHazardZoneKind.Red:
				score -= 50;
				break;
			default:
				break;
			}

			// government
			switch (governmentKind)
			{
			case TravellerGovernmentKind.Undefined:
				score -= 25;
				break;
			case TravellerGovernmentKind.Anarchy:
				score -= 25;
				break;
			case TravellerGovernmentKind.CaptiveGovernment:
				score -= 20;
				break;
			case TravellerGovernmentKind.Balkanized:
				score -= 20;
				break;
			case TravellerGovernmentKind.ClanTribal:
				score -= 16;
				break;
			case TravellerGovernmentKind.CorporateState:
				score -= 12;
				break;
			case TravellerGovernmentKind.Dictatorship:
				score -= 8;
				break;
			case TravellerGovernmentKind.Theocracy:
				score -= 8;
				break;
			case TravellerGovernmentKind.Caste:
				score -= 6;
				break;
			case TravellerGovernmentKind.Military:
				score -= 4;
				break;
			case TravellerGovernmentKind.Feudal:
				score -= 2;
				break;
			default:
				break;
			}

			return score;
		}
	}
}
