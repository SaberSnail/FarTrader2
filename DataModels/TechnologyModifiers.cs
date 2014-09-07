using System.Collections.Generic;
using System.Linq;

namespace FarTrader.DataModels
{
	internal sealed class TechnologyModifiers
	{
		public static TechnologyModifiers GetInstance(TechnologyKind technology)
		{
			return s_instances[technology];
		}

		public double GetModifiedResourceAccessibility(ResourceKind resourceKind, double availability)
		{
			return availability * m_resourceAccessibilityModifiers[resourceKind];
		}

		private TechnologyModifiers(TechnologyKind technology, Dictionary<ResourceKind, double> resourceAccessibilityModifiers)
		{
			m_technology = technology;
			m_resourceAccessibilityModifiers = resourceAccessibilityModifiers;
		}

		static readonly Dictionary<TechnologyKind, TechnologyModifiers> s_instances = new[]
		{
			new TechnologyModifiers(
				TechnologyKind.Stone,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 0.1 },
					{ ResourceKind.ScarceMetals, 0.0 },
					{ ResourceKind.FossilFuels, 0.1 },
					{ ResourceKind.NuclearFuels, 0.0 },
					{ ResourceKind.Chemicals, 0.0 },
					{ ResourceKind.BuildingMaterials, 0.5 },
					{ ResourceKind.Water, 0.1 },
					{ ResourceKind.Soil, 0.1 },
					{ ResourceKind.Biological, 0.0 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Kingdom,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 0.2 },
					{ ResourceKind.ScarceMetals, 0.1 },
					{ ResourceKind.FossilFuels, 0.2 },
					{ ResourceKind.NuclearFuels, 0.1 },
					{ ResourceKind.Chemicals, 0.1 },
					{ ResourceKind.BuildingMaterials, 0.9 },
					{ ResourceKind.Water, 0.5 },
					{ ResourceKind.Soil, 0.4 },
					{ ResourceKind.Biological, 0.1 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Industrial,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 0.5 },
					{ ResourceKind.ScarceMetals, 0.2 },
					{ ResourceKind.FossilFuels, 0.5 },
					{ ResourceKind.NuclearFuels, 0.2 },
					{ ResourceKind.Chemicals, 0.3 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 0.6 },
					{ ResourceKind.Soil, 0.7 },
					{ ResourceKind.Biological, 0.3 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Scientific,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 0.9 },
					{ ResourceKind.ScarceMetals, 0.4 },
					{ ResourceKind.FossilFuels, 0.9 },
					{ ResourceKind.NuclearFuels, 0.4 },
					{ ResourceKind.Chemicals, 0.7 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 0.9 },
					{ ResourceKind.Soil, 0.9 },
					{ ResourceKind.Biological, 0.7 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Orbital,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 1.0 },
					{ ResourceKind.ScarceMetals, 0.5 },
					{ ResourceKind.FossilFuels, 1.0 },
					{ ResourceKind.NuclearFuels, 0.5 },
					{ ResourceKind.Chemicals, 1.0 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 1.0 },
					{ ResourceKind.Soil, 1.0 },
					{ ResourceKind.Biological, 1.0 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Stellar,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 1.1 },
					{ ResourceKind.ScarceMetals, 0.6 },
					{ ResourceKind.FossilFuels, 1.1 },
					{ ResourceKind.NuclearFuels, 0.6 },
					{ ResourceKind.Chemicals, 1.1 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 1.0 },
					{ ResourceKind.Soil, 1.5 },
					{ ResourceKind.Biological, 1.1 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Interstellar,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 1.2 },
					{ ResourceKind.ScarceMetals, 0.7 },
					{ ResourceKind.FossilFuels, 1.2 },
					{ ResourceKind.NuclearFuels, 0.7 },
					{ ResourceKind.Chemicals, 1.2 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 1.0 },
					{ ResourceKind.Soil, 2.0 },
					{ ResourceKind.Biological, 1.2 },
				}),
			new TechnologyModifiers(
				TechnologyKind.StellarEmpire,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 1.3 },
					{ ResourceKind.ScarceMetals, 0.8 },
					{ ResourceKind.FossilFuels, 1.3 },
					{ ResourceKind.NuclearFuels, 0.8 },
					{ ResourceKind.Chemicals, 1.3 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 1.0 },
					{ ResourceKind.Soil, 2.0 },
					{ ResourceKind.Biological, 1.3 },
				}),
			new TechnologyModifiers(
				TechnologyKind.Exploration,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 1.4 },
					{ ResourceKind.ScarceMetals, 0.9 },
					{ ResourceKind.FossilFuels, 1.4 },
					{ ResourceKind.NuclearFuels, 0.9 },
					{ ResourceKind.Chemicals, 1.4 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 1.0 },
					{ ResourceKind.Soil, 2.0 },
					{ ResourceKind.Biological, 1.4 },
				}),
			new TechnologyModifiers(
				TechnologyKind.GalacticEmpire,
				new Dictionary<ResourceKind, double>
				{
					{ ResourceKind.CommonMetals, 1.5 },
					{ ResourceKind.ScarceMetals, 1.0 },
					{ ResourceKind.FossilFuels, 1.5 },
					{ ResourceKind.NuclearFuels, 1.0 },
					{ ResourceKind.Chemicals, 1.5 },
					{ ResourceKind.BuildingMaterials, 1.0 },
					{ ResourceKind.Water, 1.0 },
					{ ResourceKind.Soil, 2.0 },
					{ ResourceKind.Biological, 1.5 },
				}),
		}.ToDictionary(x => x.m_technology);

		readonly TechnologyKind m_technology;
		readonly Dictionary<ResourceKind, double> m_resourceAccessibilityModifiers;
	}
}
