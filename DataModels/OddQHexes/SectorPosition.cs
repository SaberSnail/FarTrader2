using System;
using Logos.Utility;

namespace FarTrader.DataModels
{
	internal sealed class SectorPosition : IEquatable<SectorPosition>
	{
		public SectorPosition(int x, int y)
		{
			m_x = x;
			m_y = y;
		}

		public const int SectorWidth = 4;
		public const int SectorHeight = 4;

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
			return Equals(that as SectorPosition);
		}

		public bool Equals(SectorPosition that)
		{
			return that != null && m_x == that.m_x && m_y == that.m_y;
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(m_x.GetHashCode(), m_y.GetHashCode());
		}

		public static bool operator ==(SectorPosition left, SectorPosition right)
		{
			return ObjectImpl.OperatorEquality(left, right);
		}

		public static bool operator !=(SectorPosition left, SectorPosition right)
		{
			return ObjectImpl.OperatorInequality(left, right);
		}

		readonly int m_x;
		readonly int m_y;
	}
}
