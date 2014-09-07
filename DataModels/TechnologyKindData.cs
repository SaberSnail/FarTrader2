using System.Collections.ObjectModel;
using System.Linq;

namespace FarTrader.DataModels
{
	internal sealed class TechnologyKindData
	{
		public static readonly ReadOnlyDictionary<TechnologyKind, TechnologyKindData> Data = new ReadOnlyDictionary<TechnologyKind, TechnologyKindData>(new[]
		{
			new TechnologyKindData(TechnologyKind.Stone),
			new TechnologyKindData(TechnologyKind.Kingdom),
			new TechnologyKindData(TechnologyKind.Industrial),
			new TechnologyKindData(TechnologyKind.Scientific),
			new TechnologyKindData(TechnologyKind.Orbital),
			new TechnologyKindData(TechnologyKind.Stellar),
			new TechnologyKindData(TechnologyKind.Interstellar),
			new TechnologyKindData(TechnologyKind.StellarEmpire),
			new TechnologyKindData(TechnologyKind.Exploration),
			new TechnologyKindData(TechnologyKind.GalacticEmpire),
		}.ToDictionary(x => x.Kind)); 

		public TechnologyKind Kind
		{
			get { return m_kind; }
		}

		private TechnologyKindData(TechnologyKind kind)
		{
			m_kind = kind;
		}

		readonly TechnologyKind m_kind;
	}
}
