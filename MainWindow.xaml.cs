using System;
using System.Windows;

namespace FarTrader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	internal partial class MainWindow : Window
	{
		public MainWindow(MainWindowViewModel viewModel)
		{
			MainWindowViewModel = viewModel;
			InitializeComponent();
		}

		public MainWindowViewModel MainWindowViewModel { get; set; }

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			StarMapScrollViewer.ScrollToHorizontalOffset((StarMapScrollViewer.ExtentWidth - StarMapScrollViewer.ViewportWidth) / 2);
			StarMapScrollViewer.ScrollToVerticalOffset((StarMapScrollViewer.ExtentHeight - StarMapScrollViewer.ViewportHeight) / 2);
		}
	}
}
