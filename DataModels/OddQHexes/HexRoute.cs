using System.Collections.Generic;
using System.Linq;

namespace FarTrader.DataModels
{
	internal sealed class HexRoute
	{
		public HexRoute(int jumpRange, HexPoint start, double value)
		{
			m_jumpRange = jumpRange;
			m_route = new List<HexPoint> { start };
			m_value = value;
		}

		public HexRoute(HexRoute that, HexPoint next, double addedValue)
		{
			m_jumpRange = that.m_jumpRange;
			m_route = new List<HexPoint>(that.m_route);
			m_route.Add(next);
			m_value = that.m_value + addedValue;
		}

		public int JumpRange
		{
			get { return m_jumpRange; }
		}

		public List<HexPoint> Route
		{
			get { return m_route; }
		}

		public HexPoint LastPoint
		{
			get { return m_route.Last(); }
		}

		public double Value
		{
			get { return m_value; }
		}

		readonly int m_jumpRange;
		readonly List<HexPoint> m_route;
		readonly double m_value;
	}
}
