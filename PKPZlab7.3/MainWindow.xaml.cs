using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PKPZlab7._3
{
    public partial class MainWindow : Window
    {
        private List<InternetSession> allSessions = new List<InternetSession>();
        private const string TimeFormat = "hh\\:mm\\:ss";

        public MainWindow()
        {
            InitializeComponent();
            dpSessionDate.SelectedDate = DateTime.Now;
        }

        private void btnAddSession_Click(object sender, RoutedEventArgs e)
        {
            if (!dpSessionDate.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtDataReceived.Text) ||
                string.IsNullOrWhiteSpace(txtDataSent.Text))
            {
                MessageBox.Show("Please fill all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime date = dpSessionDate.SelectedDate.Value.Date;

            if (!TimeSpan.TryParseExact(txtStartTime.Text, TimeFormat, CultureInfo.InvariantCulture, out TimeSpan startTime) ||
                !TimeSpan.TryParseExact(txtEndTime.Text, TimeFormat, CultureInfo.InvariantCulture, out TimeSpan endTime))
            {
                MessageBox.Show($"Invalid time format. Please use {TimeFormat}.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtDataReceived.Text, out double dataReceived) ||
                !double.TryParse(txtDataSent.Text, out double dataSent))
            {
                MessageBox.Show("Invalid data amount. Please enter numbers.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime sessionStart = date + startTime;
            DateTime sessionEnd = date + endTime;

            if (sessionEnd <= sessionStart)
            {
                sessionEnd = sessionEnd.AddDays(1);
            }

            InternetSession session = new InternetSession(
                txtLogin.Text,
                sessionStart,
                sessionEnd,
                dataReceived,
                dataSent
            );

            allSessions.Add(session);
            lstAllSessions.Items.Add(session);

            txtLogin.Clear();
            txtDataReceived.Clear();
            txtDataSent.Clear();
        }

        private void btnSearchRange_Click(object sender, RoutedEventArgs e)
        {
            lstSearchRangeResults.Items.Clear();
            if (!dpSearchStart.SelectedDate.HasValue || !dpSearchEnd.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both a start and end date.", "Date Error");
                return;
            }

            DateTime startDate = dpSearchStart.SelectedDate.Value.Date;
            DateTime endDate = dpSearchEnd.SelectedDate.Value.Date;
            double minTrafficMb = 20.0;

            var results = allSessions.Where(s =>
                s.OnlineDate >= startDate &&
                s.OnlineDate <= endDate &&
                s.TotalTrafficMb > minTrafficMb
            ).ToList();

            if (results.Count == 0)
            {
                lstSearchRangeResults.Items.Add("No matching sessions found.");
            }
            else
            {
                foreach (var session in results)
                {
                    lstSearchRangeResults.Items.Add(session);
                }
            }
        }

        private void btnCalcLastYear_Click(object sender, RoutedEventArgs e)
        {
            int month = (int)cmbMonth.SelectedItem;
            int lastYear = DateTime.Now.Year - 1;

            int count = allSessions.Count(s =>
                s.OnlineDate.Year == lastYear &&
                s.OnlineDate.Month == month
            );

            tbLastYearResult.Text = $"Sessions in {month}/{lastYear}: {count}";
        }

        private void btnFindMaxDuration_Click(object sender, RoutedEventArgs e)
        {
            if (!dpFindDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a date.", "Date Error");
                return;
            }

            DateTime date = dpFindDate.SelectedDate.Value.Date;
            var sessionsOnDate = allSessions.Where(s => s.OnlineDate == date).ToList();

            if (sessionsOnDate.Count == 0)
            {
                tbMaxDurationResult.Text = "No sessions found for this date.";
            }
            else
            {
                TimeSpan maxDuration = sessionsOnDate.Max(s => s.Duration);
                tbMaxDurationResult.Text = $"Max duration: {maxDuration:hh\\:mm\\:ss}";
            }
        }

        private void btnFindAllByDate_Click(object sender, RoutedEventArgs e)
        {
            lstFindAllByDateResults.Items.Clear();
            if (!dpFindDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a date.", "Date Error");
                return;
            }

            DateTime date = dpFindDate.SelectedDate.Value.Date;
            var sessionsOnDate = allSessions.Where(s => s.OnlineDate == date).ToList();

            if (sessionsOnDate.Count == 0)
            {
                lstFindAllByDateResults.Items.Add("No sessions found for this date.");
            }
            else
            {
                foreach (var session in sessionsOnDate)
                {
                    lstFindAllByDateResults.Items.Add(session);
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}