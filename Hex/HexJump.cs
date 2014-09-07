using System;
using JetBrains.Annotations;
using Logos.Utility;

namespace FarTrader.Hex
{
	internal sealed class HexJump : IEquatable<HexJump>
	{
		public HexJump([NotNull] HexPoint point1, [NotNull] HexPoint point2)
			: this(point1, point2, point1.GetDistanceTo(point2))
		{
		}

		public HexJump([NotNull] HexPoint point1, [NotNull] HexPoint point2, int distance)
		{
			m_distance = distance;

			if (point1 < point2)
			{
				m_point1 = point1;
				m_point2 = point2;
			}
			else
			{
				m_point1 = point2;
				m_point2 = point1;
			}
		}

		public HexPoint Point1
		{
			get { return m_point1; }
		}

		public HexPoint Point2
		{
			get { return m_point2; }
		}

		public int Distance
		{
			get { return m_distance; }
		}

		public bool IncludesPoint(HexPoint point)
		{
			return m_point1 == point || m_point2 == point;
		}

		public HexPoint GetDestination(HexPoint point)
		{
			return point == m_point2 ? m_point2 :
				point == m_point1 ? m_point1 :
				null;
		}

		public override bool Equals(object that)
		{
			return Equals(that as HexJump);
		}

		public bool Equals(HexJump that)
		{
			return that != null && m_point1 == that.m_point1 && m_point2 == that.m_point2;
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(m_point1.GetHashCode(), m_point2.GetHashCode());
		}

		public static bool operator ==(HexJump left, HexJump right)
		{
			return ObjectImpl.OperatorEquality(left, right);
		}

		public static bool operator !=(HexJump left, HexJump right)
		{
			return ObjectImpl.OperatorInequality(left, right);
		}

		readonly HexPoint m_point1;
		readonly HexPoint m_point2;
		readonly int m_distance;
	}
}
