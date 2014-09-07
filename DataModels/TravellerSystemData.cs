using System;
using FarTrader.Hex;
using FarTrader.Tools;

namespace FarTrader.DataModels
{
	internal sealed class TravellerSystemData
	{
		public static TravellerSystemData CreateEmpty(HexPoint hexPoint)
		{
			return new TravellerSystemData(hexPoint);
		}

		public static TravellerSystemData CreateRandom(Random rng, HexPoint hexPoint)
		{
			string allegiance = "Imperial";
			string name = ImperialNameGenerator.Default.GetName(rng);
			bool isSectorCapitol = false;
			bool isSubsectorCapitol = false;

			TravellerWorldKind worldKind;
			TravellerAtmosphereDensity atmosphereDensity = TravellerAtmosphereDensity.Standard;
			TravellerAtmosphereKind atmosphereKind;
			TravellerRandomSystemDataUtility.GetRandomWorldKind(rng, out worldKind, ref atmosphereDensity);
			TravellerRandomSystemDataUtility.GetRandomAtmosphere(rng, worldKind, out atmosphereKind, ref atmosphereDensity);

			int diameter;
			int densityModifier;
			TravellerRandomSystemDataUtility.GetRandomDiameter(rng, worldKind, atmosphereDensity, out diameter, out densityModifier);

			double density;
			TravellerRandomSystemDataUtility.GetRandomDensity(rng, worldKind, densityModifier, out density);

			double gravity = TravellerSystemDataUtility.CalculateGravity(density, diameter);

			int asteroidBeltsCount;
			TravellerRandomSystemDataUtility.GetRandomAsteroidBeltsCount(rng, worldKind, out asteroidBeltsCount);

			int gasGiantsCount;
			TravellerRandomSystemDataUtility.GetRandomGasGiantsCount(rng, out gasGiantsCount);

			double hydrographicCoverage;
			TravellerRandomSystemDataUtility.GetRandomHydrographicCoverage(rng, worldKind, out hydrographicCoverage);

			TravellerClimateKind climateKind;
			TravellerRandomSystemDataUtility.GetRandomClimateKind(rng, worldKind, out climateKind);

			TravellerResourceDensity resourceDensity;
			TravellerRandomSystemDataUtility.GetRandomResourceDensity(rng, worldKind, out resourceDensity);

			int habitability = TravellerSystemDataUtility.CalculateHabitability(worldKind, atmosphereKind, atmosphereDensity, hydrographicCoverage, climateKind);
			int affinity = TravellerSystemDataUtility.CalculateAffinity(habitability, resourceDensity);
			double maxPopulationRating = TravellerSystemDataUtility.CalculateMaxPopulation(atmosphereKind, atmosphereDensity, climateKind, diameter, hydrographicCoverage);

			double populationRating;
			long population;
			TravellerRandomSystemDataUtility.GetRandomPopulation(rng, resourceDensity, maxPopulationRating, out populationRating, out population);

			TravellerStarportClass starportClass;
			TravellerRandomSystemDataUtility.GetRandomStarportClass(rng, populationRating, out starportClass);

			TravellerExtraBaseKind extraBases;
			TravellerRandomSystemDataUtility.GetRandomExtraBases(rng, starportClass, populationRating, out extraBases);

			TravellerHazardZoneKind hazardZoneKind;
			TravellerRandomSystemDataUtility.GetRandomHazardZoneKind(rng, starportClass, out hazardZoneKind);

			TravellerGovernmentKind governmentKind;
			int controlRatingModifier;
			TravellerRandomSystemDataUtility.GetRandomGovernmentKind(rng, populationRating, out governmentKind, out controlRatingModifier);

			int controlRating;
			TravellerRandomSystemDataUtility.GetRandomControlRating(rng, populationRating, controlRatingModifier, out controlRating);

			int techLevel;
			TravellerRandomSystemDataUtility.GetRandomTechLevel(rng, populationRating, starportClass, diameter, worldKind, atmosphereDensity, hydrographicCoverage, governmentKind, out techLevel);

			TravellerTradeClassKind tradeClasses = TravellerSystemDataUtility.CalculateTradeClasses(atmosphereKind, atmosphereDensity, hydrographicCoverage, climateKind, populationRating, controlRating, worldKind, governmentKind);

			double worldTradeNumber = TravellerSystemDataUtility.CalculateWorldTradeNumber(populationRating, techLevel, starportClass);

			int perCapitaIncome = TravellerSystemDataUtility.CalculatePerCapitaIncome(populationRating, techLevel, tradeClasses);

			int pluralism;
			TravellerRandomSystemDataUtility.GetRandomPluralism(rng, populationRating, governmentKind, controlRating, out pluralism);

			int toleration;
			TravellerRandomSystemDataUtility.GetRandomToleration(rng, populationRating, pluralism, starportClass, hazardZoneKind, out toleration);

			int solidarity;
			TravellerRandomSystemDataUtility.GetRandomSolidarity(rng, populationRating, population, out solidarity);

			int tractability;
			TravellerRandomSystemDataUtility.GetRandomTractability(rng, populationRating, solidarity, governmentKind, out tractability);

			int aggression;
			TravellerRandomSystemDataUtility.GetRandomAggression(rng, populationRating, tractability, controlRating, out aggression);

			int pragmatism;
			TravellerRandomSystemDataUtility.GetRandomPragmatism(rng, populationRating, techLevel, out pragmatism);

			int innovation;
			TravellerRandomSystemDataUtility.GetRandomInnovation(rng, populationRating, pragmatism, toleration, out innovation);

			int providence;
			TravellerRandomSystemDataUtility.GetRandomProvidence(rng, populationRating, innovation, solidarity, out providence);

			int capitolScore = TravellerSystemDataUtility.CalculateCapitolScore(techLevel, starportClass, populationRating, hazardZoneKind, governmentKind);

			return new TravellerSystemData(hexPoint, name, worldKind, atmosphereDensity, atmosphereKind, asteroidBeltsCount, gasGiantsCount, diameter, density, gravity, hydrographicCoverage, climateKind, resourceDensity, habitability, affinity, maxPopulationRating, populationRating, population, starportClass, extraBases, hazardZoneKind, governmentKind, controlRating, techLevel, tradeClasses, worldTradeNumber, perCapitaIncome, pluralism, toleration, solidarity, tractability, aggression, pragmatism, innovation, providence, isSectorCapitol, isSubsectorCapitol, capitolScore, allegiance);
		}

		public TravellerSystemData ToSectorCapitol()
		{
			return new TravellerSystemData(m_hexPosition, m_name, m_worldKind, m_atmosphereDensity, m_atmosphereKind, m_asteroidBeltsCount, m_gasGiantsCount, m_diameter, m_density, m_gravity, m_hydrographicCoverage, m_climateKind, m_resourceDensity, m_habitability, m_affinity, m_maxPopulationRating, m_populationRating, m_population, m_starportClass, m_extraBases, m_hazardZoneKind, m_governmentKind, m_controlRating, m_techLevel, m_tradeClasses, m_worldTradeNumber, m_perCapitaIncome, m_pluralism, m_toleration, m_solidarity, m_tractability, m_aggression, m_pragmatism, m_innovation, m_providence, true, false, m_capitolScore, m_allegiance);
		}

		public TravellerSystemData ToSubsectorCapitol()
		{
			return new TravellerSystemData(m_hexPosition, m_name, m_worldKind, m_atmosphereDensity, m_atmosphereKind, m_asteroidBeltsCount, m_gasGiantsCount, m_diameter, m_density, m_gravity, m_hydrographicCoverage, m_climateKind, m_resourceDensity, m_habitability, m_affinity, m_maxPopulationRating, m_populationRating, m_population, m_starportClass, m_extraBases, m_hazardZoneKind, m_governmentKind, m_controlRating, m_techLevel, m_tradeClasses, m_worldTradeNumber, m_perCapitaIncome, m_pluralism, m_toleration, m_solidarity, m_tractability, m_aggression, m_pragmatism, m_innovation, m_providence, false, true, m_capitolScore, m_allegiance);
		}

		public bool IsEmpty
		{
			get { return m_isEmpty; }
		}

		public HexPoint Position
		{
			get { return m_hexPosition; }
		}

		public string Name
		{
			get { return m_name; }
		}

		public TravellerWorldKind WorldKind
		{
			get { return m_worldKind; }
		}

		public TravellerAtmosphereDensity AtmosphereDensity
		{
			get { return m_atmosphereDensity; }
		}

		public TravellerAtmosphereKind AtmosphereKind
		{
			get { return m_atmosphereKind; }
		}

		public int AsteroidBeltsCount
		{
			get { return m_asteroidBeltsCount; }
		}

		public int GasGiantsCount
		{
			get { return m_gasGiantsCount; }
		}

		public int Diameter
		{
			get { return m_diameter; }
		}

		public double Density
		{
			get { return m_density; }
		}

		public double Gravity
		{
			get { return m_gravity; }
		}

		public double HydrographicCoverage
		{
			get { return m_hydrographicCoverage; }
		}

		public TravellerClimateKind ClimateKind
		{
			get { return m_climateKind; }
		}

		public TravellerResourceDensity ResourceDensity
		{
			get { return m_resourceDensity; }
		}

		public int Habitability
		{
			get { return m_habitability; }
		}

		public int Affinity
		{
			get { return m_affinity; }
		}

		public double MaxPopulationRating
		{
			get { return m_maxPopulationRating; }
		}

		public double PopulationRating
		{
			get { return m_populationRating; }
		}

		public long Population
		{
			get { return m_population; }
		}

		public TravellerStarportClass StarportClass
		{
			get { return m_starportClass; }
		}

		public TravellerExtraBaseKind ExtraBases
		{
			get { return m_extraBases; }
		}

		public TravellerHazardZoneKind HazardZoneKind
		{
			get { return m_hazardZoneKind; }
		}

		public TravellerGovernmentKind GovernmentKind
		{
			get { return m_governmentKind; }
		}

		public int ControlRating
		{
			get { return m_controlRating; }
		}

		public int TechLevel
		{
			get { return m_techLevel; }
		}

		public TravellerTradeClassKind TradeClasses
		{
			get { return m_tradeClasses; }
		}

		public double WorldTradeNumber
		{
			get { return m_worldTradeNumber; }
		}

		public int PerCapitaIncome
		{
			get { return m_perCapitaIncome; }
		}

		public int Pluralism
		{
			get { return m_pluralism; }
		}

		public int Toleration
		{
			get { return m_toleration; }
		}

		public int Solidarity
		{
			get { return m_solidarity; }
		}

		public int Tractability
		{
			get { return m_tractability; }
		}

		public int Aggression
		{
			get { return m_aggression; }
		}

		public int Pragmatism
		{
			get { return m_pragmatism; }
		}

		public int Innovation
		{
			get { return m_innovation; }
		}

		public int Providence
		{
			get { return m_providence; }
		}

		public bool IsSectorCapitol
		{
			get { return m_isSectorCapitol; }
		}

		public bool IsSubsectorCapitol
		{
			get { return m_isSubsectorCapitol; }
		}

		public int CapitolScore
		{
			get { return m_capitolScore; }
		}

		public string Allegiance
		{
			get { return m_allegiance; }
		}

		private TravellerSystemData(HexPoint hexPoint, string name, TravellerWorldKind worldKind, TravellerAtmosphereDensity atmosphereDensity, TravellerAtmosphereKind atmosphereKind, int asteroidBeltsCount, int gasGiantsCount, int diameter, double density, double gravity, double hydrographicCoverage, TravellerClimateKind climateKind, TravellerResourceDensity resourceDensity, int habitability, int affinity, double maxPopulationRating, double populationRating, long population, TravellerStarportClass starportClass, TravellerExtraBaseKind extraBases, TravellerHazardZoneKind hazardZoneKind, TravellerGovernmentKind governmentKind, int controlRating, int techLevel, TravellerTradeClassKind tradeClasses, double worldTradeNumber, int perCapitaIncome, int pluralism, int toleration, int solidarity, int tractability, int aggression, int pragmatism, int innovation, int providence, bool isSectorCapitol, bool isSubsectorCapitol, int capitolScore, string allegiance)
		{
			m_hexPosition = hexPoint;
			m_name = name;
			m_worldKind = worldKind;
			m_atmosphereDensity = atmosphereDensity;
			m_atmosphereKind = atmosphereKind;
			m_asteroidBeltsCount = asteroidBeltsCount;
			m_gasGiantsCount = gasGiantsCount;
			m_diameter = diameter;
			m_density = density;
			m_gravity = gravity;
			m_hydrographicCoverage = hydrographicCoverage;
			m_climateKind = climateKind;
			m_resourceDensity = resourceDensity;
			m_habitability = habitability;
			m_affinity = affinity;
			m_maxPopulationRating = maxPopulationRating;
			m_populationRating = populationRating;
			m_population = population;
			m_starportClass = starportClass;
			m_extraBases = extraBases;
			m_hazardZoneKind = hazardZoneKind;
			m_governmentKind = governmentKind;
			m_controlRating = controlRating;
			m_techLevel = techLevel;
			m_tradeClasses = tradeClasses;
			m_worldTradeNumber = worldTradeNumber;
			m_perCapitaIncome = perCapitaIncome;
			m_pluralism = pluralism;
			m_toleration = toleration;
			m_solidarity = solidarity;
			m_tractability = tractability;
			m_aggression = aggression;
			m_pragmatism = pragmatism;
			m_innovation = innovation;
			m_providence = providence;
			m_isSectorCapitol = isSectorCapitol;
			m_isSubsectorCapitol = isSubsectorCapitol;
			m_capitolScore = capitolScore;
			m_allegiance = allegiance;
		}

		private TravellerSystemData(HexPoint hexPoint)
		{
			m_isEmpty = true;
			m_hexPosition = hexPoint;
		}

		readonly bool m_isEmpty;
		readonly HexPoint m_hexPosition;
		readonly string m_name;
		readonly TravellerWorldKind m_worldKind;
		readonly TravellerAtmosphereDensity m_atmosphereDensity;
		readonly TravellerAtmosphereKind m_atmosphereKind;
		readonly int m_asteroidBeltsCount;
		readonly int m_gasGiantsCount;
		readonly int m_diameter;
		readonly double m_density;
		readonly double m_gravity;
		readonly double m_hydrographicCoverage;
		readonly TravellerClimateKind m_climateKind;
		readonly TravellerResourceDensity m_resourceDensity;
		readonly int m_habitability;
		readonly int m_affinity;
		readonly double m_maxPopulationRating;
		readonly double m_populationRating;
		readonly long m_population;
		readonly TravellerStarportClass m_starportClass;
		readonly TravellerExtraBaseKind m_extraBases;
		readonly TravellerHazardZoneKind m_hazardZoneKind;
		readonly TravellerGovernmentKind m_governmentKind;
		readonly int m_controlRating;
		readonly int m_techLevel;
		readonly TravellerTradeClassKind m_tradeClasses;
		readonly double m_worldTradeNumber;
		readonly int m_perCapitaIncome;
		readonly int m_pluralism;
		readonly int m_toleration;
		readonly int m_solidarity;
		readonly int m_tractability;
		readonly int m_aggression;
		readonly int m_pragmatism;
		readonly int m_innovation;
		readonly int m_providence;
		readonly bool m_isSectorCapitol;
		readonly bool m_isSubsectorCapitol;
		readonly int m_capitolScore;
		readonly string m_allegiance;
	}
}
