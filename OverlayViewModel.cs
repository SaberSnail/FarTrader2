using System;
using System.Windows.Media;
using FarTrader.DataModels;
using GoldenAnvil.Utility;

namespace FarTrader
{
	internal sealed class OverlayViewModel
	{
		public OverlayViewModel(string label)
			: this(label, null, 0, 1)
		{
		}

		public OverlayViewModel(string label, Func<SystemData, double> valueFunction)
			: this(label, valueFunction, 0, 1)
		{
		}

		public OverlayViewModel(string label, Func<SystemData, double> valueFunction, double maxValue)
			: this(label, valueFunction, 0, maxValue)
		{
		}

		public OverlayViewModel(string label, Func<SystemData, double> valueFunction, double minValue, double maxValue)
		{
			m_label = label;
			m_valueFunction = valueFunction;
			m_minValue = minValue;
			m_maxValue = maxValue;
		}

		public string Label
		{
			get { return m_label; }
		}

		public Brush GetBackgroundBrush(SystemData data)
		{
			if (data.IsEmpty || m_valueFunction == null)
				return Brushes.Transparent;

			double value = m_valueFunction(data);
			value = MathUtility.Clamp((value - m_minValue) / (m_maxValue - m_minValue), 0, 1);

			//byte red = (byte) (m_minColor.R + (m_maxColor.R - m_minColor.R) * value);
			//byte green = (byte) (m_minColor.G + (m_maxColor.G - m_minColor.G) * value);
			//byte blue = (byte) (m_minColor.B + (m_maxColor.B - m_minColor.B) * value);
			//Brush brush = new SolidColorBrush(Color.FromArgb(255, red, green, blue));
			Brush brush = new SolidColorBrush(ColorUtility.FromAHSV(0x40, value * 120, 1, 1));

			return brush.Frozen();
		}

		readonly string m_label;
		readonly Func<SystemData, double> m_valueFunction;
		readonly double m_minValue;
		readonly double m_maxValue;
	}
}
