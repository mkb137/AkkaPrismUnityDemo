using System;
using AkkaPrismUnityDemo.Modules.Stocks.ViewModels;

namespace AkkaPrismUnityDemo.Modules.Stocks.Views {

	/// <summary>
	/// Interaction logic for StockToggleButton.xaml
	/// </summary>
	public partial class StockToggleButton {

		/// <summary>
		/// The constructor.
		/// </summary>
		public StockToggleButton() : this( null ) {}

		/// <summary>
		/// The initialized constructor.
		/// </summary>
		/// <param name="viewModel"></param>
		public StockToggleButton( StockToggleButtonViewModel viewModel ) {
			this.DataContext = viewModel;
			this.InitializeComponent();
		}

	}

}
