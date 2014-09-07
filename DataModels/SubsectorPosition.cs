using System;
using Logos.Utility;

namespace FarTrader.DataModels
{
	internal sealed class SubsectorPosition : IEquatable<SubsectorPosition>
	{
		public SubsectorPosition(SectorPosition sectorPosition, int x, int y)
		{
			m_sectorPosition = sectorPosition;
			m_x = x;
			m_y = y;
		}

		public SubsectorPosition(int sectorX, int sectorY, int subsectorX, int subsectorY)
		{
			m_sectorPosition = new SectorPosition(sectorX, sectorY);
			m_x = subsectorX;
			m_y = subsectorY;
		}

		public const int SubsectorWidth = 8;
		public const int SubsectorHeight = 10;

		public SectorPosition SectorPosition
		{
			get { return m_sectorPosition; }
		}

		public int X
		{
			get { return m_x; }
		}

		public int Y
		{
			get { return m_y; }
		}

		public override bool Equals(object that)
		{
			return Equals(that as SubsectorPosition);
		}

		public bool Equals(SubsectorPosition that)
		{
			return that != null && m_x == that.m_x && m_y == that.m_y && m_sectorPosition == that.m_sectorPosition;
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(m_x.GetHashCode(), m_y.GetHashCode(), m_sectorPosition.GetHashCode());
		}

		public static bool operator ==(SubsectorPosition left, SubsectorPosition right)
		{
			return ObjectImpl.OperatorEquality(left, right);
		}

		public static bool operator !=(SubsectorPosition left, SubsectorPosition right)
		{
			return ObjectImpl.OperatorInequality(left, right);
		}

		readonly SectorPosition m_sectorPosition;
		readonly int m_x;
		readonly int m_y;
	}
}
