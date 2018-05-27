using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaesarCipher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private StringBuilder output = new StringBuilder();
		private Regex ShiftValidationRegex = new Regex("[^0-9]+");
		
		/// <summary>
		/// 
		/// </summary>
		public MainWindow() => InitializeComponent();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private async void EncodeBtnClick(object sender, RoutedEventArgs args)
		{
			if (String.IsNullOrEmpty(this.TextBox.Text))
			{
				MessageBox.Show("Input textbox is empty, nothing to encode!");
				return;
			}

			output.Clear();

			string[] text = this.TextBox.Text.Split('\n');
			int shift = Convert.ToInt32(this.ShiftValue.Text);

			await Task.Run(() => {

				foreach (string item in text)
				{
					CaesarShiftCipher.Instance.Encode(item, ref output, shift);
				}

			});

			this.TextBox.Text = output.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private async void DecodeBtnClick(object sender, RoutedEventArgs args)
		{

			if (String.IsNullOrEmpty(this.TextBox.Text))
			{
				MessageBox.Show("Input textbox is empty, nothing to decode!");
				return;
			}

			output.Clear();


			string[] text = this.TextBox.Text.Split('\n');
			int shift = Convert.ToInt32(this.ShiftValue.Text);

			await Task.Run(() => {

				foreach (string item in text)
				{
					CaesarShiftCipher.Instance.Decode(item, ref output, shift);
				}

			});

			this.TextBox.Text = output.ToString();

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private async void CrackBtnClick(object sender, RoutedEventArgs args)
		{

			if (String.IsNullOrEmpty(this.TextBox.Text))
			{
				MessageBox.Show("Input textbox is empty, nothing to hack!");
				return;
			}

			int crackedShift = 0;

			output.Clear();

			string[] text = this.TextBox.Text.Split('\n');

			await Task.Run(() => {

				foreach (string item in text)
				{
					CaesarShiftCipher.Instance.Crack(item, ref output, ref crackedShift);
				}

			});

			this.TextBox.Text = output.ToString();
			this.ShiftValue.Text = crackedShift.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ClearInputBtnClick(object sender, RoutedEventArgs args)
		{
			this.TextBox.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ExitBtnClick(object sender, RoutedEventArgs args)
		{
			Environment.Exit(0);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShiftValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			e.Handled = ShiftValidationRegex.IsMatch(e.Text);
		}

	}
}
