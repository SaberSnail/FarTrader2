using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Logos.Utility;

namespace FarTrader.DataModels
{
	internal sealed class HexPoint : IEquatable<HexPoint>
	{
		public HexPoint(int absoluteX, int absoluteY)
		{
			const int sectorWidth = SectorPosition.SectorWidth * SubsectorPosition.SubsectorWidth;
			const int sectorHeight = SectorPosition.SectorHeight * SubsectorPosition.SubsectorHeight;
			if (absoluteX < 0)
				absoluteX -= sectorWidth;
			if (absoluteY < 0)
				absoluteY -= sectorHeight;
			SectorPosition sectorPosition = new SectorPosition(absoluteX / sectorWidth, absoluteY / sectorHeight);

			int x = absoluteX % sectorWidth;
			if (x < 0)
				x += sectorWidth;
			int y = absoluteY % sectorHeight;
			if (y < 0)
				y += sectorHeight;

			m_subsectorPosition = new SubsectorPosition(sectorPosition, x / SubsectorPosition.SubsectorWidth, y / SubsectorPosition.SubsectorHeight);
			m_x = x % SubsectorPosition.SubsectorWidth;
			m_y = y % SubsectorPosition.SubsectorHeight;
		}

		public HexPoint([NotNull] SubsectorPosition subsectorPosition, int x, int y)
		{
			m_subsectorPosition = subsectorPosition;
			m_x = x;
			m_y = y;
		}

		public HexPoint(int sectorX, int sectorY, int subsectorX, int subsectorY, int hexX, int hexY)
		{
			m_subsectorPosition = new SubsectorPosition(sectorX, sectorY, subsectorX, subsectorY);
			m_x = hexX;
			m_y = hexY;
		}

		[NotNull]
		public SubsectorPosition SubsectorPosition
		{
			get { return m_subsectorPosition; }
		}

		public int X
		{
			get { return m_x; }
		}

		public int Y
		{
			get { return m_y; }
		}

		public int AbsoluteX
		{
			get { return m_x + m_subsectorPosition.X * SubsectorPosition.SubsectorWidth + m_subsectorPosition.SectorPosition.X * SectorPosition.SectorWidth * SubsectorPosition.SubsectorWidth; }
		}

		public int AbsoluteY
		{
			get { return m_y + m_subsectorPosition.Y * SubsectorPosition.SubsectorHeight + m_subsectorPosition.SectorPosition.Y * SectorPosition.SectorHeight * SubsectorPosition.SubsectorHeight; }
		}

		public override bool Equals(object that)
		{
			return Equals(that as HexPoint);
		}

		public bool Equals(HexPoint that)
		{
			return that != null && m_x == that.m_x && m_y == that.m_y && m_subsectorPosition == that.m_subsectorPosition;
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(m_x.GetHashCode(), m_y.GetHashCode(), m_subsectorPosition.GetHashCode());
		}

		public static bool operator ==(HexPoint left, HexPoint right)
		{
			return ObjectImpl.OperatorEquality(left, right);
		}

		public static bool operator !=(HexPoint left, HexPoint right)
		{
			return ObjectImpl.OperatorInequality(left, right);
		}

		[NotNull]
		public string ToHexDecorationString()
		{
			return string.Format("{1:00},{0:00}", m_subsectorPosition.Y * SubsectorPosition.SubsectorHeight + m_y/* + 1*/, m_subsectorPosition.X * SubsectorPosition.SubsectorWidth + m_x/* + 1*/);
		}

		public int DistanceTo([NotNull] HexPoint that)
		{
			int x1 = AbsoluteX;
			int y2 = that.AbsoluteY;
			int dx = Math.Abs(that.AbsoluteX - x1);
			int lowY = 2 * AbsoluteY + (1 - dx % 2);
			if (x1 % 2 == 0)
				lowY--;
			else
				lowY += dx % 2;
			int highY = (lowY + dx) / 2;
			lowY = (lowY - dx) / 2;
			if (y2 < lowY)
				return dx + lowY - y2;
			else if (y2 > highY)
				return dx + y2 - highY;
			return dx;
		}

		public HexPoint NextPosition(HexDirection direction)
		{
			int x = AbsoluteX;
			int y = AbsoluteY;

			switch (direction)
			{
			case HexDirection.Up:
				return new HexPoint(x, y - 1);
			case HexDirection.UpRight:
				return new HexPoint(x + 1, y - (1 - m_x % 2));
			case HexDirection.DownRight:
				return new HexPoint(x + 1, y + (m_x % 2));
			case HexDirection.Down:
				return new HexPoint(x, y + 1);
			case HexDirection.DownLeft:
				return new HexPoint(x - 1, y + (m_x % 2));
			case HexDirection.UpLeft:
				return new HexPoint(x - 1, y - (1 - m_x % 2));
			default:
				throw new NotImplementedException(direction.ToString());
			}
		}

		public IEnumerable<HexPoint> PositionsInRadius(int radius)
		{
			HexPoint point = this;
			yield return point;

			for (int r = 1; r <= radius; r++)
			{
				point = point.NextPosition(HexDirection.DownLeft);
				foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
				{
					for (int i = 0; i < r; i++)
					{
						point = point.NextPosition(direction);
						yield return point;
					}
				}
			}
		}

		readonly SubsectorPosition m_subsectorPosition;
		readonly int m_x;
		readonly int m_y;
	}
}
