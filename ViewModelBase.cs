using GoldenAnvil.Utility;

namespace FarTrader
{
	internal abstract class ViewModelBase : NotifyPropertyChangedBase
	{
		public AppModel AppModel
		{
			get { return AppModel.Current; }
		}
	}
}
