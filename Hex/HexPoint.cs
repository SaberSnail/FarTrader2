using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Logos.Utility;

namespace FarTrader.Hex
{
	/// <summary>
	/// Hex coordinates using flat-topped hexes with an axial system where +x is upper-right and +y is up.
	/// </summary>
	internal sealed class HexPoint : IEquatable<HexPoint>, IComparable<HexPoint>
	{
		public HexPoint(int x, int y)
		{
			m_x = x;
			m_y = y;
		}

		public int X
		{
			get { return m_x; }
		}

		public int Y
		{
			get { return m_y; }
		}

		public int Z
		{
			get { return -m_x - m_y; }
		}

		public int DisplayRow
		{
			get { return m_y + (m_x + (m_x & 1)) / 2; }
		}

		public int DisplayColumn
		{
			get { return m_x; }
		}

		[NotNull]
		public string DisplayLabel
		{
			get { return string.Format("{0:00}, {1:00}", DisplayRow, DisplayColumn); }
		}

		public override bool Equals(object that)
		{
			return Equals(that as HexPoint);
		}

		public bool Equals(HexPoint that)
		{
			return that != null && m_x == that.m_x && m_y == that.m_y;
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(m_x.GetHashCode(), m_y.GetHashCode());
		}

		public static bool operator ==(HexPoint left, HexPoint right)
		{
			return ObjectImpl.OperatorEquality(left, right);
		}

		public static bool operator !=(HexPoint left, HexPoint right)
		{
			return ObjectImpl.OperatorInequality(left, right);
		}

		public int CompareTo(HexPoint that)
		{
			if (that == null)
				return 1;
			if (m_x < that.m_x)
				return -1;
			if (m_x > that.m_x)
				return 1;
			if (m_y < that.m_y)
				return -1;
			if (m_y > that.m_y)
				return 1;
			return 0;
		}

		public static bool operator <(HexPoint left, HexPoint right)
		{
			if (left == null)
				return right != null;
			return left.CompareTo(right) < 0;
		}

		public static bool operator >(HexPoint left, HexPoint right)
		{
			if (left == null)
				return false;
			return left.CompareTo(right) > 0;
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}", m_x, m_y);
		}

		public int GetDistanceTo([NotNull] HexPoint that)
		{
			return (Math.Abs(m_x - that.m_x) + Math.Abs(m_y - that.m_y) + Math.Abs(m_x + m_y - that.m_x - that.m_y)) / 2;
		}

		public HexDirection GetDirectionTo([NotNull] HexPoint that)
		{
			int dx = that.m_x - m_x;
			int dy = that.m_y - m_y;
			int dz = -dx - dy;
			int adx = Math.Abs(dx);
			int ady = Math.Abs(dy);
			int adz = Math.Abs(dz);
			int smallest = Math.Min(adx, Math.Min(ady, adz));

			if (dx > 0 && adz == smallest && adx != smallest)
				return HexDirection.DownRight;
			if (dx > 0 && ady == smallest)
				return HexDirection.UpRight;
			if (dx < 0 && ady == smallest && adz != smallest)
				return HexDirection.DownLeft;
			if (dx < 0 && adz == smallest && adx != smallest)
				return HexDirection.UpLeft;
			if (dy > 0 && adx == smallest)
				return HexDirection.Up;
			return HexDirection.Down;
		}

		public HexPoint GetNextPoint(HexDirection direction)
		{
			switch (direction)
			{
			case HexDirection.Up:
				return new HexPoint(m_x, m_y + 1);
			case HexDirection.UpRight:
				return new HexPoint(m_x + 1, m_y);
			case HexDirection.DownRight:
				return new HexPoint(m_x + 1, m_y - 1);
			case HexDirection.Down:
				return new HexPoint(m_x, m_y - 1);
			case HexDirection.DownLeft:
				return new HexPoint(m_x - 1, m_y);
			case HexDirection.UpLeft:
				return new HexPoint(m_x - 1, m_y + 1);
			default:
				throw new NotImplementedException(direction.ToString());
			}
		}

		public IEnumerable<HexPoint> GetPositionsInRadius(int radius)
		{
			HexPoint point = this;
			yield return point;

			for (int r = 1; r <= radius; r++)
			{
				point = point.GetNextPoint(HexDirection.DownLeft);
				foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
				{
					for (int i = 0; i < r; i++)
					{
						point = point.GetNextPoint(direction);
						yield return point;
					}
				}
			}
		}

		readonly int m_x;
		readonly int m_y;
	}
}
