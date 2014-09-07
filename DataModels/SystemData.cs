using System;
using System.Diagnostics;
using FarTrader.Hex;
using FarTrader.Tools;
using JetBrains.Annotations;

namespace FarTrader.DataModels
{
	internal sealed class SystemData
	{
		[NotNull]
		public static SystemData CreateEmpty([NotNull] HexPoint hexPoint)
		{
			return new SystemData(hexPoint);
		}

		[NotNull]
		public static SystemData CreateHomeworld([NotNull] HexPoint hexPoint, [NotNull] Random rng)
		{
			string name = ImperialNameGenerator.Default.GetName(rng);
			PhysicalSystemData physicalData = RandomSystemDataUtility.CreateHomeworldPhysicalSystemData(rng);
			long population = RandomSystemDataUtility.GetPopulation(rng);
			bool isInterdicted = false;
			double capitolScore = SystemDataUtility.GetCapitolScore(population, isInterdicted) + 100;

			return new SystemData(hexPoint, name, physicalData, population, isInterdicted, AdministrativeRole.None, capitolScore);
		}

		[NotNull]
		public static SystemData CreateRandom([NotNull] HexPoint hexPoint, [NotNull] Random rng)
		{
			string name = ImperialNameGenerator.Default.GetName(rng);
			PhysicalSystemData physicalData = RandomSystemDataUtility.CreatePhysicalSystemData(rng);
			long population = RandomSystemDataUtility.GetPopulation(rng);
			bool isInterdicted = RandomSystemDataUtility.GetIsInterdicted(rng);
			double capitolScore = SystemDataUtility.GetCapitolScore(population, isInterdicted);

			return new SystemData(hexPoint, name, physicalData, population, isInterdicted, AdministrativeRole.None, capitolScore);
		}

		[NotNull]
		public SystemData CreateWithPoliticalRole(AdministrativeRole role)
		{
			VerifyNotEmpty();
			return new SystemData(m_location, m_name, m_physicalData, m_population, m_isInterdicted, role, m_capitolScore);
		}

		public bool IsEmpty
		{
			get { return m_isEmpty; }
		}

		[NotNull]
		public HexPoint Location
		{
			get { return m_location; }
		}

		[NotNull]
		public string Name
		{
			get
			{
				VerifyNotEmpty();
				return m_name;
			}
		}

		public bool HasGasGiants
		{
			get
			{
				VerifyNotEmpty();
				return m_physicalData.HasGasGiants;
			}
		}

		public bool HasAsteroidBelts
		{
			get
			{
				VerifyNotEmpty();
				return m_physicalData.HasAsteroidBelts;
			}
		}

		public long Population
		{
			get
			{
				VerifyNotEmpty();
				return m_population;
			}
		}

		public bool IsInterdicted
		{
			get
			{
				VerifyNotEmpty();
				return m_isInterdicted;
			}
		}

		public AdministrativeRole AdministrativeRole
		{
			get
			{
				VerifyNotEmpty();
				return m_administrativeRole;
			}
		}

		public double CapitolScore
		{
			get
			{
				VerifyNotEmpty();
				return m_capitolScore;
			}
		}

		public double GetResourceAvailability(ResourceKind resource)
		{
			VerifyNotEmpty();
			ResourceAvailability availability = m_physicalData.SystemResourceAvailability[resource];
			return availability.GetEffectiveAccessibility(TechnologyKind.StellarEmpire) * availability.Quantity;
		}

		private SystemData(HexPoint hexPoint)
		{
			m_isEmpty = true;
			m_location = hexPoint;
		}

		private SystemData(HexPoint hexPoint, string name, PhysicalSystemData physicalData, long population, bool isInterdicted, AdministrativeRole administrativeRole, double capitolScore)
		{
			m_location = hexPoint;
			m_name = name;
			m_physicalData = physicalData;
			m_population = population;
			m_isInterdicted = isInterdicted;
			m_administrativeRole = administrativeRole;
			m_capitolScore = capitolScore;
		}

		[Conditional("DEBUG")]
		private void VerifyNotEmpty()
		{
			if (m_isEmpty)
				throw new InvalidOperationException("Function may not be called on Empty system data.");
		}

		readonly bool m_isEmpty;
		readonly HexPoint m_location;
		readonly string m_name;
		readonly PhysicalSystemData m_physicalData;
		readonly long m_population;
		readonly bool m_isInterdicted;
		readonly double m_capitolScore;
		readonly AdministrativeRole m_administrativeRole;
	}

	internal sealed class SocialSystemData
	{
		public string Name { get; set; }

		public long Population { get; set; }

		public bool IsInterdicted { get; set; }

		public double CapitolScore { get; set; }

		public AdministrativeRole AdministrativeRole { get; set; }
	}
}
