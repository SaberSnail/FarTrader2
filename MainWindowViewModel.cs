using System.Collections.ObjectModel;
using System.Linq;
using FarTrader.DataModels;
using GoldenAnvil.Utility;

namespace FarTrader
{
	internal sealed class MainWindowViewModel : ViewModelBase
	{
		public MainWindowViewModel(AppModel app)
		{
			m_availableOverlays = new[]
			{
				new OverlayViewModel("<none>"),
				new OverlayViewModel("Gas Giants", data => data.HasGasGiants ? 1 : 0),
				new OverlayViewModel("Asteroid Belts", data => data.HasAsteroidBelts ? 1 : 0),
				new OverlayViewModel("Common Metals", data => data.GetResourceAvailability(ResourceKind.CommonMetals)),
				new OverlayViewModel("Scarce Metals", data => data.GetResourceAvailability(ResourceKind.ScarceMetals)),
				new OverlayViewModel("Fossil Fuels", data => data.GetResourceAvailability(ResourceKind.FossilFuels)),
				new OverlayViewModel("Nuclear Fuels", data => data.GetResourceAvailability(ResourceKind.NuclearFuels)),
				new OverlayViewModel("Chemicals", data => data.GetResourceAvailability(ResourceKind.Chemicals)),
				new OverlayViewModel("Building Materials", data => data.GetResourceAvailability(ResourceKind.BuildingMaterials)),
				new OverlayViewModel("Water", data => data.GetResourceAvailability(ResourceKind.Water)),
				new OverlayViewModel("Soil", data => data.GetResourceAvailability(ResourceKind.Soil)),
				new OverlayViewModel("Biological", data => data.GetResourceAvailability(ResourceKind.Biological)),
			}.ToList().AsReadOnly();
			m_selectedOverlay = m_availableOverlays[0];

			m_starMap = new StarMapViewModel(app.Random);
			m_selectedSystem = m_starMap.HomeWorld;
			//m_starfield = StarfieldUtility.CreateStarfieldImage(rng, new Size(100, 100));
			//m_starfield = StarfieldUtility.CreateAccidentalStarfield(rng, new Size(400, 400));
		}

		public StarMapViewModel StarMap
		{
			get { return m_starMap; }
		}

		public static readonly string SelectedSystemProperty = ReflectionUtility<MainWindowViewModel>.GetMemberName(x => x.SelectedSystem);

		public SystemData SelectedSystem
		{
			get { return m_selectedSystem; }
			set { SetPropertyField(SelectedSystemProperty, value, ref m_selectedSystem); }
		}

		public static readonly string HoveredSystemProperty = ReflectionUtility<MainWindowViewModel>.GetMemberName(x => x.HoveredSystem);

		public SystemData HoveredSystem
		{
			get { return m_hoveredSystem; }
			set { SetPropertyField(HoveredSystemProperty, value, ref m_hoveredSystem); }
		}

		public static readonly string SelectedOverlayProperty = ReflectionUtility<MainWindowViewModel>.GetMemberName(x => x.SelectedOverlay);

		public OverlayViewModel SelectedOverlay
		{
			get { return m_selectedOverlay; }
			set { SetPropertyField(SelectedOverlayProperty, value, ref m_selectedOverlay); }
		}

		public ReadOnlyCollection<OverlayViewModel> AvailableOverlays
		{
			get { return m_availableOverlays; }
		}

		//public ImageSource Starfield
		//{
		//	get { return m_starfield; }
		//}

		//public Rect StarfieldViewport
		//{
		//	get { return new Rect(0, 0, m_starfield.Width, m_starfield.Height); }
		//}

		//readonly ImageSource m_starfield;
		readonly StarMapViewModel m_starMap;
		readonly ReadOnlyCollection<OverlayViewModel> m_availableOverlays;

		SystemData m_selectedSystem;
		SystemData m_hoveredSystem;
		OverlayViewModel m_selectedOverlay;
	}
}
