using System;
using AkkaPrismUnityDemo.Modules.Stocks.ViewModels;

namespace AkkaPrismUnityDemo.Modules.Stocks.Views {

	/// <summary>
	/// Interaction logic for StockChart.xaml
	/// </summary>
	public partial class StockChart {

		/// <summary>
		/// The constructor.
		/// </summary>
		public StockChart() : this( null ) {}

		/// <summary>
		/// The initialized constructor.
		/// </summary>
		/// <param name="viewModel"></param>
		public StockChart( StockChartViewModel viewModel ) {
			this.DataContext = viewModel;
			this.InitializeComponent();
		}

	}

}
