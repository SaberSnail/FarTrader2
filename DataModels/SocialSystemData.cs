using System;
using System.Diagnostics;

namespace FarTrader.DataModels
{
	internal sealed class SocialSystemData
	{
		public SocialSystemData()
		{
		}

		public SocialSystemData(SocialSystemData that)
		{
			m_administrativeRole = that.m_administrativeRole;
			m_capitolScore = that.m_capitolScore;
			m_isInterdicted = that.m_isInterdicted;
			m_name = that.m_name;
			m_population = that.m_population;
		}

		public string Name
		{
			get
			{
				VerifyPropertySet(m_name);
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public long Population
		{
			get
			{
				VerifyPropertySet(m_population);
				return m_population.Value;
			}
			set
			{
				m_population = value;
			}
		}

		public bool IsInterdicted
		{
			get
			{
				VerifyPropertySet(m_isInterdicted);
				return m_isInterdicted.Value;
			}
			set
			{
				m_isInterdicted = value;
			}
		}

		public double CapitolScore
		{
			get
			{
				VerifyPropertySet(m_capitolScore);
				return m_capitolScore.Value;
			}
			set
			{
				m_capitolScore = value;
			}
		}

		public AdministrativeRole AdministrativeRole
		{
			get
			{
				VerifyPropertySet(m_administrativeRole);
				return m_administrativeRole.Value;
			}
			set
			{
				m_administrativeRole = value;
			}
		}

		public SocialSystemData Clone()
		{
			return new SocialSystemData(this);
		}

		[Conditional("DEBUG")]
		private static void VerifyPropertySet(object value)
		{
			if (value == null)
				throw new InvalidOperationException("Value may not be accessed before it is set.");
		}

		string m_name;
		long? m_population;
		bool? m_isInterdicted;
		double? m_capitolScore;
		AdministrativeRole? m_administrativeRole;
	}
}
