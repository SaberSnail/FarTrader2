using System;
using GoldenAnvil.Utility;

namespace FarTrader.DataModels
{
	internal static class TravellerRandomSystemDataUtility
	{
		public static void GetRandomWorldKind(Random rng, out TravellerWorldKind worldKind, ref TravellerAtmosphereDensity atmosphereDensity)
		{
			int value = rng.NextRoll(4, 6);
			if (value <= 7)
			{
				worldKind = TravellerWorldKind.AsteroidBelt;
			}
			else if (value <= 9)
			{
				if (rng.NextRoll(3, 6) <= 12)
					worldKind = TravellerWorldKind.BarrenRock;
				else
					worldKind = TravellerWorldKind.BarrenIce;
			}
			else if (value <= 10)
			{
				worldKind = TravellerWorldKind.DesertRock;
			}
			else if (value <= 12)
			{
				worldKind = TravellerWorldKind.Garden;
				atmosphereDensity = TravellerAtmosphereDensity.VeryThin;
			}
			else if (value <= 14)
			{
				worldKind = TravellerWorldKind.Garden;
				atmosphereDensity = TravellerAtmosphereDensity.Thin;
			}
			else if (value <= 16)
			{
				worldKind = TravellerWorldKind.Garden;
				atmosphereDensity = TravellerAtmosphereDensity.Standard;
			}
			else if (value <= 18)
			{
				worldKind = TravellerWorldKind.Garden;
				atmosphereDensity = TravellerAtmosphereDensity.Dense;
			}
			else if (value <= 21)
			{
				value = rng.NextRoll(3, 6);
				if (value <= 4)
				{
					worldKind = TravellerWorldKind.SubGiant;
				}
				else if (value <= 6)
				{
					worldKind = TravellerWorldKind.DesertIce;
				}
				else if (value <= 9)
				{
					worldKind = TravellerWorldKind.Glacier;
				}
				else if (value <= 15)
				{
					worldKind = TravellerWorldKind.PreGarden;
				}
				else
				{
					worldKind = TravellerWorldKind.Greenhouse;
				}
			}
			else
			{
				worldKind = TravellerWorldKind.Garden;
				atmosphereDensity = TravellerAtmosphereDensity.VeryDense;
			}
		}

		public static void GetRandomAtmosphere(Random rng, TravellerWorldKind worldKind, out TravellerAtmosphereKind atmosphereKind, ref TravellerAtmosphereDensity atmosphereDensity)
		{
			switch (worldKind)
			{
			case TravellerWorldKind.AsteroidBelt:
			case TravellerWorldKind.BarrenIce:
			case TravellerWorldKind.BarrenRock:
				atmosphereKind = TravellerAtmosphereKind.None;
				atmosphereDensity = TravellerAtmosphereDensity.None;
				break;

			case TravellerWorldKind.DesertIce:
				if (rng.NextRoll(3, 6) <= 9)
					atmosphereKind = TravellerAtmosphereKind.Toxic;
				else
					atmosphereKind = TravellerAtmosphereKind.Corrosive;
				atmosphereDensity = TravellerAtmosphereDensity.Standard;
				break;

			case TravellerWorldKind.DesertRock:
				if (rng.NextRoll(3, 6) <= 15)
				{
					atmosphereKind = TravellerAtmosphereKind.Normal;
					atmosphereDensity = TravellerAtmosphereDensity.Trace;
				}
				else
				{
					atmosphereKind = TravellerAtmosphereKind.Suffocating;
					atmosphereDensity = TravellerAtmosphereDensity.Standard;
				}
				break;

			case TravellerWorldKind.Garden:
				if (rng.NextRoll(3, 6) <= 9)
				{
					int value = rng.NextRoll(3, 6);
					if (value <= 4)
						atmosphereKind = TravellerAtmosphereKind.ChlorineFlourine;
					else if (value <= 6)
						atmosphereKind = TravellerAtmosphereKind.SulfurCompounds;
					else if (value <= 8)
						atmosphereKind = TravellerAtmosphereKind.NitrogenCompounds;
					else if (value <= 10)
						atmosphereKind = TravellerAtmosphereKind.OrganicToxins;
					else if (value <= 12)
						atmosphereKind = TravellerAtmosphereKind.Pollutants;
					else if (value <= 14)
						atmosphereKind = TravellerAtmosphereKind.HighCarbonDioxide;
					else if (value <= 16)
						atmosphereKind = TravellerAtmosphereKind.HighOxygen;
					else
						atmosphereKind = TravellerAtmosphereKind.InertGases;
				}
				else
				{
					atmosphereKind = TravellerAtmosphereKind.Normal;
				}
				break;

			case TravellerWorldKind.Glacier:
			case TravellerWorldKind.PreGarden:
				if (rng.NextRoll(3, 6) <= 12)
					atmosphereKind = TravellerAtmosphereKind.Suffocating;
				else
					atmosphereKind = TravellerAtmosphereKind.Toxic;
				atmosphereDensity = TravellerAtmosphereDensity.Standard;
				break;

			case TravellerWorldKind.Greenhouse:
				if (rng.NextRoll(3, 6) <= 12)
					atmosphereKind = TravellerAtmosphereKind.Insidious;
				else
					atmosphereKind = TravellerAtmosphereKind.Corrosive;
				atmosphereDensity = TravellerAtmosphereDensity.SuperDense;
				break;

			case TravellerWorldKind.SubGiant:
				if (rng.NextRoll(3, 6) <= 12)
					atmosphereKind = TravellerAtmosphereKind.Corrosive;
				else
					atmosphereKind = TravellerAtmosphereKind.Toxic;
				atmosphereDensity = TravellerAtmosphereDensity.VeryDense;
				break;

			default:
				throw new ArgumentOutOfRangeException("worldKind", String.Format("Unsupported world kind {0}", worldKind));
			}
		}

		public static void GetRandomDiameter(Random rng, TravellerWorldKind worldKind, TravellerAtmosphereDensity atmosphereDensity, out int diameter, out int densityModifier)
		{
			int value;
			densityModifier = 0;

			switch (worldKind)
			{
			case TravellerWorldKind.AsteroidBelt:
				diameter = 0;
				break;

			case TravellerWorldKind.BarrenIce:
				value = rng.NextRoll(3, 6);
				if (value <= 8)
				{
					diameter = 1000;
					densityModifier = -4;
				}
				else if (value <= 12)
				{
					diameter = 1500;
					densityModifier = -1;
				}
				else if (value <= 14)
				{
					diameter = 2000;
				}
				else if (value <= 16)
				{
					diameter = 2500;
					densityModifier = -4;
				}
				else
				{
					diameter = 3000;
					densityModifier = -8;
				}
				break;

			case TravellerWorldKind.BarrenRock:
				value = rng.NextRoll(3, 6);
				if (value <= 8)
				{
					diameter = 1000;
					densityModifier = -8;
				}
				else if (value <= 10)
				{
					diameter = 1500;
					densityModifier = -4;
				}
				else if (value <= 12)
				{
					diameter = 2000;
					densityModifier = -1;
				}
				else if (value <= 14)
				{
					diameter = 2500;
				}
				else if (value <= 16)
				{
					diameter = 3000;
					densityModifier = -4;
				}
				else if (value <= 17)
				{
					diameter = 3500;
					densityModifier = -8;
				}
				else
				{
					diameter = 4000;
					densityModifier = -12;
				}
				break;

			case TravellerWorldKind.DesertIce:
				value = rng.NextRoll(3, 6);
				if (value <= 8)
				{
					diameter = 3000;
					densityModifier = 8;
				}
				else if (value <= 12)
				{
					diameter = 3500;
					densityModifier = 4;
				}
				else if (value <= 14)
				{
					diameter = 4000;
				}
				else if (value <= 16)
				{
					diameter = 5500;
				}
				else
				{
					diameter = 6000;
				}
				break;

			case TravellerWorldKind.DesertRock:
				value = rng.NextRoll(3, 6);
				if (value <= 5)
				{
					diameter = 3000;
					densityModifier = 12;
				}
				else if (value <= 8)
				{
					diameter = 3500;
					densityModifier = 4;
				}
				else if (value <= 12)
				{
					diameter = 4000;
				}
				else if (value <= 15)
				{
					diameter = 4500;
					densityModifier = -4;
				}
				else
				{
					diameter = 5000;
					densityModifier = -8;
				}
				break;

			case TravellerWorldKind.Garden:
			case TravellerWorldKind.PreGarden:
				value = rng.NextRoll(3, 6);
				if (worldKind == TravellerWorldKind.Garden)
				{
					switch (atmosphereDensity)
					{
					case TravellerAtmosphereDensity.VeryThin:
						value -= 6;
						break;
					case TravellerAtmosphereDensity.Thin:
						value -= 3;
						break;
					case TravellerAtmosphereDensity.Dense:
						value += 3;
						break;
					case TravellerAtmosphereDensity.VeryDense:
						value += 6;
						break;
					}
				}
				if (value <= 3)
				{
					diameter = 4500;
					densityModifier = 15;
				}
				else if (value <= 4)
				{
					diameter = 5000;
					densityModifier = 8;
				}
				else if (value <= 6)
				{
					diameter = 5500;
				}
				else if (value <= 8)
				{
					diameter = 6000;
				}
				else if (value <= 10)
				{
					diameter = 6500;
				}
				else if (value <= 12)
				{
					diameter = 7000;
				}
				else if (value <= 14)
				{
					diameter = 7500;
				}
				else if (value <= 16)
				{
					diameter = 8000;
					densityModifier = -1;
				}
				else if (value <= 17)
				{
					diameter = 8500;
					densityModifier = -4;
				}
				else
				{
					diameter = 9000;
					densityModifier = -8;
				}
				break;

			case TravellerWorldKind.Glacier:
				value = rng.NextRoll(3, 6);
				if (value <= 3)
				{
					diameter = 4000;
					densityModifier = 12;
				}
				else if (value <= 4)
				{
					diameter = 4500;
				}
				else if (value <= 6)
				{
					diameter = 5000;
				}
				else if (value <= 8)
				{
					diameter = 5500;
				}
				else if (value <= 10)
				{
					diameter = 6000;
				}
				else if (value <= 12)
				{
					diameter = 6500;
				}
				else if (value <= 14)
				{
					diameter = 7000;
				}
				else if (value <= 16)
				{
					diameter = 7500;
				}
				else if (value <= 17)
				{
					diameter = 8000;
					densityModifier = -4;
				}
				else
				{
					diameter = 8500;
					densityModifier = -8;
				}
				break;

			case TravellerWorldKind.Greenhouse:
				value = rng.NextRoll(3, 6);
				if (value <= 3)
				{
					diameter = 4500;
					densityModifier = 15;
				}
				else if (value <= 4)
				{
					diameter = 5000;
					densityModifier = 8;
				}
				else if (value <= 5)
				{
					diameter = 5500;
				}
				else if (value <= 6)
				{
					diameter = 6000;
				}
				else if (value <= 8)
				{
					diameter = 6500;
				}
				else if (value <= 10)
				{
					diameter = 7000;
				}
				else if (value <= 12)
				{
					diameter = 7500;
				}
				else if (value <= 14)
				{
					diameter = 8000;
				}
				else if (value <= 15)
				{
					diameter = 8500;
				}
				else if (value <= 16)
				{
					diameter = 9000;
					densityModifier = -1;
				}
				else if (value <= 17)
				{
					diameter = 9500;
					densityModifier = -4;
				}
				else
				{
					diameter = 10000;
					densityModifier = -8;
				}
				break;

			case TravellerWorldKind.SubGiant:
				value = rng.NextRoll(3, 6);
				if (value <= 4)
				{
					diameter = 8000;
					densityModifier = 8;
				}
				else if (value <= 7)
				{
					diameter = 8500;
					densityModifier = 4;
				}
				else if (value <= 10)
				{
					diameter = 9000;
				}
				else if (value <= 13)
				{
					diameter = 9500;
				}
				else
				{
					diameter = 10000;
				}
				break;

			default:
				throw new ArgumentOutOfRangeException("worldKind", String.Format("Unsupported world kind {0}", worldKind));
			}

			if (diameter > 0)
			{
				switch (rng.NextRoll(1, 6))
				{
				case 1:
					diameter -= 200;
					break;
				case 2:
					diameter -= 100;
					break;
				case 5:
					diameter += 100;
					break;
				case 6:
					diameter += 200;
					break;
				}
			}
		}

		public static void GetRandomDensity(Random rng, TravellerWorldKind worldKind, int densityModifier, out double density)
		{
			int value;

			switch (worldKind)
			{
			case TravellerWorldKind.AsteroidBelt:
				density = 0.0;
				break;

			case TravellerWorldKind.BarrenIce:
			case TravellerWorldKind.DesertIce:
				// Icy Core
				value = rng.NextRoll(3, 6) + densityModifier;
				if (value <= 6)
					density = 0.3;
				else if (value <= 10)
					density = 0.4;
				else if (value <= 14)
					density = 0.5;
				else if (value <= 17)
					density = 0.6;
				else
					density = 0.7;
				break;

			case TravellerWorldKind.BarrenRock:
			case TravellerWorldKind.DesertRock:
				// Small Iron Core
				value = rng.NextRoll(3, 6) + densityModifier;
				if (value <= 6)
					density = 0.6;
				else if (value <= 10)
					density = 0.7;
				else if (value <= 14)
					density = 0.8;
				else if (value <= 17)
					density = 0.9;
				else
					density = 1.0;
				break;

			case TravellerWorldKind.Garden:
			case TravellerWorldKind.Glacier:
			case TravellerWorldKind.Greenhouse:
			case TravellerWorldKind.PreGarden:
			case TravellerWorldKind.SubGiant:
				// Large Iron Core
				value = rng.NextRoll(3, 6) + densityModifier;
				if (value <= 6)
					density = 0.8;
				else if (value <= 10)
					density = 0.9;
				else if (value <= 14)
					density = 1.0;
				else if (value <= 17)
					density = 1.1;
				else if (value <= 20)
					density = 1.2;
				else if (value <= 23)
					density = 1.3;
				else
					density = 1.4;
				break;

			default:
				throw new ArgumentOutOfRangeException("worldKind", String.Format("Unsupported world kind {0}", worldKind));
			}
		}

		public static void GetRandomAsteroidBeltsCount(Random rng, TravellerWorldKind worldKind, out int asteroidBeltsCount)
		{
			// calculated from average values in Spinward Marches
			int value = rng.NextRoll(1, 100);
			if (value <= 33)
				asteroidBeltsCount = 0;
			else if (value <= 65)
				asteroidBeltsCount = 1;
			else if (value <= 96)
				asteroidBeltsCount = 2;
			else
				asteroidBeltsCount = 3;

			if (worldKind == TravellerWorldKind.AsteroidBelt && asteroidBeltsCount == 0)
				asteroidBeltsCount = 1;
		}

		public static void GetRandomGasGiantsCount(Random rng, out int gasGiantsCount)
		{
			// calculated from average values in Spinward Marches
			int value = rng.NextRoll(1, 100);
			if (value <= 32)
				gasGiantsCount = 0;
			else if (value <= 55)
				gasGiantsCount = 1;
			else if (value <= 70)
				gasGiantsCount = 2;
			else if (value <= 84)
				gasGiantsCount = 3;
			else if (value <= 97)
				gasGiantsCount = 4;
			else
				gasGiantsCount = 5;
		}

		public static void GetRandomHydrographicCoverage(Random rng, TravellerWorldKind worldKind, out double hydrographicCoverage)
		{
			switch (worldKind)
			{
			case TravellerWorldKind.AsteroidBelt:
			case TravellerWorldKind.BarrenIce:
			case TravellerWorldKind.BarrenRock:
			case TravellerWorldKind.DesertRock:
				hydrographicCoverage = 0;
				break;

			case TravellerWorldKind.DesertIce:
			case TravellerWorldKind.Garden:
			case TravellerWorldKind.PreGarden:
			case TravellerWorldKind.SubGiant:
				hydrographicCoverage = 10 * (rng.NextRoll(2, 6) - 2) + rng.NextRoll(2, 6) - 7;
				break;

			case TravellerWorldKind.Glacier:
				hydrographicCoverage = 10 * (rng.NextRoll(2, 6) - 10) + rng.NextRoll(2, 6) - 7;
				break;

			case TravellerWorldKind.Greenhouse:
				hydrographicCoverage = 10 * (rng.NextRoll(2, 6) - 7) + rng.NextRoll(2, 6) - 7;
				break;

			default:
				throw new ArgumentOutOfRangeException("worldKind", String.Format("Unsupported world kind {0}", worldKind));
			}

			if (hydrographicCoverage < 0)
				hydrographicCoverage = 0;
			if (hydrographicCoverage > 100)
				hydrographicCoverage = 100;
		}

		public static void GetRandomClimateKind(Random rng, TravellerWorldKind worldKind, out TravellerClimateKind climateKind)
		{
			int value;
			switch (worldKind)
			{
			case TravellerWorldKind.AsteroidBelt:
				value = rng.NextRoll(3, 6);
				if (value <= 6) climateKind = TravellerClimateKind.Infernal;
				else if (value <= 9) climateKind = TravellerClimateKind.Undefined;
				else climateKind = TravellerClimateKind.Frozen;
				break;

			case TravellerWorldKind.BarrenIce:
			case TravellerWorldKind.DesertIce:
			case TravellerWorldKind.Glacier:
			case TravellerWorldKind.SubGiant:
				climateKind = TravellerClimateKind.Frozen;
				break;

			case TravellerWorldKind.BarrenRock:
			case TravellerWorldKind.DesertRock:
				value = rng.NextRoll(3, 6);
				if (value <= 8) climateKind = TravellerClimateKind.Infernal;
				else if (value <= 12) climateKind = TravellerClimateKind.Undefined;
				else climateKind = TravellerClimateKind.Frozen;
				break;

			case TravellerWorldKind.Garden:
			case TravellerWorldKind.PreGarden:
				climateKind = TravellerClimateKind.Undefined;
				break;

			case TravellerWorldKind.Greenhouse:
				climateKind = TravellerClimateKind.Infernal;
				break;

			default:
				throw new ArgumentOutOfRangeException("worldKind", String.Format("Unsupported world kind {0}", worldKind));
			}

			if (climateKind == TravellerClimateKind.Undefined)
			{
				value = rng.NextRoll(3, 6);
				if (value <= 3)
					climateKind = TravellerClimateKind.VeryCold;
				else if (value <= 5)
					climateKind = TravellerClimateKind.Cold;
				else if (value <= 7)
					climateKind = TravellerClimateKind.Chilly;
				else if (value <= 9)
					climateKind = TravellerClimateKind.Cool;
				else if (value <= 11)
					climateKind = TravellerClimateKind.Average;
				else if (value <= 13)
					climateKind = TravellerClimateKind.Warm;
				else if (value <= 15)
					climateKind = TravellerClimateKind.Tropical;
				else if (value <= 17)
					climateKind = TravellerClimateKind.Hot;
				else
					climateKind = TravellerClimateKind.VeryHot;
			}
		}

		public static void GetRandomResourceDensity(Random rng, TravellerWorldKind worldKind, out TravellerResourceDensity resourceDensity)
		{
			if (worldKind == TravellerWorldKind.AsteroidBelt)
			{
				int value = rng.NextRoll(3, 6);
				if (value <= 3)
					resourceDensity = TravellerResourceDensity.Worthless;
				else if (value <= 4)
					resourceDensity = TravellerResourceDensity.VeryScant;
				else if (value <= 5)
					resourceDensity = TravellerResourceDensity.Scant;
				else if (value <= 7)
					resourceDensity = TravellerResourceDensity.VeryPoor;
				else if (value <= 9)
					resourceDensity = TravellerResourceDensity.Poor;
				else if (value <= 11)
					resourceDensity = TravellerResourceDensity.Average;
				else if (value <= 13)
					resourceDensity = TravellerResourceDensity.Rich;
				else if (value <= 15)
					resourceDensity = TravellerResourceDensity.VeryRich;
				else if (value <= 16)
					resourceDensity = TravellerResourceDensity.Abundant;
				else if (value <= 17)
					resourceDensity = TravellerResourceDensity.VeryAbundant;
				else
					resourceDensity = TravellerResourceDensity.Motherlode;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (value <= 4)
					resourceDensity = TravellerResourceDensity.VeryPoor;
				else if (value <= 7)
					resourceDensity = TravellerResourceDensity.Poor;
				else if (value <= 13)
					resourceDensity = TravellerResourceDensity.Average;
				else if (value <= 16)
					resourceDensity = TravellerResourceDensity.Rich;
				else
					resourceDensity = TravellerResourceDensity.VeryRich;
			}
		}

		public static void GetRandomPopulation(Random rng, TravellerResourceDensity resourceDensity, double maxPopulationRating, out double popuplationRating, out long population)
		{
			popuplationRating = rng.NextDouble(0.5, 6.5) + rng.NextDouble(0.5, 6.5) - 2.0;
			if (popuplationRating < 0.0)
				popuplationRating = 0.0;
			if (popuplationRating > 10.0)
				popuplationRating = 10.0;
			popuplationRating += TravellerSystemDataUtility.GetResourceModifier(resourceDensity);
			if (maxPopulationRating < 5)
				popuplationRating -= (5 - maxPopulationRating);
			if (popuplationRating < 1)
				popuplationRating = 0.0;

			if (popuplationRating > 0.0)
				population = (long) Math.Floor(Math.Pow(10.0, popuplationRating));
			else
				population = 0;
		}

		public static void GetRandomStarportClass(Random rng, double popuplationRating, out TravellerStarportClass starportClass)
		{
			starportClass = TravellerStarportClass.X;
			if (popuplationRating >= 6)
			{
				if (rng.NextRoll(3, 6) < popuplationRating + 3)
					starportClass = TravellerStarportClass.A;
			}
			if ((starportClass == TravellerStarportClass.X) && (popuplationRating >= 6))
			{
				if (rng.NextRoll(3, 6) < popuplationRating + 6)
					starportClass = TravellerStarportClass.B;
			}
			if (starportClass == TravellerStarportClass.X)
			{
				if (rng.NextRoll(3, 6) < popuplationRating + 9)
					starportClass = TravellerStarportClass.C;
			}
			if (starportClass == TravellerStarportClass.X)
			{
				if (rng.NextRoll(3, 6) < popuplationRating + 8)
					starportClass = TravellerStarportClass.D;
			}
			if (starportClass == TravellerStarportClass.X)
			{
				if (rng.NextRoll(3, 6) < 15)
					starportClass = TravellerStarportClass.E;
			}
		}

		public static void GetRandomExtraBases(Random rng, TravellerStarportClass starportClass, double popuplationRating, out TravellerExtraBaseKind extraBases)
		{
			extraBases = TravellerExtraBaseKind.None;

			if (starportClass >= TravellerStarportClass.B && rng.NextRoll(2, 6) >= 8)
				extraBases |= TravellerExtraBaseKind.Naval;

			if (starportClass >= TravellerStarportClass.D)
			{
				int value = rng.NextRoll(2, 6);
				if (starportClass == TravellerStarportClass.C)
					value += 2;
				else if (starportClass == TravellerStarportClass.B)
					value += 1;
				if (value >= 10)
					extraBases |= TravellerExtraBaseKind.Scout;
			}

			if (starportClass >= TravellerStarportClass.C)
			{
				int value = rng.NextRoll(2, 6);
				if (popuplationRating >= 8)
					value += 1;
				if (starportClass == TravellerStarportClass.C)
					value += 2;
				else if (starportClass == TravellerStarportClass.B)
					value += 1;
				if (extraBases.HasFlag(TravellerExtraBaseKind.Naval | TravellerExtraBaseKind.Scout))
					value -= 2;
				if (value >= 10)
					extraBases |= TravellerExtraBaseKind.Military;
			}

			if (rng.NextRoll(3, 6) >= 17)
				extraBases |= TravellerExtraBaseKind.Research;
		}

		public static void GetRandomHazardZoneKind(Random rng, TravellerStarportClass starportClass, out TravellerHazardZoneKind hazardZoneKind)
		{
			hazardZoneKind = TravellerHazardZoneKind.None;
			int value = rng.NextRoll(3, 6);
			if (starportClass == TravellerStarportClass.X)
				value += 4;
			if (value >= 17)
				hazardZoneKind = TravellerHazardZoneKind.Red;
			else if (value >= 13)
				hazardZoneKind = TravellerHazardZoneKind.Amber;
		}

		public static void GetRandomGovernmentKind(Random rng, double populationRating, out TravellerGovernmentKind governmentKind, out int controlRatingModifier)
		{
			if (populationRating == 0)
			{
				governmentKind = TravellerGovernmentKind.Undefined;
				controlRatingModifier = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6) - 7 + (int) populationRating;
				if (value <= 0)
					governmentKind = TravellerGovernmentKind.Anarchy;
				else if (value <= 1)
					governmentKind = TravellerGovernmentKind.CorporateState;
				else if (value <= 2)
					governmentKind = TravellerGovernmentKind.AthenianDemocracy;
				else if (value <= 3)
					governmentKind = TravellerGovernmentKind.Oligarchy;
				else if (value <= 4)
					governmentKind = TravellerGovernmentKind.RepresentativeDemocracy;
				else if (value <= 5)
					governmentKind = TravellerGovernmentKind.Technocracy;
				else if (value <= 6)
					governmentKind = TravellerGovernmentKind.CaptiveGovernment;
				else if (value <= 7)
					governmentKind = TravellerGovernmentKind.Balkanized;
				else if (value <= 8)
					governmentKind = TravellerGovernmentKind.Meritocracy;
				else if (value <= 9)
					governmentKind = TravellerGovernmentKind.Bureaucracy;
				else if (value <= 11)
					governmentKind = TravellerGovernmentKind.Dictatorship;
				else if (value <= 12)
					governmentKind = TravellerGovernmentKind.Oligarchy;
				else
					governmentKind = TravellerGovernmentKind.Theocracy;
				controlRatingModifier = value;
			}
		}

		public static void GetRandomControlRating(Random rng, double populationRating, int controlRatingModifier, out int controlRating)
		{
			if (populationRating == 0)
			{
				controlRating = 0;
			}
			else
			{
				int value = rng.NextRoll(2, 6) - 7 + controlRatingModifier;
				if (value <= 0)
					controlRating = 0;
				else if (value <= 2)
					controlRating = 1;
				else if (value <= 4)
					controlRating = 2;
				else if (value <= 5)
					controlRating = 3;
				else if (value <= 7)
					controlRating = 4;
				else if (value <= 8)
					controlRating = 5;
				else
					controlRating = 6;
			}
		}

		public static void GetRandomTechLevel(Random rng, double populationRating, TravellerStarportClass starportClass, int diameter, TravellerWorldKind worldKind, TravellerAtmosphereDensity atmosphereDensity, double hydrographicCoverage, TravellerGovernmentKind governmentKind, out int techLevel)
		{
			if (populationRating == 0)
			{
				techLevel = 0;
			}
			else
			{
				int value = rng.NextRoll(1, 6);

				if (starportClass == TravellerStarportClass.A)
					value += 6;
				else if (starportClass == TravellerStarportClass.B)
					value += 4;
				else if (starportClass == TravellerStarportClass.C)
					value += 2;
				else if (starportClass == TravellerStarportClass.X)
					value -= 4;

				if (diameter < 1500 || worldKind == TravellerWorldKind.AsteroidBelt)
					value += 2;
				else if (diameter < 4500)
					value += 1;

				if (atmosphereDensity < TravellerAtmosphereDensity.Thin || atmosphereDensity > TravellerAtmosphereDensity.Dense)
					value += 1;

				if (hydrographicCoverage > 0.85 && hydrographicCoverage <= 0.95)
					value += 1;
				else if (hydrographicCoverage > 0.95)
					value += 2;

				if (populationRating > 0 && populationRating <= 5)
					value += 1;
				else if (populationRating >= 9 && populationRating < 10)
					value += 2;
				else if (populationRating >= 10)
					value += 4;

				if (governmentKind == TravellerGovernmentKind.Anarchy || governmentKind == TravellerGovernmentKind.Technocracy || governmentKind == TravellerGovernmentKind.Military)
					value += 1;
				else if (governmentKind == TravellerGovernmentKind.Theocracy)
					value -= 2;

				if (value <= 1)
				{
					value = rng.NextRoll(1, 6);
					if (value <= 2)
						techLevel = 1;
					else if (value <= 4)
						techLevel = 2;
					else
						techLevel = 3;
				}
				else if (value <= 2)
				{
					techLevel = 4;
				}
				else if (value <= 4)
				{
					techLevel = 5;
				}
				else if (value <= 6)
				{
					techLevel = 6;
				}
				else if (value <= 7)
				{
					techLevel = 7;
				}
				else if (value <= 8)
				{
					techLevel = 8;
				}
				else if (value <= 9)
				{
					techLevel = 9;
				}
				else if (value <= 11)
				{
					techLevel = 10;
				}
				else
				{
					techLevel = 11;
				}

				if (worldKind != TravellerWorldKind.Garden || atmosphereDensity <= TravellerAtmosphereDensity.VeryThin)
				{
					if (techLevel < 8)
						techLevel = 8;
				}
			}
		}

		public static void GetRandomPluralism(Random rng, double populationRating, TravellerGovernmentKind governmentKind, int controlRating, out int pluralism)
		{
			if (populationRating == 0)
			{
				pluralism = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (governmentKind == TravellerGovernmentKind.Balkanized)
					value += 2;
				if (controlRating == 0)
					value += 2;
				if (controlRating == 1)
					value += 1;
				if (controlRating == 5)
					value -= 1;
				if (controlRating == 6)
					value -= 2;

				if (value <= 5)
					pluralism = -3;
				else if (value <= 7)
					pluralism = -2;
				else if (value <= 9)
					pluralism = -1;
				else if (value <= 11)
					pluralism = 0;
				else if (value <= 13)
					pluralism = 1;
				else if (value <= 15)
					pluralism = 2;
				else
					pluralism = 3;
			}
		}

		public static void GetRandomToleration(Random rng, double populationRating, int pluralism, TravellerStarportClass starportClass, TravellerHazardZoneKind hazardZoneKind, out int toleration)
		{
			if (populationRating == 0)
			{
				toleration = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (pluralism >= 2)
					value += 2;
				if (pluralism <= -2)
					value -= 2;
				if (starportClass == TravellerStarportClass.A && hazardZoneKind == TravellerHazardZoneKind.None)
					value += 2;
				if (starportClass == TravellerStarportClass.B && hazardZoneKind == TravellerHazardZoneKind.None)
					value += 1;
				if (starportClass <= TravellerStarportClass.E && hazardZoneKind == TravellerHazardZoneKind.None)
					value -= 1;
				if (hazardZoneKind != TravellerHazardZoneKind.None)
					value -= 2;

				if (value <= 5)
					toleration = -3;
				else if (value <= 7)
					toleration = -2;
				else if (value <= 9)
					toleration = -1;
				else if (value <= 11)
					toleration = 0;
				else if (value <= 13)
					toleration = 1;
				else if (value <= 15)
					toleration = 2;
				else
					toleration = 3;
			}
		}

		public static void GetRandomSolidarity(Random rng, double populationRating, long population, out int solidarity)
		{
			if (populationRating == 0)
			{
				solidarity = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (population >= 1000000000)
					value += 2;
				else if (population >= 100000000)
					value += 1;
				if (population < 1000000)
					value -= 2;
				else if (population < 10000000)
					value -= 1;

				if (value <= 5)
					solidarity = -3;
				else if (value <= 7)
					solidarity = -2;
				else if (value <= 9)
					solidarity = -1;
				else if (value <= 11)
					solidarity = 0;
				else if (value <= 13)
					solidarity = 1;
				else if (value <= 15)
					solidarity = 2;
				else
					solidarity = 3;
			}
		}

		public static void GetRandomTractability(Random rng, double populationRating, int solidarity, TravellerGovernmentKind governmentKind, out int tractability)
		{
			if (populationRating == 0)
			{
				tractability = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (solidarity >= 2)
					value += 2;
				if (solidarity <= -2)
					value -= 2;
				if (governmentKind == TravellerGovernmentKind.Caste || governmentKind == TravellerGovernmentKind.Dictatorship || governmentKind == TravellerGovernmentKind.Feudal || governmentKind == TravellerGovernmentKind.Theocracy || governmentKind == TravellerGovernmentKind.Military)
					value += 2;
				if (governmentKind == TravellerGovernmentKind.CorporateState || governmentKind == TravellerGovernmentKind.Oligarchy)
					value += 1;
				if (governmentKind == TravellerGovernmentKind.AthenianDemocracy)
					value -= 1;
				if (governmentKind == TravellerGovernmentKind.Anarchy || governmentKind == TravellerGovernmentKind.ClanTribal)
					value -= 2;

				if (value <= 5)
					tractability = -3;
				else if (value <= 7)
					tractability = -2;
				else if (value <= 9)
					tractability = -1;
				else if (value <= 11)
					tractability = 0;
				else if (value <= 13)
					tractability = 1;
				else if (value <= 15)
					tractability = 2;
				else
					tractability = 3;
			}
		}

		public static void GetRandomAggression(Random rng, double populationRating, int tractability, int controlRating, out int aggression)
		{
			if (populationRating == 0)
			{
				aggression = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (tractability <= -2)
					value += 2;
				if (tractability >= 2)
					value -= 2;
				if (controlRating == 0)
					value += 2;
				if (controlRating == 1)
					value += 1;
				if (controlRating == 5)
					value -= 1;
				if (controlRating == 6)
					value -= 2;

				if (value <= 5)
					aggression = -3;
				else if (value <= 7)
					aggression = -2;
				else if (value <= 9)
					aggression = -1;
				else if (value <= 11)
					aggression = 0;
				else if (value <= 13)
					aggression = 1;
				else if (value <= 15)
					aggression = 2;
				else
					aggression = 3;
			}
		}

		public static void GetRandomPragmatism(Random rng, double populationRating, int techLevel, out int pragmatism)
		{
			if (populationRating == 0)
			{
				pragmatism = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (techLevel >= 6 && techLevel <= 8)
					value += 2;
				if (techLevel == 5 || techLevel == 9)
					value += 1;
				if (techLevel == 2 || techLevel == 3)
					value -= 1;
				if (techLevel <= 1)
					value -= 2;

				if (value <= 5)
					pragmatism = -3;
				else if (value <= 7)
					pragmatism = -2;
				else if (value <= 9)
					pragmatism = -1;
				else if (value <= 11)
					pragmatism = 0;
				else if (value <= 13)
					pragmatism = 1;
				else if (value <= 15)
					pragmatism = 2;
				else
					pragmatism = 3;
			}
		}

		public static void GetRandomInnovation(Random rng, double populationRating, int pragmatism, int toleration, out int innovation)
		{
			if (populationRating == 0)
			{
				innovation = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (pragmatism >= 2)
					value += 2;
				if (pragmatism <= -2)
					value -= 2;
				if (toleration >= 2)
					value += 1;
				if (toleration <= -2)
					value -= 1;

				if (value <= 5)
					innovation = -3;
				else if (value <= 7)
					innovation = -2;
				else if (value <= 9)
					innovation = -1;
				else if (value <= 11)
					innovation = 0;
				else if (value <= 13)
					innovation = 1;
				else if (value <= 15)
					innovation = 2;
				else
					innovation = 3;
			}
		}

		public static void GetRandomProvidence(Random rng, double populationRating, int innovation, int solidarity, out int providence)
		{
			if (populationRating == 0)
			{
				providence = 0;
			}
			else
			{
				int value = rng.NextRoll(3, 6);
				if (innovation <= -2)
					value += 2;
				if (innovation >= 2)
					value -= 2;
				if (solidarity >= 2)
					value += 1;
				if (solidarity <= -2)
					value -= 1;

				if (value <= 5)
					providence = -3;
				else if (value <= 7)
					providence = -2;
				else if (value <= 9)
					providence = -1;
				else if (value <= 11)
					providence = 0;
				else if (value <= 13)
					providence = 1;
				else if (value <= 15)
					providence = 2;
				else
					providence = 3;
			}
		}
	}
}
