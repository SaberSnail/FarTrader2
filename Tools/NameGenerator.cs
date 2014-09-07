using System;

namespace FarTrader.Tools
{
	// Aslan, K'kree, Vargr, Zhodani, Droyne
	interface INameGenerator
	{
		string GetName(Random rng);
	}
}
