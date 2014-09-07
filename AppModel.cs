using System;
using System.Windows;
using GoldenAnvil.Utility;
using Logos.Utility;
using Logos.Utility.Logging;

namespace FarTrader
{
	internal sealed class AppModel
	{
		public static AppModel Current
		{
			get
			{
				return ((App) Application.Current).AppModel;
			}
		}

		public AppModel()
		{
			LogManager.Initialize(x => new ConsoleLogger(x));
			m_rng = new Random();
		}

		public event EventHandler OnStartupFinished;

		public Random Random
		{
			get { return m_rng; }
		}

		public MainWindowViewModel MainWindowViewModel
		{
			get { return m_mainWindowViewModel; }
		}

		public void Startup()
		{
			m_mainWindowViewModel = new MainWindowViewModel(this);
			OnStartupFinished.Raise(this);
		}

		public void Shutdown()
		{
		}

		readonly Random m_rng;
		MainWindowViewModel m_mainWindowViewModel;
	}
}
