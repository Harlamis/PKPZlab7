using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PKPZlab7._2
{
    public partial class MainWindow : Window
    {
        private List<AEROFLOT> AIR = new List<AEROFLOT>();

        public MainWindow()
        {
            InitializeComponent();
            cmbPlaneType.ItemsSource = Enum.GetValues(typeof(Plane));
            cmbPlaneType.SelectedIndex = 0;
        }

        private void btnAddFlight_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Please, enter city destination", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtFlightNumber.Text, out int flightNum))
            {
                MessageBox.Show("Enterred wrong number", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string city = txtCity.Text;
            Plane planeType = (Plane)cmbPlaneType.SelectedItem;

            AEROFLOT newFlight = new AEROFLOT(city, flightNum, planeType);
            AIR.Add(newFlight);
            lstFlights.Items.Add(newFlight);

            txtCity.Clear();
            txtFlightNumber.Clear();
            cmbPlaneType.SelectedIndex = 0;
            txtCity.Focus();
        }

        private void btnSortAndSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchCity = txtSearchCity.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchCity))
            {
                MessageBox.Show("Please, enter a city to search", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            lstResults.Items.Clear();

            List<AEROFLOT> sortedList = AIR.OrderBy(flight => flight.NUM).ToList();

            AIR = sortedList;
            lstFlights.Items.Clear();
            foreach (var flight in AIR)
            {
                lstFlights.Items.Add(flight);
            }

            var foundFlights = AIR.Where(flight =>
                flight.CITY.Equals(searchCity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (foundFlights.Count == 0)
            {
                MessageBox.Show("No flights with that filter", "Search", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                foreach (var flight in foundFlights)
                {
                    lstResults.Items.Add(flight.ToString());
                }
            }
        }

        private void txtFlightNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}