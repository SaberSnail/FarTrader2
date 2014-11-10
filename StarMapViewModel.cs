using System;
using System.Collections.Generic;
using System.Linq;
using FarTrader.DataModels;
using FarTrader.Hex;
using GoldenAnvil.Utility;
using JetBrains.Annotations;

namespace FarTrader
{
	internal sealed class StarMapViewModel : ViewModelBase
	{
		public StarMapViewModel(Random rng)
		{
			m_rng = rng;
			m_systems = CreateSystems();

			HexPoint startPoint = m_systems.Values.Where(x => !x.IsEmpty).First(x => x.AdministrativeRole == AdministrativeRole.Capitol).Location;
			IEnumerable<HexPoint> endPositions = m_systems.Values.Where(x => !x.IsEmpty && x.AdministrativeRole == AdministrativeRole.RegionalCapitol).Select(x => x.Location);
			m_communicationNetwork = CreateNetwork(startPoint, endPositions, 4);
		}

		public Random Rng
		{
			get { return m_rng; }
		}

		public SystemData HomeWorld
		{
			get { return m_systems.Values.First(x => x.AdministrativeRole == AdministrativeRole.Capitol); }
		}

		public IEnumerable<SystemData> Systems
		{
			get { return m_systems.Values; }
		}

		public HexNetwork CommunicationNetwork
		{
			get { return m_communicationNetwork; }
		}

		public IEnumerable<SystemData> GetSystemsInRadius([NotNull] HexPoint point, int radius)
		{
			return point.GetPositionsInRadius(radius)
				.Select(p =>
				{
					SystemData data;
					return m_systems.TryGetValue(p, out data) ? data : null;
				})
				.Where(x => x != null && !x.IsEmpty);
		}

		public IEnumerable<SystemData> GetReachableSystems([NotNull] HexPoint startPoint, int jumpRange)
		{
			HashSet<HexPoint> visited = new HashSet<HexPoint> { startPoint };
			List<HexPoint> endPoints = new List<HexPoint> { startPoint };

			SystemData data;
			if (m_systems.TryGetValue(startPoint, out data) && !data.IsEmpty)
				yield return data;

			while (endPoints.Count != 0)
			{
				List<HexPoint> newPoints = new List<HexPoint>();
				foreach (HexPoint endPoint in endPoints)
				{
					foreach (SystemData nextSystem in GetSystemsInRadius(endPoint, jumpRange))
					{
						if (visited.Contains(nextSystem.Location))
							continue;

						yield return nextSystem;
						visited.Add(nextSystem.Location);
						newPoints.Add(nextSystem.Location);
					}
				}

				endPoints = newPoints;
			}
		}

		public HexRoute CreateRoute(HexPoint start, HexPoint end, int jumpRange)
		{
			SystemData startData = m_systems[start];
			List<HexRoute> routes = new List<HexRoute> { new HexRoute(jumpRange, start, startData.CapitolScore) };
			HashSet<HexPoint> visited = new HashSet<HexPoint> { start };

			while (routes.Count > 0)
			{
				Dictionary<HexPoint, HexRoute> newRoutes = new Dictionary<HexPoint, HexRoute>();
				foreach (HexRoute route in routes)
				{
					IEnumerable<SystemData> nextSystems = GetSystemsInRadius(route.EndPoint, jumpRange)
						.Where(x => !visited.Contains(x.Location));
					foreach (SystemData nextSystem in nextSystems.Reverse())
					{
						HexRoute newRoute = new HexRoute(route, nextSystem.Location, nextSystem.CapitolScore);
						if (newRoutes.ContainsKey(nextSystem.Location))
						{
							if (newRoute.Value > newRoutes[nextSystem.Location].Value)
								newRoutes[nextSystem.Location] = newRoute;
						}
						else
						{
							newRoutes[nextSystem.Location] = newRoute;
						}
					}
				}

				foreach (HexRoute newRoute in newRoutes.Values)
				{
					if (newRoute.EndPoint == end)
						return newRoute;
					visited.Add(newRoute.EndPoint);
				}
				routes = newRoutes.Values.ToList();
			}

			return null;
		}

		private HexNetwork CreateNetwork(HexPoint startPoint, IEnumerable<HexPoint> endPoints, int jumpRange)
		{
			HexNetwork network = new HexNetwork();
			HashSet<HexPoint> endPointSet = endPoints.ToHashSet();

			List<HexRoute> routes = new List<HexRoute> { new HexRoute(jumpRange, startPoint, 1) };
			HashSet<HexPoint> visited = new HashSet<HexPoint> { startPoint };

			while (endPointSet.Count != 0 && routes.Count != 0)
			{
				Dictionary<HexPoint, HexRoute> newRoutes = new Dictionary<HexPoint, HexRoute>();
				foreach (HexRoute route in routes)
				{
					IEnumerable<SystemData> nextSystems = GetSystemsInRadius(route.EndPoint, jumpRange)
						.Where(x => !visited.Contains(x.Location));
					foreach (SystemData nextSystem in nextSystems.Reverse())
					{
						bool atEndPoint = endPointSet.Contains(nextSystem.Location);
						HexRoute newRoute = new HexRoute(route, nextSystem.Location, atEndPoint ? 1 : 0);
						if (newRoutes.ContainsKey(nextSystem.Location))
						{
							if (newRoute.Value > newRoutes[nextSystem.Location].Value)
								newRoutes[nextSystem.Location] = newRoute;
						}
						else
						{
							newRoutes[nextSystem.Location] = newRoute;
						}
					}
				}

				foreach (HexRoute newRoute in newRoutes.Values)
				{
					if (endPointSet.Contains(newRoute.EndPoint))
					{
						network.AddRoute(newRoute);
						endPointSet.Remove(newRoute.EndPoint);
					}
					visited.Add(newRoute.EndPoint);
				}
				routes = newRoutes.Values.ToList();
			}

			return network;
		}

		private Dictionary<HexPoint, SystemData> CreateSystems()
		{
			HexPoint center = new HexPoint(0, 0);
			Dictionary<HexPoint, SystemData> sectorData = new Dictionary<HexPoint, SystemData>();
			foreach (HexPoint point in center.GetPositionsInRadius(20))
			{
				if (point == center)
					sectorData.Add(point, SystemData.CreateHomeworld(point, m_rng));
				else if (m_rng.NextDouble() < c_systemChance)
					sectorData.Add(point, SystemData.CreateRandom(point, m_rng));
				else
					sectorData.Add(point, SystemData.CreateEmpty(point));
			}

			sectorData[center] = sectorData[center].CloneWithAdministrativeRole(AdministrativeRole.Capitol);

			List<SystemData> capitolCandidates = sectorData.Values.Where(x => !x.IsEmpty && x.Location != center && center.GetDistanceTo(x.Location) > 4).OrderByDescending(x => x.CapitolScore).ToList();
			foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
			{
				SystemData data = capitolCandidates.FirstOrDefault(x => center.GetDirectionTo(x.Location) == direction);
				if (data != null)
					sectorData[data.Location] = data.CloneWithAdministrativeRole(AdministrativeRole.RegionalCapitol);
			}

			return sectorData;
		}

		const double c_systemChance = 0.375;

		readonly Random m_rng;
		readonly Dictionary<HexPoint, SystemData> m_systems;
		readonly HexNetwork m_communicationNetwork;
	}
}
