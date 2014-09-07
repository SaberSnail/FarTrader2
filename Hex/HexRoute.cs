using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GoldenAnvil.Utility;

namespace FarTrader.Hex
{
	internal sealed class HexRoute
	{
		public HexRoute(int jumpRange, HexPoint start, double value)
		{
			m_jumpRange = jumpRange;
			m_route = new ReadOnlyCollection<HexPoint>(new[] { start });
			m_value = value;
		}

		public HexRoute(HexRoute that, HexPoint next, double addedValue)
		{
			m_jumpRange = that.m_jumpRange;
			m_route = that.m_route.Append(next).ToList().AsReadOnly();
			m_value = that.m_value + addedValue;
		}

		public int JumpRange
		{
			get { return m_jumpRange; }
		}

		public ReadOnlyCollection<HexPoint> Route
		{
			get { return m_route; }
		}

		public HexPoint StartPoint
		{
			get { return m_route.FirstOrDefault(); }
		}

		public HexPoint EndPoint
		{
			get { return m_route.LastOrDefault(); }
		}

		public double Value
		{
			get { return m_value; }
		}

		readonly int m_jumpRange;
		readonly ReadOnlyCollection<HexPoint> m_route;
		readonly double m_value;
	}
}
