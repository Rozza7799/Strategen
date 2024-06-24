using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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
using Path = System.IO.Path;

namespace Strategen {
    /// <summary>
    /// Interaction logic for StrategySelector.xaml
    /// </summary>
    public partial class TournamentManager {
        public Grid baseFileIcon;

        public StrategyBase redStrategy, blueStrategy;
        public bool redStrategyOpen = true, blueStrategyOpen = true;

        List<TournamentStrategy> strategies = new List<TournamentStrategy>();
        List<TextBlock> boxes = new List<TextBlock>(); //List containing all textboxes of the strategy names to be displayed.


        public TournamentManager() {
            InitializeComponent();

            boxes.Add(Strategy1); //0
            boxes.Add(Strategy2);
            boxes.Add(Strategy3);
            boxes.Add(Strategy4);
            boxes.Add(Strategy5);
            boxes.Add(Strategy6);
            boxes.Add(Strategy7);
            boxes.Add(Strategy8); //7


            boxes.Add(SF1Red); //8
            boxes.Add(SF1Blue);
            boxes.Add(SF2Blue);
            boxes.Add(SF2Red); //11

            boxes.Add(FRed); //12
            boxes.Add(FBlue); //13
        }

        public void AddStrategyFile(Grid file) {
            //file.Background = baseFileIcon.Background;
            //FileScrollPanel.Children.Add(file);
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect= true;
            ofd.ShowDialog();
            string[] filenames = ofd.FileNames;
            foreach (string fileName in filenames) {
                string fileContents = File.ReadAllText(fileName);


                int classNameIndex = 0; //Finds Starting index for the name of the class
                for (int i = 0; i < fileContents.Length - 6 && classNameIndex == 0; i++) {
                    if (fileContents.Substring(i, 6) == "class ") {
                        classNameIndex = i + 6;
                    }

                }

                string className = "";
                bool encounteredStart = false;
                bool encounteredEnd = false;
                int counter = 0;
                while (!encounteredEnd) {
                    if (fileContents[classNameIndex + counter].ToString() != " ") { //Gets the class name
                        encounteredStart = true;
                        className += fileContents[classNameIndex + counter].ToString();
                        counter++;
                    } else {
                        if (encounteredStart) { encounteredEnd = true; }
                    }

                }

                try {
                    strategies.Add(new TournamentStrategy(this, fileContents, className, false));
                } catch {
                    MessageBox.Show($"Strategy failed to load with classname = {className}. \nEnsure that the code loads properly in editor or try troubleshooting fixes.");
                }
            }
            MessageBox.Show($"Managed to load with {strategies.Count} strategies! \n Generating bracket");

            for (int i = strategies.Count; i < 8; i++) { //Fill in the empty spots
                strategies.Add(new TournamentStrategy(this, "", "", true));
            }
            for (int i = 0; i < 50; i++) { //Spice things up
                int a = new Random().Next(0, 8);
                int b = new Random().Next(0, 8);
                TournamentStrategy temp = strategies[a];
                strategies[a] = strategies[b];
                strategies[b] = temp;
            }
            for (int i = 0; i < 8; i++) { 
                boxes[i].Text = strategies[i].name;
            }
            

        }

        

        public void PlayMatch(object sender, RoutedEventArgs e) {
            if (!redStrategyOpen && !blueStrategyOpen) {
                Match m = new Match(redStrategy, blueStrategy, true, new TournamentStarter(this));
                m.Show();
                this.Hide();
            }

        }
    }

    public class TournamentStarter : MatchStarter {
        private TournamentManager t;

        public TournamentStarter(TournamentManager t) {
            this.t = t;
        }

        public override void matchIsDone(bool winner) {
            
        }

    }

    public class TournamentStrategy {
        private StrategyBase strategy;
        public string fileLocation;
        public string name = "Blank";
        public string status = "Unloaded";
        public string author = "Unknown";
        public string className = "Unknown";
        public TextBlock nameTextBlock;
        public TextBlock authorTextBlock;
        public Button removeButton;
        TournamentManager tournamentManager;
        public TournamentStrategy(TournamentManager tournamentManager, string fileContents, string className, bool blank) {
            if (blank) {
                strategy = new StrategyBase();
            } else {
                strategy = DynamicRuntimeCompiler.ExecuteCode(fileContents, className);
                strategy.OnStart();
                name = strategy.name;
            }
            status = "Loaded";
            this.className = className;
            author = strategy.author;
        }
    }
}
