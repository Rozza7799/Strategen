using Strategen.Strategies;
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
using System.Windows.Threading;

namespace Strategen {
    /// <summary>
    /// Interaction logic for Match.xaml
    /// </summary>
    public partial class Match : Window {

        List<List<Image>> images = new List<List<Image>>();

        BitmapImage emptyImage = new BitmapImage();
        BitmapImage redCastleImage = new BitmapImage();
        BitmapImage blueCastleImage = new BitmapImage();

        DispatcherTimer timer = new DispatcherTimer();

        private Gameboard gameboard;

        public Match(StrategyBase red, StrategyBase blue) {

            InitializeComponent();

            timer.Tick += Update;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
            
            emptyImage.BeginInit();
            emptyImage.UriSource = new Uri("Images/Units/Empty.png", UriKind.Relative);
            emptyImage.EndInit();

            redCastleImage.BeginInit();
            redCastleImage.UriSource = new Uri("Images/Units/RedCastle.png", UriKind.Relative);
            redCastleImage.EndInit();

            blueCastleImage.BeginInit();
            blueCastleImage.UriSource = new Uri("Images/Units/BlueCastle.png", UriKind.Relative);
            blueCastleImage.EndInit();
            Create(red, blue);
            gameboard = new Gameboard(this, red, blue);
        }

        private void Update(object sender, EventArgs e) {

            if (gameboard.GetTurn() > 50) {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            } else if (gameboard.GetTurn() > 10) {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            }

            RedHealth.Text = gameboard.getHealth(true).ToString();
            RedHealthBar.Width = gameboard.getHealth(true);
            BlueHealth.Text = gameboard.getHealth(false).ToString();
            BlueHealthBar.Width = gameboard.getHealth(false);

            RedCores.Text = gameboard.GetCores(true).ToString();
            RedCoresBar.Width = gameboard.GetCores(true);
            BlueCores.Text = gameboard.GetCores(false).ToString();
            BlueCoresBar.Width = gameboard.GetCores(false);

            gameboard.Update();
        }

        public void Render(List<List<Unit>> units, int turnNumber) {
            TxtTurnNumber.Text = turnNumber.ToString();
            for (int  x = 0;  x < 16;  x++) {
                for (int y = 0; y < 16; y++) {
                    if (units[x][y].GetUnitType() == UnitType.EMPTY) {
                        if (images[x][y].Source != emptyImage) {
                            images[x][y].Source = emptyImage;
                        }
                    } else if (units[x][y].getTeam()) {
                        if (units[x][y].GetUnitType() == UnitType.WALL) {
                            images[x][y].Source = redCastleImage;
                        }
                    } else {
                        if (units[x][y].GetUnitType() == UnitType.WALL) {
                            images[x][y].Source = blueCastleImage;
                        }
                    }
                }
            }
        }

        public void Create(StrategyBase red, StrategyBase blue) {
            for (int i = 0; i < 16; i++) {
                GameCanvas.RowDefinitions.Add(new RowDefinition());
                GameCanvas.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int x = 0; x < 16; x++) {
                images.Add(new List<Image>());
                for (int y = 0; y < 16; y++) {
                    images[x].Add(new Image());

                    Grid.SetRow(images[x][y], x);
                    Grid.SetColumn(images[x][y], y);

                    images[x][y].Source = emptyImage;
                    images[x][y].Name = $"UNIT_{x}_{y}";
                    GameCanvas.Children.Add(images[x][y]);
                }
            }
        }
    }
}