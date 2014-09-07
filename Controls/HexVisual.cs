using System.Globalization;
using System.Windows;
using System.Windows.Media;
using FarTrader.DataModels;
using GoldenAnvil.Utility;

namespace FarTrader.Controls
{
	internal class HexVisual : DrawingVisual
	{
		public const double HexHeight = 100.0;
		public const double HexFaceLength = 0.577350269 * HexHeight;
		public const double HexWidth = HexFaceLength * 2.0;
		public const double HexCornerOffset = HexFaceLength * MathUtility.Sin30;
		public const double HexColumnOffset = HexCornerOffset + HexFaceLength;
		public static readonly Point HexCenter = new Point(HexFaceLength, HexHeight / 2.0);

		public HexVisual(SystemData data)
		{
			m_data = data;
		}

		public SystemData Data
		{
			get { return m_data; }
		}

		public void Render(Brush backgroundBrush)
		{
			using (DrawingContext drawingContext = RenderOpen())
			{
				drawingContext.DrawGeometry(backgroundBrush, s_hexPen, s_hexGeometry);

				if (StarMapView.RenderConfig.LabelEmptyHex || !Data.IsEmpty)
				{
					string text = StarMapView.RenderConfig.ShowTrueCoordinates ? Data.Location.ToString() : Data.Location.DisplayLabel;
					FormattedText positionText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), c_hexPositionHeight, Brushes.White);
					drawingContext.DrawText(positionText, new Point(HexCenter.X - positionText.Width / 2.0, positionText.Height * 0.5));
				}

				//HexDirection dir = (new HexPoint(0, 0)).GetDirectionTo(Data.Location);
				//string direction = dir == HexDirection.Up ? "U" : dir == HexDirection.Down ? "D" : dir == HexDirection.UpLeft ? "UL" : dir == HexDirection.UpRight ? "UR" : dir == HexDirection.DownLeft ? "DL" : "DR";
				//FormattedText directionText = new FormattedText(direction, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), c_hexPositionHeight, Brushes.White);
				//drawingContext.DrawText(directionText, new Point(6, (HexHeight - directionText.Height) / 2));

				RenderCore(drawingContext);
			}
		}

		protected virtual void RenderCore(DrawingContext drawingContext)
		{ }

		private static Geometry CreateHexGeometry()
		{
			StreamGeometry hex = new StreamGeometry();
			using (StreamGeometryContext hexContext = hex.Open())
			{
				hexContext.BeginFigure(new Point(HexCornerOffset, HexHeight), true, true);
				hexContext.LineTo(new Point(HexColumnOffset, HexHeight), true, false);
				hexContext.LineTo(new Point(HexWidth, HexHeight / 2.0), true, false);
				hexContext.LineTo(new Point(HexColumnOffset, 0.0), true, false);
				hexContext.LineTo(new Point(HexCornerOffset, 0.0), true, false);
				hexContext.LineTo(new Point(0.0, HexHeight / 2.0), true, false);
			}
			hex.Freeze();
			return hex;
		}

		private static Geometry CreateHexTopBorder()
		{
			StreamGeometry hex = new StreamGeometry();
			using (StreamGeometryContext hexContext = hex.Open())
			{
				hexContext.BeginFigure(new Point(0.0, HexHeight / 2.0), false, false);
				hexContext.LineTo(new Point(HexCornerOffset, HexHeight), true, false);
				hexContext.LineTo(new Point(HexColumnOffset, HexHeight), true, false);
				hexContext.LineTo(new Point(HexWidth, HexHeight / 2.0), true, false);
			}
			hex.Freeze();
			return hex;
		}

		private static Geometry CreateHexBottomBorder()
		{
			StreamGeometry hex = new StreamGeometry();
			using (StreamGeometryContext hexContext = hex.Open())
			{
				hexContext.BeginFigure(new Point(HexWidth, HexHeight / 2.0), false, false);
				hexContext.LineTo(new Point(HexColumnOffset, 0.0), true, false);
				hexContext.LineTo(new Point(HexCornerOffset, 0.0), true, false);
				hexContext.LineTo(new Point(0.0, HexHeight / 2.0), true, false);
			}
			hex.Freeze();
			return hex;
		}

		const double c_hexPositionHeight = HexHeight * 0.10;

		static readonly Geometry s_hexGeometry = CreateHexGeometry();
		static readonly Geometry s_hexTopBorder = CreateHexTopBorder();
		static readonly Geometry s_hexBottomBorder = CreateHexBottomBorder();
		static readonly Pen s_hexPen = (new Pen(Brushes.White, 1.5)).Frozen();

		readonly SystemData m_data;
	}
}
