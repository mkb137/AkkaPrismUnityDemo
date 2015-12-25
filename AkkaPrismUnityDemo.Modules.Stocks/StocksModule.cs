using System;
using Akka.Actor;
using Akka.DI.Unity;
using AkkaPrismUnityDemo.Infrastructure;
using AkkaPrismUnityDemo.Modules.Stocks.ExternalServices;
using AkkaPrismUnityDemo.Modules.Stocks.ViewModels;
using AkkaPrismUnityDemo.Modules.Stocks.Views;
using log4net;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace AkkaPrismUnityDemo.Modules.Stocks {

	public class StocksModule : IModule {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger( typeof( StocksModule ) );

		/// <summary>
		/// The region manager.
		/// </summary>
		private readonly IRegionManager _regionManager;
		/// <summary>
		/// The unity container.
		/// </summary>
		private readonly IUnityContainer _unityContainer;

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="regionManager"></param>
		/// <param name="unityContainer"></param>
		public StocksModule( IRegionManager regionManager, IUnityContainer unityContainer ) {
			this._regionManager = regionManager;
			this._unityContainer = unityContainer;
		}

		/// <summary>
		/// See <see cref="IModule.Initialize" />
		/// </summary>
		public void Initialize() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "Initialize" );
			// Create the actor system
			var actorSystem = ActorSystem.Create( "StockActorSystem" );
			// Create a dependency resolver
			var resolver = new UnityDependencyResolver( this._unityContainer, actorSystem );
			// Register the actor system with the container
			this._unityContainer.RegisterInstance( actorSystem );
			// Register our random stock price service gateway to the interface
			this._unityContainer.RegisterInstance<IStockPriceServiceGateway>( new RandomStockPriceServiceGateway() );

			// Register our views
			this._unityContainer.RegisterTypeForNavigation<StockToggleButton>( StockToggleButtonViewModel.ViewName );
			this._regionManager.RegisterViewWithRegion( RegionNames.MainRegion, typeof( StockChart ) );
		}

	}

}
