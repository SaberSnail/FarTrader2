using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FarTrader.DataModels;
using FarTrader.Hex;
using GoldenAnvil.Utility;
using Logos.Utility;

namespace FarTrader.Controls
{
	internal sealed class RenderConfig
	{
		public bool LabelEmptyHex { get; set; }
		public bool ShowTrueCoordinates { get; set; }
	}

	internal class StarMapView : Panel
	{
		public static readonly RenderConfig RenderConfig = new RenderConfig
		{
#if DEBUG
			LabelEmptyHex = false,
			ShowTrueCoordinates = false,
#endif
		};

		public StarMapView()
		{
			m_hitResults = new List<HexVisual>();
			m_visuals = (new List<DrawingVisual>()).AsReadOnly();
			m_pointToHexVisual = new Dictionary<HexPoint, HexVisual>();

			m_travelRouteVisual = new DrawingVisual();
			m_travelRouteVisual.Transform = new ScaleTransform(c_defaultScale, c_defaultScale);
		}

		public static readonly DependencyProperty StarMapProperty = DependencyPropertyUtility<StarMapView>.Register(x => x.StarMap, OnStarMapChanged);

		public StarMapViewModel StarMap
		{
			get { return (StarMapViewModel) GetValue(StarMapProperty); }
			set { SetValue(StarMapProperty, value); }
		}

		public static readonly DependencyProperty OverlayProperty = DependencyPropertyUtility<StarMapView>.Register(x => x.Overlay, OnOverlayChanged);

		public OverlayViewModel Overlay
		{
			get { return (OverlayViewModel) GetValue(OverlayProperty); }
			set { SetValue(OverlayProperty, value); }
		}

		public static readonly DependencyProperty HoveredSystemProperty = DependencyPropertyUtility<StarMapView>.Register(x => x.HoveredSystem, OnHoveredSystemChanged);

		public SystemData HoveredSystem
		{
			get { return (SystemData) GetValue(HoveredSystemProperty); }
			set { SetValue(HoveredSystemProperty, value); }
		}

		public static readonly DependencyProperty SelectedSystemProperty = DependencyPropertyUtility<StarMapView>.Register(x => x.SelectedSystem, OnSelectedSystemChanged, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender);

		public SystemData SelectedSystem
		{
			get { return (SystemData) GetValue(SelectedSystemProperty); }
			set { SetValue(SelectedSystemProperty, value); }
		}

		protected override int VisualChildrenCount
		{
			get { return m_visuals.Count; }
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= m_visuals.Count)
				throw new ArgumentOutOfRangeException("index");
			return m_visuals[index];
		}

		protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseMove(e);

			m_hitResults.Clear();

			Point point = e.GetPosition(this);
			VisualTreeHelper.HitTest(this, null, HitTestResult, new PointHitTestParameters(point));

			HoveredSystem = m_hitResults.Select(x => x.Data).FirstOrDefault();
		}

		protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);

			m_hitResults.Clear();

			Point point = e.GetPosition(this);
			VisualTreeHelper.HitTest(this, null, HitTestResult, new PointHitTestParameters(point));

			SelectedSystem = m_hitResults.Select(x => x.Data).FirstOrDefault();
		}

		protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			HoveredSystem = null;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Rect bounds = new Rect();
			foreach (DrawingVisual visual in m_visuals)
				bounds.Union(visual.Transform.TransformBounds(visual.ContentBounds));
			return bounds.Size;
		}

		private HitTestResultBehavior HitTestResult(HitTestResult result)
		{
			HexVisual hit = result.VisualHit as HexVisual;
			if (hit != null)
				m_hitResults.Add(hit);
			return HitTestResultBehavior.Continue;
		}

		private static void OnStarMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StarMapView view = (StarMapView) d;
			view.RenderMap();
			view.HoveredSystem = null;
		}

		static void OnOverlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StarMapView view = (StarMapView) d;
			view.RenderMap();
		}

		private static void OnSelectedSystemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StarMapView view = (StarMapView) d;
			view.UpdateTravelRoute();
		}

		private static void OnHoveredSystemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StarMapView view = (StarMapView) d;

			if (e.OldValue != null)
			{
				HexVisual oldVisual = view.m_pointToHexVisual.GetValueOrDefault(((SystemData) e.OldValue).Location);
				if (oldVisual != null)
					oldVisual.Opacity = 0.6;
			}
			if (e.NewValue != null)
			{
				HexVisual newVisual = view.m_pointToHexVisual.GetValueOrDefault(((SystemData) e.NewValue).Location);
				if (newVisual != null)
					newVisual.Opacity = 1.0;
			}

			view.UpdateTravelRoute();
		}

		private void RenderMap()
		{
			foreach (DrawingVisual visual in m_visuals)
			{
				RemoveVisualChild(visual);
				RemoveLogicalChild(visual);
			}

			List<DrawingVisual> visuals = new List<DrawingVisual>();

			visuals.AddRange(RenderHexNetwork(StarMap.CommunicationNetwork));
			List<HexVisual> systemVisuals = RenderSystems(StarMap.Systems).ToList();
			visuals.AddRange(systemVisuals);
			UpdateTravelRoute();
			visuals.Add(m_travelRouteVisual);
			//visuals.AddRange(RenderTestData());

			foreach (DrawingVisual visual in visuals)
			{
				AddVisualChild(visual);
				AddLogicalChild(visual);
			}
			m_visuals = visuals.AsReadOnly();
			m_pointToHexVisual = systemVisuals.ToDictionary(x => x.Data.Location);

			Rect bounds = new Rect();
			foreach (DrawingVisual visual in m_visuals)
				bounds.Union(visual.Transform.TransformBounds(visual.ContentBounds));
			RenderTransform = new TranslateTransform(-bounds.Left, -bounds.Top);

			InvalidateArrange();
		}

		private IEnumerable<DrawingVisual> RenderTestData()
		{
			Dictionary<int,int> results = new Dictionary<int, int>();
			for (int i = 0; i < 20000; i++)
			{
				//int result = (int) Math.Ceiling(StarMap.Rng.NextGauss() * 20);
				//int result = (int) (StarMap.Rng.NextWaitTime(50));
				double value = StarMap.Rng.NextDouble() * StarMap.Rng.NextDouble();
				int result = (int) (value * 100);
				if (results.ContainsKey(result))
					results[result]++;
				else
					results[result] = 1;
			}
			foreach (KeyValuePair<int, int> result in results)
			{
				DrawingVisual visual = new DrawingVisual();
				using (DrawingContext drawingContext = visual.RenderOpen())
					drawingContext.DrawRectangle(Brushes.Violet, null, new Rect(result.Key * 10, 0, 8, result.Value));
				visual.Transform = new ScaleTransform(c_defaultScale, c_defaultScale);
				yield return visual;
			}
		}

		private IEnumerable<DrawingVisual> RenderHexNetwork(HexNetwork network)
		{
			foreach (HexJump jump in network.Jumps.OrderByDescending(x => x.Point1))
			{
				DrawingVisual visual = new DrawingVisual();
				using (DrawingContext drawingContext = visual.RenderOpen())
				{
					Point center1 = HexPositionToOffset(jump.Point1) + (Vector) HexVisual.HexCenter;
					Point center2 = HexPositionToOffset(jump.Point2) + (Vector) HexVisual.HexCenter;
					Pen jumpPen = CreateJumpPen(center1, center2, s_communicationRouteGradient, 8.0);
					drawingContext.DrawLine(jumpPen, center1, center2);
				}
				visual.Transform = new ScaleTransform(c_defaultScale, c_defaultScale);
				yield return visual;
			}
		}

		private void RenderTravelRoute(HexRoute route)
		{
			using (DrawingContext drawingContext = m_travelRouteVisual.RenderOpen())
			{
				if (route != null)
				{
					Point? center1 = null;
					foreach (HexPoint point in route.Route)
					{
						Point center2 = HexPositionToOffset(point) + (Vector) HexVisual.HexCenter;
						if (center1 != null)
						{
							Pen jumpPen = CreateJumpPen(center1.Value, center2, s_travelRouteGradient, 6.0);
							drawingContext.DrawLine(jumpPen, center1.Value, center2);
						}
						center1 = center2;
					}
				}
			}
		}

		private IEnumerable<HexVisual> RenderSystems(IEnumerable<SystemData> systems)
		{
			foreach (SystemData data in systems)
			{
				HexVisual visual = data.IsEmpty ? new HexVisual(data) : new SystemVisual(data);

				Brush backgroundBrush = Overlay == null ? Brushes.Transparent : Overlay.GetBackgroundBrush(data);
				visual.Render(backgroundBrush);
				visual.Opacity = 0.6;

				double xOffset = HexPositionToXOffset(data.Location);
				double yOffset = HexPositionToYOffset(data.Location);
				TransformGroup transforms = new TransformGroup();
				transforms.Children.Add(new TranslateTransform(xOffset, yOffset));
				transforms.Children.Add(new ScaleTransform(c_defaultScale, c_defaultScale));
				visual.Transform = transforms;

				yield return visual;
			}
		}

		private void UpdateTravelRoute()
		{
			HexRoute route = null;
			SystemData selectedSystem = SelectedSystem;
			SystemData hoveredSystem = HoveredSystem;
			if (selectedSystem != null && !selectedSystem.IsEmpty && hoveredSystem != null && !hoveredSystem.IsEmpty)
				route = StarMap.CreateRoute(selectedSystem.Location, hoveredSystem.Location, 2);
			RenderTravelRoute(route);
			InvalidateVisual();
		}

		private static Pen CreateJumpPen(Point point1, Point point2, GradientStopCollection gradient, double width)
		{
			return (new Pen(new LinearGradientBrush(gradient)
			{
				MappingMode = BrushMappingMode.Absolute,
				StartPoint = point1,
				EndPoint = point2,
			}, width) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round }).Frozen();
		}

		private static double HexPositionToXOffset(HexPoint point)
		{
			return HexVisual.HexColumnOffset * point.X;
		}

		private static double HexPositionToYOffset(HexPoint point)
		{
			return -HexVisual.HexHeight * point.Y - HexVisual.HexHeight * point.X / 2;
		}

		private static Point HexPositionToOffset(HexPoint point)
		{
			return new Point(HexPositionToXOffset(point), HexPositionToYOffset(point));
		}

		const double c_defaultScale = 0.75;

		static readonly GradientStopCollection s_communicationRouteGradient = (new GradientStopCollection(new[]
			{
				new GradientStop(Color.FromArgb(255, 90, 0, 0), 0),
				new GradientStop(Color.FromArgb(40, 90, 0, 0), 0.5),
				new GradientStop(Color.FromArgb(255, 90, 0, 0), 1),
			})).Frozen();
		static readonly GradientStopCollection s_travelRouteGradient = (new GradientStopCollection(new[]
			{
				new GradientStop(Color.FromArgb(255, 0, 150, 150), 0),
				new GradientStop(Color.FromArgb(40, 0, 150, 150), 0.5),
				new GradientStop(Color.FromArgb(255, 0, 150, 150), 1),
			})).Frozen();

		readonly List<HexVisual> m_hitResults;
		ReadOnlyCollection<DrawingVisual> m_visuals;
		Dictionary<HexPoint, HexVisual> m_pointToHexVisual;
		readonly DrawingVisual m_travelRouteVisual;
	}
}
