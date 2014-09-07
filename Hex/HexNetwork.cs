using System.Collections.Generic;
using System.Linq;
using GoldenAnvil.Utility;
using JetBrains.Annotations;

namespace FarTrader.Hex
{
	internal sealed class HexNetwork
	{
		public HexNetwork()
			: this(Enumerable.Empty<HexJump>())
		{ }

		public HexNetwork(HexJump jump)
			: this(new[] { jump })
		{ }

		public HexNetwork(IEnumerable<HexJump> jumps)
		{
			m_jumps = jumps.ToHashSet();
		}

		[NotNull]
		public IEnumerable<HexJump> Jumps
		{
			get { return m_jumps; }
		}

		public void AddJump([NotNull] HexJump jump)
		{
			m_jumps.Add(jump);
		}

		public void AddJumps([NotNull] IEnumerable<HexJump> jumps)
		{
			m_jumps.UnionWith(jumps);
		}

		public void AddRoute([NotNull] HexRoute route)
		{
			HexPoint point1 = null;
			foreach (HexPoint point2 in route.Route)
			{
				if (point1 != null)
					m_jumps.Add(new HexJump(point1, point2));
				point1 = point2;
			}
		}

		readonly HashSet<HexJump> m_jumps;
	}
}
