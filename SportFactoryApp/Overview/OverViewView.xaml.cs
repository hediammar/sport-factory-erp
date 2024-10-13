using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportFactoryApp
{
    public partial class OverviewView : UserControl
    {
        public ChartValues<ObservableValue> PeakUsageValues { get; set; }
        public Func<double, string> TimeFormatter { get; set; }

        public OverviewView()
        {
            InitializeComponent();

            // Sample data for the timeline chart
            PeakUsageValues = new ChartValues<ObservableValue>
            {/*
                new ObservableValue { DateTime.Now.AddHours(1).TimeOfDay.TotalHours, 20 },
                new ObservableValue { DateTime.Now.AddHours(2).TimeOfDay.TotalHours, 35 },
                new ObservableValue { DateTime.Now.AddHours(3).TimeOfDay.TotalHours, 25 }*/
            };

            TimeFormatter = value => TimeSpan.FromHours(value).ToString(@"hh\:mm");

            DataContext = this;

            // Load data and update stats (Placeholder values for demonstration)
            UpdateStats();
        }

        private void UpdateStats()
        {
            // Placeholder values
            TotalSessionsTodayTextBlock.Text = "120";
            TotalMembersTextBlock.Text = "50";
            ActiveMembersTextBlock.Text = "30";

            // Load session data
            var sessions = new List<Session>
            {
                new Session { MemberName = "John Doe", SessionDate = DateTime.Now, SessionNumber = 1 },
                new Session { MemberName = "Jane Smith", SessionDate = DateTime.Now, SessionNumber = 2 }
            };

            SessionsListView.ItemsSource = sessions;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement search functionality
        }

        public class Session
        {
            public string MemberName { get; set; }
            public DateTime SessionDate { get; set; }
            public int SessionNumber { get; set; }
        }

        public class ObservableValue
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
