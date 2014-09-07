using System;
using System.Collections.Generic;
using System.Linq;

namespace FarTrader.DataModels
{
	internal static class RandomSystemDataUtility
	{
		public static long GetPopulation(Random rng)
		{
			return (long) (rng.NextDouble() * 10000000000);
		}

		public static bool GetIsInterdicted(Random rng)
		{
			return rng.NextDouble() < 0.05;
		}

		public static PhysicalSystemData CreateHomeworldPhysicalSystemData(Random rng)
		{
			PhysicalSystemData data = new PhysicalSystemData();

			// system attributes
			data.HasGasGiants = true;
			data.HasAsteroidBelts = true;
			SetSystemResourceAvailability(data, rng);

			// planet attributes
			SetRadius(data, rng);
			SetDensity(data, rng);
			SetGravity(data, rng);
			SetOceanCoverage(data, rng);
			SetPlanetaryResourceAvailability(data, rng);

			return data;
		}

		public static PhysicalSystemData CreatePhysicalSystemData(Random rng)
		{
			PhysicalSystemData data = new PhysicalSystemData();

			// system attributes
			SetHasGasGiants(data, rng);
			SetHasAsteroidBelts(data, rng);
			SetSystemResourceAvailability(data, rng);

			// planet attributes
			SetRadius(data, rng);
			SetDensity(data, rng);
			SetGravity(data, rng);
			SetOceanCoverage(data, rng);
			SetPlanetaryResourceAvailability(data, rng);

			return data;
		}

		private static void SetHasGasGiants(PhysicalSystemData data, Random rng)
		{
			data.HasGasGiants = rng.NextDouble() < 0.6;
		}

		private static void SetHasAsteroidBelts(PhysicalSystemData data, Random rng)
		{
			data.HasAsteroidBelts = rng.NextDouble() < 0.2;
		}

		private static void SetSystemResourceAvailability(PhysicalSystemData data, Random rng)
		{
			data.SystemResourceAvailability = new List<ResourceAvailability>
			{
				CreateCommonMetalsSystemAvailability(data, rng),
				CreateScarceMetalsSystemAvailability(data, rng),
				CreateFossilFuelsSystemAvailability(rng),
				CreateNuclearFuelsSystemAvailability(data, rng),
				CreateChemicalsSystemAvailability(data, rng),
				CreateBuildingMaterialsSystemAvailability(data, rng),
				CreateWaterSystemAvailability(data, rng),
				CreateSoilSystemAvailability(rng),
				CreateBiologicalSystemAvailability(rng),
			}.ToDictionary(x => x.ResourceKind);
		}

		private static ResourceAvailability CreateCommonMetalsSystemAvailability(PhysicalSystemData data, Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble();
			if (data.HasAsteroidBelts)
			{
				access += (1 - access) * 0.5;
				quantity *= 2;
			}
			return new ResourceAvailability(ResourceKind.CommonMetals)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateScarceMetalsSystemAvailability(PhysicalSystemData data, Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			if (data.HasAsteroidBelts)
			{
				access += (1 - access) * 0.5;
				quantity *= 1.5;
			}
			return new ResourceAvailability(ResourceKind.ScarceMetals)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateFossilFuelsSystemAvailability(Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			return new ResourceAvailability(ResourceKind.FossilFuels)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateNuclearFuelsSystemAvailability(PhysicalSystemData data, Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			if (data.HasAsteroidBelts)
			{
				access += (1 - access) * 0.3;
				quantity *= 1.3;
			}
			if (data.HasGasGiants)
			{
				access += (1 - access) * 0.5;
				quantity *= 1.5;
			}
			return new ResourceAvailability(ResourceKind.NuclearFuels)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateChemicalsSystemAvailability(PhysicalSystemData data, Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			if (data.HasAsteroidBelts)
			{
				access += (1 - access) * 0.2;
				quantity *= 1.2;
			}
			if (data.HasGasGiants)
			{
				access += (1 - access) * 0.5;
				quantity *= 1.5;
			}
			return new ResourceAvailability(ResourceKind.Chemicals)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateBuildingMaterialsSystemAvailability(PhysicalSystemData data, Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble();
			if (data.HasAsteroidBelts)
			{
				access += (1 - access) * 0.5;
				quantity *= 1.5;
			}
			return new ResourceAvailability(ResourceKind.BuildingMaterials)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateWaterSystemAvailability(PhysicalSystemData data, Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			if (data.HasAsteroidBelts)
			{
				access += (1 - access) * 0.1;
				quantity *= 1.1;
			}
			return new ResourceAvailability(ResourceKind.Water)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateSoilSystemAvailability(Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			return new ResourceAvailability(ResourceKind.Soil)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static ResourceAvailability CreateBiologicalSystemAvailability(Random rng)
		{
			double access = rng.NextDouble() * rng.NextDouble();
			double quantity = rng.NextDouble() * rng.NextDouble() * rng.NextDouble() * rng.NextDouble();
			return new ResourceAvailability(ResourceKind.Biological)
			{
				RawAccessibility = access,
				Quantity = quantity,
			};
		}

		private static void SetRadius(PhysicalSystemData data, Random rng)
		{
			data.Radius = rng.NextDouble();
		}

		private static void SetDensity(PhysicalSystemData data, Random rng)
		{
			data.Density = rng.NextDouble();
		}

		private static void SetGravity(PhysicalSystemData data, Random rng)
		{
			data.Gravity = rng.NextDouble();
		}

		private static void SetOceanCoverage(PhysicalSystemData data, Random rng)
		{
			data.OceanCoverage = rng.NextDouble();
		}

		private static void SetPlanetaryResourceAvailability(PhysicalSystemData data, Random rng)
		{
			data.SystemResourceAvailability = new List<ResourceAvailability>
			{
				CreateCommonMetalsSystemAvailability(data, rng),
				CreateScarceMetalsSystemAvailability(data, rng),
				CreateFossilFuelsSystemAvailability(rng),
				CreateNuclearFuelsSystemAvailability(data, rng),
				CreateChemicalsSystemAvailability(data, rng),
				CreateBuildingMaterialsSystemAvailability(data, rng),
				CreateWaterSystemAvailability(data, rng),
				CreateSoilSystemAvailability(rng),
				CreateBiologicalSystemAvailability(rng),
			}.ToDictionary(x => x.ResourceKind);
		}
	}
}
