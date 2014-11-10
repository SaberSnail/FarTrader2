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
			PhysicalSystemData physicalData = RandomSystemDataUtility.CreateHomeworldPhysicalSystemData(rng);
			SocialSystemData socialData = RandomSystemDataUtility.CreateHomeworldSocialSystemData(physicalData, rng);

			return new SystemData(hexPoint, physicalData, socialData);
		}

		[NotNull]
		public static SystemData CreateRandom([NotNull] HexPoint hexPoint, [NotNull] Random rng)
		{
			PhysicalSystemData physicalData = RandomSystemDataUtility.CreatePhysicalSystemData(rng);
			SocialSystemData socialData = RandomSystemDataUtility.CreateSocialSystemData(physicalData, rng);

			return new SystemData(hexPoint, physicalData, socialData);
		}

		[NotNull]
		public SystemData CloneWithAdministrativeRole(AdministrativeRole role)
		{
			VerifyNotEmpty();
			SocialSystemData socialData = m_socialData.Clone();
			socialData.AdministrativeRole = role;
			return new SystemData(m_location, m_physicalData.Clone(), socialData);
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
				return m_socialData.Name;
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
				return m_socialData.Population;
			}
		}

		public bool IsInterdicted
		{
			get
			{
				VerifyNotEmpty();
				return m_socialData.IsInterdicted;
			}
		}

		public AdministrativeRole AdministrativeRole
		{
			get
			{
				VerifyNotEmpty();
				return m_socialData.AdministrativeRole;
			}
		}

		public double CapitolScore
		{
			get
			{
				VerifyNotEmpty();
				return m_socialData.CapitolScore;
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

		private SystemData(HexPoint hexPoint, PhysicalSystemData physicalData, SocialSystemData socialData)
		{
			m_location = hexPoint;
			m_physicalData = physicalData;
			m_socialData = socialData;
		}

		[Conditional("DEBUG")]
		private void VerifyNotEmpty()
		{
			if (m_isEmpty)
				throw new InvalidOperationException("Function may not be called on Empty system data.");
		}

		readonly bool m_isEmpty;
		readonly HexPoint m_location;
		readonly PhysicalSystemData m_physicalData;
		readonly SocialSystemData m_socialData;
	}
}
