using System;
using System.Configuration;
using System.Windows;
using AkkaPrismUnityDemo.Modules.Stocks;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Prism.Modularity;
using Prism.Unity;
using Akka.Actor;
using Akka.DI.Unity;

namespace AkkaPrismUnityDemo {

	/// <summary>
	/// The Unity-based Prism bootstrapper.
	/// </summary>
	internal sealed class Bootstrapper : UnityBootstrapper {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger( typeof( Bootstrapper ) );

		/// <summary>
		/// See <see cref="UnityBootstrapper.ConfigureContainer" />
		/// </summary>
		protected override void ConfigureContainer() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "ConfigureContainer" );
			base.ConfigureContainer();
			// Configure from app.config
			this.ConfigureContainerFromAppConfig();
		}

		/// <summary>
		/// Configures the unity container from app.config.
		/// </summary>
		private void ConfigureContainerFromAppConfig() {
			var configurationSection = (UnityConfigurationSection) ConfigurationManager.GetSection( "unity" );
			configurationSection?.Configure( this.Container );
		}

		/// <summary>
		/// See <see cref="UnityBootstrapper.ConfigureModuleCatalog" />
		/// </summary>
		protected override void ConfigureModuleCatalog() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "ConfigureModuleCatalog" );
			base.ConfigureModuleCatalog();
			var moduleCatalog = (ModuleCatalog) this.ModuleCatalog;
			moduleCatalog.AddModule( typeof( StocksModule ) );
		}

		/// <summary>
		/// See <see cref="UnityBootstrapper.CreateShell" />
		/// </summary>
		/// <returns></returns>
		protected override DependencyObject CreateShell() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "CreateShell" );
			return this.Container.Resolve<Shell>();
		}

		protected override void InitializeModules() {
			// Create the actor system
			var actorSystem = ActorSystem.Create( "StockActorSystem" );
			// Create a dependency resolver
			var resolver = new UnityDependencyResolver( this.Container, actorSystem );
			// Register the actor system with the container
			this.Container.RegisterInstance( actorSystem );
			// Continue initializing modules
			base.InitializeModules();
		}

		/// <summary>
		/// See <see cref="UnityBootstrapper.InitializeShell" />
		/// </summary>
		protected override void InitializeShell() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "InitializeShell" );
			base.InitializeShell();
			Application.Current.MainWindow = (Window) this.Shell;
			Application.Current.MainWindow.Show();
		}

	}

}
