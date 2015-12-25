using System;
using System.Collections.Generic;
using Akka.Actor;
using AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Messages;
using OxyPlot;
using OxyPlot.Axes;
using LineSeries = OxyPlot.Series.LineSeries;
using log4net;

namespace AkkaPrismUnityDemo.Modules.Stocks.ActorModels.Actors.UI {

	internal sealed class LineChartingActor : ReceiveActor {

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly static ILog    _log    = LogManager.GetLogger( typeof( LineChartingActor ) );
		
		/// <summary>
		/// The chart model.
		/// </summary>
		private readonly PlotModel _chartModel;
		/// <summary>
		/// Our series.
		/// </summary>
		private readonly Dictionary<string, LineSeries> _series = new Dictionary<string, LineSeries>();

		/// <summary>
		/// The constructor.
		/// </summary>
		/// <param name="chartModel"></param>
		public LineChartingActor( PlotModel chartModel ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "LineChartingActor.ctor" );
			this._chartModel = chartModel;
			this.Receive( (Action<AddChartSeriesMessage>) this.AddSeriesToChart );
			this.Receive( (Action<RemoveChartSeriesMessage>) this.RemoveSeriesFromChart );
			this.Receive( (Action<StockPriceMessage>) this.HandleNewStockPrice );
		}

		/// <summary>
		/// Adds a series to the chart.
		/// </summary>
		/// <param name="message"></param>
		private void AddSeriesToChart( AddChartSeriesMessage message ) {
			if ( _log.IsDebugEnabled ) _log.DebugFormat( "AddSeriesToChart" );
			if ( this._series.ContainsKey( message.StockSymbol ) ) return;
			var series = new LineSeries {
				StrokeThickness = 2,
				MarkerType = MarkerType.None,
				CanTrackerInterpolatePoints = false,
				Title = message.StockSymbol,
				Smooth = false
			};
			this._series.Add( message.StockSymbol, series );
			this._chartModel.Series.Add( series );
			this.RefreshChart();
		}

		/// <summary>
		/// Refreshes the chart.
		/// </summary>
		private void RefreshChart() {
			this._chartModel.InvalidatePlot( true );
		}

		/// <summary>
		/// Called when a new stock price is received.
		/// </summary>
		/// <param name="message"></param>
		private void HandleNewStockPrice( StockPriceMessage message ) {
			if ( !this._series.ContainsKey( message.StockSymbol ) ) return;
			var series = this._series[message.StockSymbol];
			// Show at most 10 points
			if ( series.Points.Count > 10 ) {
				series.Points.RemoveAt( 0 );
			}
			series.Points.Add( new DataPoint( DateTimeAxis.ToDouble( message.Date ), Axis.ToDouble( message.Price ) ) );
			this.RefreshChart();
		}

		/// <summary>
		/// Removes a series from the chart.
		/// </summary>
		/// <param name="message"></param>
		private void RemoveSeriesFromChart( RemoveChartSeriesMessage message ) {
			if ( !this._series.ContainsKey( message.StockSymbol ) ) return;
			var series = this._series[message.StockSymbol];
			this._series.Remove( message.StockSymbol );
			this._chartModel.Series.Remove( series );
			this.RefreshChart();
		}

	}

}
