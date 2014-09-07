using System.Windows;
using System.Windows.Controls;
using FarTrader.DataModels;

namespace FarTrader.Controls
{
	internal sealed class SystemDataInfoTemplateSelector : DataTemplateSelector
	{
		public static readonly SystemDataInfoTemplateSelector Instance = new SystemDataInfoTemplateSelector();

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;
			if (element == null)
				return null;

			SystemData data = item as SystemData;
			if (data == null)
				return null;

			if (data.IsEmpty)
				return (DataTemplate) element.FindResource("HexInfoTemplate");
			return (DataTemplate) element.FindResource("SystemInfoTemplate");
		}
	}
}
