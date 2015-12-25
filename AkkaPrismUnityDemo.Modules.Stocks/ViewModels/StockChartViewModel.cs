using System;
using Akka.Actor;
using AkkaPrismUnityDemo.Infrastructure;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors.UI;
using log4net;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Axes;
using Prism.Commands;
using Prism.Regions;

namespace AkkaPrismUnityDemo.Modules.Stocks.ViewModels {

	/// <summary>
	/// The view model for the stock chart page.
	/// </summary>
	public sealed class StockChartViewModel : ViewModel {

		/// <summary>
		/// The view name.
		/// </summary>
		public const string ViewName = "StockChart";
		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog _log = LogManager.GetLogger( typeof( StockChartViewModel ) );

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="regionManager"></param>
		/// <param name="unityContainer"></param>
		public StockChartViewModel( IRegionManager regionManager, IUnityContainer unityContainer ) : base( regionManager, unityContainer ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "StockChartViewModel.ctor" );
			this.ViewLoadedCommand = new DelegateCommand( this.ViewLoaded );
			// Set up the chart model
			this.SetUpChartModel();
			// Initialize the actors
			this.InitializeActors();
		}

		/// <summary>
		/// The charting actor.
		/// </summary>
		public IActorRef ChartingActorRef { get; private set; }

		/// <summary>
		/// THe plot model.
		/// </summary>
		public PlotModel PlotModel { get; private set; }

		/// <summary>
		/// The stocks coordinator actor.
		/// </summary>
		public IActorRef StocksCoordinatorActorRef { get; private set; }

		/// <summary>
		/// The command called when the view is loaded.
		/// </summary>
		public DelegateCommand ViewLoadedCommand { get; }

		/// <summary>
		/// Initialize the actors.
		/// </summary>
		private void InitializeActors() {
			this.ChartingActorRef =
				this.UnityContainer
					.Resolve<ActorSystem>()
					.ActorOf( Props.Create( () => new LineChartingActor( this.PlotModel ) ) );
			this.StocksCoordinatorActorRef =
				this.UnityContainer
					.Resolve<ActorSystem>()
					.ActorOf( Props.Create( () => new StocksCoordinatorActor( this.ChartingActorRef ) ), "StocksCoordinator" );
		}

		/// <summary>
		/// See <see cref="INavigationAware.OnNavigatedTo" />
		/// </summary>
		/// <param name="navigationContext"></param>
		public override void OnNavigatedTo( NavigationContext navigationContext ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "OnNavigatedTo" );
		}

		/// <summary>
		/// Sets up the chart model.
		/// </summary>
		private void SetUpChartModel() {
			this.PlotModel = new PlotModel {
				LegendTitle = "Legend",
				LegendOrientation = LegendOrientation.Horizontal,
				LegendPlacement = LegendPlacement.Outside,
				LegendPosition = LegendPosition.TopRight,
				Background = OxyColor.FromAColor( 200, OxyColors.White ),
				LegendBorder = OxyColors.Black
			};

			this.PlotModel.Axes.Add( new DateTimeAxis {
				Position = AxisPosition.Bottom,
				MajorGridlineStyle = LineStyle.Solid,
				MinorGridlineStyle = LineStyle.Dot,
				Title = "Date",
				StringFormat = "HH:mm:ss"
			} );

			this.PlotModel.Axes.Add( new LinearAxis {
				Minimum = 0,
				MajorGridlineStyle = LineStyle.Solid,
				MinorGridlineStyle = LineStyle.Dot,
				Title = "Price"
			} );
		}

		/// <summary>
		/// Shows a toggle button for the given stock.
		/// </summary>
		/// <param name="stockSymbol"></param>
		private void ShowStockToggleButton( string stockSymbol ) {
			this.RegionManager.RequestNavigate(
				RegionNames.ButtonsRegion,
				StockToggleButtonViewModel.ViewName,
				new NavigationParameters {
					{"StockSymbol", stockSymbol},
					{"StocksCoordinatorActor", this.StocksCoordinatorActorRef}
				}
				);
		}

		/// <summary>
		/// Called when the view is loaded.
		/// </summary>
		private void ViewLoaded() {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "ViewLoaded" );
			// Show the buttons
			this.ShowStockToggleButton( "AAA" );
			this.ShowStockToggleButton( "BBB" );
			this.ShowStockToggleButton( "CCC" );
		}

	}

}
