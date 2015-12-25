using System;
using System.Windows;
using log4net;
using log4net.Config;

namespace AkkaPrismUnityDemo {

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger( typeof( App ) );

		/// <summary>
		/// See <see cref="Application.OnStartup" />
		/// </summary>
		/// <param name="args"></param>
		protected override void OnStartup( StartupEventArgs args ) {
			// Configure logging
			XmlConfigurator.Configure();
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "OnStartup" );
			// Continue
			base.OnStartup( args );
			var bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}

	}

}
