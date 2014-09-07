using System;

namespace FarTrader.DataModels
{
	[Flags]
	internal enum TravellerTradeClassKind
	{
		None = 0,
		Agricultural = 0x0001,
		Extreme = 0x0002,
		Industrial = 0x0004,
		NonAgricultural = 0x0008,
		NonIndustrial = 0x0010,
		Poor = 0x0020,
		Rich = 0x0040,
		AsteroidClass = 0x0080,
		BarrenWorld = 0x0100,
		DesertWorld = 0x0200,
		ExoticOcean = 0x0400,
		HighPopulation = 0x0800,
		IceCapped = 0x1000,
		LowPopulation = 0x2000,
		VacuumWorld = 0x4000,
		WaterWorld = 0x8000,
	}
}
