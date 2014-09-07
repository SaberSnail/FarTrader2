using System;

namespace FarTrader.DataModels
{
	[Flags]
	internal enum TravellerExtraBaseKind
	{
		None = 0,
		Naval = 0x1,
		Scout = 0x2,
		Military = 0x4,
		Research = 0x8,
	}
}
