﻿using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Strategen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavToMatch(object sender, RoutedEventArgs e) {
            StrategySelector s = new StrategySelector();
            s.Show();
            this.Close();
        }

        private void NavToTournament(object sender, RoutedEventArgs e) {
            TournamentManager s = new TournamentManager();
            s.Show();
            this.Close();
        }
    }
}