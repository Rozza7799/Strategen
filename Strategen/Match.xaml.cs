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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace Strategen {
    /// <summary>
    /// Interaction logic for Match.xaml
    /// </summary>
    public partial class Match : Window {

        List<List<Image>> images = new List<List<Image>>();
        List<List<Rectangle>> damageDisplays = new List<List<Rectangle>>();
        
        BitmapImage emptyImage = new BitmapImage();
        BitmapImage redBarricadeImage = new BitmapImage();
        BitmapImage blueBarricadeImage = new BitmapImage();
        BitmapImage redKnightImage = new BitmapImage();
        BitmapImage blueKnightImage = new BitmapImage();
        BitmapImage redArcherImage = new BitmapImage();
        BitmapImage blueArcherImage = new BitmapImage();
        BitmapImage redCatapaultImage = new BitmapImage();
        BitmapImage blueCatapaultImage = new BitmapImage();
        BitmapImage redDragonImage = new BitmapImage();
        BitmapImage blueDragonImage = new BitmapImage();


        private DispatcherTimer timer = new DispatcherTimer();

        private Gameboard gameboard;

        private StrategyBase redStrategy;
        private StrategyBase blueStrategy;

        public bool winner;
        private bool withGUI;
        private bool finished = false;
        public MatchStarter stratStarter;

        public Match(StrategyBase red, StrategyBase blue, bool withGUI, MatchStarter stratStarter) {

            InitializeComponent();
            this.withGUI = withGUI;

            this.stratStarter = stratStarter;

            //Re-instancing it allows for two duplicate strategies to play against eachother
            redStrategy = red;//(StrategyBase) Activator.CreateInstance(red.GetType()); //This is fine because the strategy is already cast in the dynamic runtime compiler
            blueStrategy = blue;//(StrategyBase) Activator.CreateInstance(blue.GetType());
            redStrategy.OnStart();
            blueStrategy.OnStart();

            if (withGUI) { //If the user has slected to run the match with GUI using the checkbox
                timer.Interval = new TimeSpan(0, 0, 0, 0, 300);

                
                //Loading images into memory - massive pain
                emptyImage.BeginInit();
                emptyImage.UriSource = new Uri("Images/Units/Empty.png", UriKind.Relative);
                emptyImage.EndInit();

                redBarricadeImage.BeginInit();
                redBarricadeImage.UriSource = new Uri("Images/Units/RedBarricade.png", UriKind.Relative);
                redBarricadeImage.EndInit();

                blueBarricadeImage.BeginInit();
                blueBarricadeImage.UriSource = new Uri("Images/Units/BlueBarricade.png", UriKind.Relative);
                blueBarricadeImage.EndInit();

                redKnightImage.BeginInit();
                redKnightImage.UriSource = new Uri("Images/Units/RedKnight.png", UriKind.Relative);
                redKnightImage.EndInit();

                blueKnightImage.BeginInit();
                blueKnightImage.UriSource = new Uri("Images/Units/BlueKnight.png", UriKind.Relative);
                blueKnightImage.EndInit();

                redArcherImage.BeginInit();
                redArcherImage.UriSource = new Uri("Images/Units/RedWizard.png", UriKind.Relative);
                redArcherImage.EndInit();

                blueArcherImage.BeginInit();
                blueArcherImage.UriSource = new Uri("Images/Units/BlueWizard.png", UriKind.Relative);
                blueArcherImage.EndInit();

                redCatapaultImage.BeginInit();
                redCatapaultImage.UriSource = new Uri("Images/Units/RedBallista.png", UriKind.Relative);
                redCatapaultImage.EndInit();

                blueCatapaultImage.BeginInit();
                blueCatapaultImage.UriSource = new Uri("Images/Units/BlueBallista.png", UriKind.Relative);
                blueCatapaultImage.EndInit();

                redDragonImage.BeginInit();
                redDragonImage.UriSource = new Uri("Images/Units/RedDragon.png", UriKind.Relative);
                redDragonImage.EndInit();

                blueDragonImage.BeginInit();
                blueDragonImage.UriSource = new Uri("Images/Units/BlueDragon.png", UriKind.Relative);
                blueDragonImage.EndInit();
                Create(red, blue);

                gameboard = new Gameboard(this, red, blue);
                timer.Tick += Update;
                timer.Start();

            } else {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                Create(red, blue);

                gameboard = new Gameboard(this, red, blue);
                for (int i = 0; i < 199; i++) { Update(this, null); } //Runs 199 turns really damn fast
                timer.Tick += Update; 
                timer.Start(); //Lets the gameboard schedule the last update then naturally end after
            }
        }

        private void Update(object sender, EventArgs e) {

            if (withGUI && gameboard.GetTurn() > 50) {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            } else if (withGUI && gameboard.GetTurn() > 10) {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
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
            if (gameboard.GetTurn() >= 200) {
                EndMatch(gameboard.getHealth(true) > gameboard.getHealth(false));
            }
        }
        
        private void EndMatch(bool win) {
            timer.Stop();
            Winner winner = new Winner(redStrategy, blueStrategy, gameboard.getHealth(true), gameboard.getHealth(false), win, this);
            stratStarter.matchIsDone(win);
            winner.Show();
            this.winner = win;
        }

        public void Render(List<List<Unit>> units, List<TileDamage> damages, int turnNumber) {
            TxtTurnNumber.Text = turnNumber.ToString();

            if (withGUI) {

                List<List<float>> damagePerTile = new List<List<float>>();
                for (int x = 0; x < 16; x++) {
                    damagePerTile.Add(new List<float>());
                    for (int y = 0; y < 16; y++) {
                        damagePerTile[x].Add(0);
                    }
                }

                //An attempt to visualize the damage on the screen
                //Doesn't work due to the way that WPF draws images

                /*foreach (TileDamage damage in damages) { 
                    damagePerTile[damage.x][damage.y] += damage.damage;
                    damageDisplays[damage.x][damage.y].Fill = new SolidColorBrush(Color.FromArgb(50, (byte) damagePerTile[damage.x][damage.y], 0, 0));
                }
                damageDisplays[1][1].Fill = new SolidColorBrush(Color.FromArgb(50, 254, 0, 0));
                */



                //This looks like garbage but its the fastest method of redrawing the images
                for (int x = 0; x < 16; x++) {
                    for (int y = 0; y < 16; y++) {
                        if (units[x][y].GetUnitType() == UnitType.EMPTY) {
                            if (images[x][y].Source != emptyImage) { //this check is done to save computing time as redrawing an image takes more time than a comparison
                                images[x][y].Source = emptyImage;
                            }
                        } else if (units[x][y].GetUnitType() == UnitType.BARRICADE) {
                            if (units[x][y].getTeam()) {
                                if (images[x][y].Source != redBarricadeImage) {
                                    images[x][y].Source = redBarricadeImage;
                                }
                            } else {
                                if (images[x][y].Source != blueBarricadeImage) {
                                    images[x][y].Source = blueBarricadeImage;
                                }
                            }
                        } else if (units[x][y].GetUnitType() == UnitType.KNIGHT) {
                            if (units[x][y].getTeam()) {
                                if (images[x][y].Source != redKnightImage) {
                                    images[x][y].Source = redKnightImage;
                                }
                            } else {
                                if (images[x][y].Source != blueKnightImage) {
                                    images[x][y].Source = blueKnightImage;
                                }
                            } 
                        } else if (units[x][y].GetUnitType() == UnitType.ARCHER) {
                            if (units[x][y].getTeam()) {
                                if (images[x][y].Source != redArcherImage) {
                                    images[x][y].Source = redArcherImage;
                                }
                            } else {
                                if (images[x][y].Source != blueArcherImage) {
                                    images[x][y].Source = blueArcherImage;
                                }
                            }
                        } else if (units[x][y].GetUnitType() == UnitType.CATAPULT) {
                            if (units[x][y].getTeam()) {
                                if (images[x][y].Source != redCatapaultImage) {
                                    images[x][y].Source = redCatapaultImage;
                                }
                            } else {
                                if (images[x][y].Source != blueCatapaultImage) {
                                    images[x][y].Source = blueCatapaultImage;
                                }
                            }
                        } else if (units[x][y].GetUnitType() == UnitType.DRAGON) {
                            if (units[x][y].getTeam()) {
                                if (images[x][y].Source != redDragonImage) {
                                    images[x][y].Source = redDragonImage;
                                }
                            } else {
                                if (images[x][y].Source != blueDragonImage) {
                                    images[x][y].Source = blueDragonImage;
                                }
                            }
                        } 
                    }
                }
            }
        }

        public void Create(StrategyBase red, StrategyBase blue) { //An asynchronus constructor for the graphics so that I can instance the strategies in the other constructor
            RedStratName.Text = red.name;
            RedStratAuthor.Text = red.author;
            BlueStratName.Text = blue.name;
            BlueStratAuthor.Text = blue.author;
            if (withGUI) {
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

                for (int i = 0; i < 16; i++) {
                    DamageCanvas.RowDefinitions.Add(new RowDefinition());
                    DamageCanvas.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for (int x = 0; x < 16; x++) {

                    damageDisplays.Add(new List<Rectangle>());
                    for (int y = 0; y < 16; y++) {
                        damageDisplays[x].Add(new Rectangle());


                        Grid.SetRow(damageDisplays[x][y], x);
                        Grid.SetColumn(damageDisplays[x][y], y);

                        damageDisplays[x][y].Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                        damageDisplays[x][y].Name = $"DAMAGE_{x}_{y}";
                        DamageCanvas.Children.Add(damageDisplays[x][y]);
                    }
                }
            }
        }
    }
}