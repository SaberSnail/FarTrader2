using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using FarTrader.DataModels;
using GoldenAnvil.Utility;

namespace FarTrader.Controls
{
	internal sealed class SystemVisual : HexVisual
	{
		public SystemVisual(SystemData data)
			: base(data)
		{
		}

		protected override void RenderCore(DrawingContext drawingContext)
		{
			Random rng = AppModel.Current.Random;
			Point center;

			if (Data.IsInterdicted)
			{
				Color color = s_interdictedColor;
				Brush hazardBrush = new RadialGradientBrush(color, new Color { A = 0, R = color.R, G = color.G, B = color.B });
				drawingContext.DrawEllipse(hazardBrush, null, HexCenter, c_hazardRadius, c_hazardRadius);
			}

			if (Data.HasAsteroidBelts)
			{
				drawingContext.DrawEllipse(null, s_asteroidBeltPen, HexCenter, c_asteroidBeltRadius, c_asteroidBeltRadius);
			}

			drawingContext.DrawEllipse(s_planetBrush, null, HexCenter, c_planetRadius, c_planetRadius);

			if (Data.HasGasGiants)
			{
				center = HexCenter + new Vector(c_gasGiantXOffset, c_gasGiantYOffset);
				drawingContext.DrawEllipse(Brushes.White, null, center, c_gasGiantRadius, c_gasGiantRadius);
			}

			if (Data.AdministrativeRole == AdministrativeRole.RegionalCapitol)
			{
				center = HexCenter + new Vector(-c_capitolRadius, c_capitolYOffset);
				drawingContext.PushTransform(new TranslateTransform(center.X, center.Y));
				drawingContext.DrawGeometry(Brushes.White, null, s_capitolGeometry);
				drawingContext.Pop();
			}
			else if (Data.AdministrativeRole == AdministrativeRole.Capitol)
			{
				center = HexCenter + new Vector(-c_capitolRadius, c_capitolYOffset);
				drawingContext.PushTransform(new TranslateTransform(center.X, center.Y));
				drawingContext.DrawGeometry(Brushes.Gold, null, s_capitolGeometry);
				drawingContext.Pop();
			}

			string name = Data.Name;
			if (Data.Population >= 1000000000)
				name = name.ToUpper(CultureInfo.CurrentCulture);
			FormattedText nameText = new FormattedText(name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), c_nameHeight, Brushes.White);
			nameText.MaxTextWidth = HexFaceLength;
			nameText.Trimming = TextTrimming.CharacterEllipsis;
			nameText.MaxLineCount = 1;
			drawingContext.DrawText(nameText, new Point(HexCenter.X - nameText.Width / 2.0, HexHeight - nameText.Height * 1.5));

			FormattedText positionText = new FormattedText(Data.Location.DisplayLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), c_hexPositionHeight, Brushes.White);
			drawingContext.DrawText(positionText, new Point(HexCenter.X - positionText.Width / 2.0, positionText.Height * 0.5));
		}

		private static Geometry CreateCapitolGeometry()
		{
			StreamGeometry geometry = new StreamGeometry();
			using (StreamGeometryContext geometryContext = geometry.Open())
			{
				double angle = Math.PI;
				const double delta = Math.PI * 4.0 / 5.0;
				geometryContext.BeginFigure(new Point(Math.Sin(angle) * c_capitolRadius + c_capitolRadius, Math.Cos(angle) * c_capitolRadius + c_capitolRadius), true, true);
				for (int i = 0; i < 4; i++)
				{
					angle += delta;
					if (angle > Math.PI * 2.0)
						angle -= Math.PI * 2.0;
					geometryContext.LineTo(new Point(Math.Sin(angle) * c_capitolRadius + c_capitolRadius, Math.Cos(angle) * c_capitolRadius + c_capitolRadius), true, false);
				}
			}
			geometry.FillRule = FillRule.Nonzero;
			geometry.Freeze();
			return geometry;
		}

		const double c_hexPositionHeight = HexHeight * 0.10;
		const double c_planetRadius = HexHeight * 0.1;
		const double c_asteroidBeltRadius = HexHeight * 0.14;
		const double c_nameHeight = HexHeight * 0.12;
		const double c_hazardRadius = HexHeight * 0.3;
		const double c_gasGiantRadius = HexHeight * 0.035;
		const double c_gasGiantXOffset = HexHeight * 0.325 * MathUtility.Cos30;
		const double c_gasGiantYOffset = HexHeight * -0.325 * MathUtility.Sin30;
		const double c_capitolYOffset = HexHeight * -0.3;
		const double c_capitolRadius = HexHeight * 0.06;
		const double c_asteroidBeltPenWidth = 2.5;
		const int c_asteroidBeltPenCount = 16;
		const double c_asteroidBeltDashWidth = 2.0 * Math.PI * c_asteroidBeltRadius / (2 * c_asteroidBeltPenCount * c_asteroidBeltPenWidth);

		static readonly Brush s_planetBrush = Brushes.White;
		static readonly Pen s_asteroidBeltPen = (new Pen(Brushes.DarkGray, c_asteroidBeltPenWidth) { DashCap = PenLineCap.Flat, DashStyle = new DashStyle(new[] { c_asteroidBeltDashWidth, c_asteroidBeltDashWidth }, 0) }).Frozen();
		static readonly Color s_interdictedColor = Color.FromRgb(200, 0, 0);
		static readonly Geometry s_capitolGeometry = CreateCapitolGeometry();
	}
}
