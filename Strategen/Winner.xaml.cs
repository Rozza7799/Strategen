using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Strategen {
    /// <summary>
    /// Interaction logic for Winner.xaml
    /// </summary>
    public partial class Winner : Window {
        StrategyBase redStrat;
        StrategyBase blueStrat;
        float redHealth; 
        float blueHealth;
        bool winner;
        Match match;
        public Winner(StrategyBase redStrat, StrategyBase blueStrat, float redHealth, float blueHealth, bool winner, Match match) {
            InitializeComponent();
            this.redStrat= redStrat;
            this.blueStrat= blueStrat;
            this.redHealth= redHealth;
            this.blueHealth= blueHealth;
            this.match = match;
            RedHealth.Text = redHealth.ToString();
            BlueHealth.Text = blueHealth.ToString();
            RedStratAuthor.Text = redStrat.author;
            BlueStratAuthor.Text = blueStrat.author;
            RedStratName.Text = redStrat.name;
            BlueStratName.Text = blueStrat.name;
            this.winner = winner;
            
            WinningStrategy.Text = winner ? redStrat.name : blueStrat.name;
            WinningAuthor.Text = "Congratulations " + (winner ? redStrat.author : blueStrat.author);

        }
        private void ReturnClick(object sender, RoutedEventArgs e) {
            match.stratStarter.matchIsDone(winner);
            this.Close();
        }
    }
}
