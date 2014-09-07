using System;
using System.Windows;

namespace FarTrader
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	internal partial class App : Application
	{
		public App()
		{
			m_appModel = new AppModel();
			m_appModel.OnStartupFinished += AppModel_OnStartupFinished;
		}

		public AppModel AppModel
		{
			get { return m_appModel; }
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			m_appModel.Startup();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			m_appModel.Shutdown();

			base.OnExit(e);
		}

		private void AppModel_OnStartupFinished(object sender, EventArgs eventArgs)
		{
			Window window = new MainWindow(m_appModel.MainWindowViewModel);
			window.Show();
		}

		readonly AppModel m_appModel;
	}
}
