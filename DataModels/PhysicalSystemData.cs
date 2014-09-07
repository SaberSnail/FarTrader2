using System;
using System.Collections.Generic;
using System.Diagnostics;
using FarTrader.Hex;

namespace FarTrader.DataModels
{
	internal sealed class PhysicalSystemData
	{
		// System attributes
		public HexPoint Location
		{
			get
			{
				VerifyPropertySet(m_location);
				return m_location;
			}
			set
			{
				m_location = value;
			}
		}

		public bool HasGasGiants
		{
			get
			{
				VerifyPropertySet(m_hasGasGiants);
				return m_hasGasGiants.Value;
			}
			set
			{
				m_hasGasGiants = value;
			}
		}

		public bool HasAsteroidBelts
		{
			get
			{
				VerifyPropertySet(m_hasAsteroidBelts);
				return m_hasAsteroidBelts.Value;
			}
			set
			{
				m_hasAsteroidBelts = value;
			}
		}

		public Dictionary<ResourceKind, ResourceAvailability> SystemResourceAvailability
		{
			get
			{
				VerifyPropertySet(m_systemResourceAvailability);
				return m_systemResourceAvailability;
			}
			set
			{
				m_systemResourceAvailability = value;
			}
		}

		// Planet attributes
		public double Radius
		{
			get
			{
				VerifyPropertySet(m_radius);
				return m_radius.Value;
			}
			set
			{
				m_radius = value;
			}
		}

		public double Density
		{
			get
			{
				VerifyPropertySet(m_density);
				return m_density.Value;
			}
			set
			{
				m_density = value;
			}
		}

		public double Gravity
		{
			get
			{
				VerifyPropertySet(m_gravity);
				return m_gravity.Value;
			}
			set
			{
				m_gravity = value;
			}
		}

		public double OceanCoverage
		{
			get
			{
				VerifyPropertySet(m_oceanCoverage);
				return m_oceanCoverage.Value;
			}
			set
			{
				m_oceanCoverage = value;
			}
		}

		public Dictionary<ResourceKind, ResourceAvailability> PlanetaryResourceAvailability
		{
			get
			{
				VerifyPropertySet(m_planetaryResourceAvailability);
				return m_planetaryResourceAvailability;
			}
			set
			{
				m_planetaryResourceAvailability = value;
			}
		}

		[Conditional("DEBUG")]
		private static void VerifyPropertySet(object value)
		{
			if (value == null)
				throw new InvalidOperationException("Value may not be accessed before it is set.");
		}

		HexPoint m_location;
		bool? m_hasGasGiants;
		bool? m_hasAsteroidBelts;
		Dictionary<ResourceKind, ResourceAvailability> m_systemResourceAvailability;
		double? m_radius;
		double? m_density;
		double? m_gravity;
		double? m_oceanCoverage;
		Dictionary<ResourceKind, ResourceAvailability> m_planetaryResourceAvailability;
	}
}
