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
    /// 
    public partial class StrategySelector {
        public Grid baseFileIcon;

        public StrategyBase redStrategy, blueStrategy;
        public bool redStrategyOpen = true, blueStrategyOpen = true;
        private Match m;

        public StrategySelector() {
            InitializeComponent();
            baseFileIcon = StrategyFileIcon;
            baseFileIcon.Visibility = Visibility.Hidden;

            //Loads saved strategies from the savedstrategies file
            string[] savedStrategies = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SavedStrategies.txt"));
            foreach (string strategy in savedStrategies) {
                string[] splitStrat = strategy.Split(',');
                if (splitStrat.Length == 2) {
                    string fileContents = File.ReadAllText(splitStrat[0]);
                    try {
                        new StrategyFile(this, fileContents, splitStrat[1]);
                    } catch {
                        MessageBox.Show(splitStrat[1] + " failed to load. \nEnsure that the code loads properly in editor or try troubleshooting fixes.");
                    }
                }
            }
        }

        public void AddStrategyFile(Grid file) {
            file.Background = baseFileIcon.Background;
            FileScrollPanel.Children.Add(file);
        }
        
        public void PlayMatch(object sender, RoutedEventArgs e) {
            if (!redStrategyOpen && !blueStrategyOpen) {
                m = new Match(redStrategy, blueStrategy, btnWithGUI.IsChecked.Value, new StrategySelecterStarter(this));
                m.Show();
                this.Hide();
                //while (!m.getFinished()) ;
                
            }
        }
        public void matchIsDone(bool winner) {
            this.Show();
            m.Close();
        }


        private void ImportBtn_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();
            try {
                string fileContents = File.ReadAllText(ofd.FileName);
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

                try { //Tries to make an instance of the strategy 
                    new StrategyFile(this, fileContents, className);
                    File.AppendAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SavedStrategies.txt"), (ofd.FileName + "," + className + "\n"));
                } catch {
                    MessageBox.Show("Strategy failed to load. \nEnsure that the code loads properly in editor or try troubleshooting fixes.");
                }
            } catch {
            }
        }

        public bool SelectStrategy(StrategyBase strategy) { //Whenever a strategy is selected from the list
            if (redStrategyOpen) {
                redStrategy = strategy;
                redStrategyTextBlock.Text = strategy.name;
                redStrategyOpen = false;
                return true;
            } else if (blueStrategyOpen) {
                blueStrategy = strategy;
                blueStrategyOpen = false;
                blueStrategyTextBlock.Text = strategy.name;
                PlayMatchBtn.Opacity = 1;
                return true;
            } else {
                return false;
            }
        }
        public void RemoveRedStrategy(object sender, RoutedEventArgs e) {
            PlayMatchBtn.Opacity = 0.5;
            redStrategyTextBlock.Text = "Empty";
            redStrategyOpen = true;
        }
        public void RemoveBlueStrategy(object sender, RoutedEventArgs e) {
            PlayMatchBtn.Opacity = 0.5;
            blueStrategyTextBlock.Text = "Empty";
            blueStrategyOpen = true;
        }
    }

    public class StrategySelecterStarter : MatchStarter {
        StrategySelector s;
        public StrategySelecterStarter(StrategySelector s) {
            this.s = s;
        }

        public override void matchIsDone(bool winner) {
            s.matchIsDone(winner);
        }
    }

    public class StrategyFile {
        Grid fileview;
        private StrategyBase strategy;
        public string fileLocation;
        public string name = "Unknown";
        public string status = "Unloaded";
        public string author = "Unknown";
        public string className = "Unknown";
        public TextBlock nameTextBlock;
        public TextBlock authorTextBlock;
        public Button removeButton;
        StrategySelector strategySelector;
        public StrategyFile(StrategySelector strategySelector, string fileContents, string className) {
            strategy = DynamicRuntimeCompiler.ExecuteCode(fileContents, className);
            strategy.OnStart();
            this.strategySelector = strategySelector;
            fileview = new Grid() {Height = 80};
            fileview.MouseDown += AttemptSelect;
            name = strategy.name;
            status = "Loaded";
            this.className = className;
            author = strategy.author;

            //Dynamically making WPF components is a massive pain
            fileview.RowDefinitions.Add(new RowDefinition());
            fileview.RowDefinitions.Add(new RowDefinition());
            fileview.ColumnDefinitions.Add(new ColumnDefinition());
            fileview.ColumnDefinitions.Add(new ColumnDefinition());
            fileview.Name = name + "FileView";

            nameTextBlock = new TextBlock() { Name = name + "NameTextBlock", Text = name, FontSize = 24, Margin = new Thickness(15, 0, 15, 0) };
            Grid.SetRow(nameTextBlock, 0);
            Grid.SetColumnSpan(nameTextBlock, 2);
            authorTextBlock = new TextBlock() { Name = name + "AuthorTextBlock", Text = author, FontSize = 15, Margin = new Thickness(15, 0, 15, 0) };
            Grid.SetRow(authorTextBlock, 1);
            
            removeButton = new Button() { Width = 40, HorizontalAlignment=HorizontalAlignment.Right };
            Grid.SetRow(removeButton, 1);
            Grid.SetColumn(removeButton, 1);
            removeButton.Click += DeleteStrategy; //Attaching an interrupt

            fileview.Children.Add(nameTextBlock);
            fileview.Children.Add(authorTextBlock);
            fileview.Children.Add(removeButton);
            strategySelector.AddStrategyFile(fileview);
        }

        private void DeleteStrategy(object sender, RoutedEventArgs e) {
            try { //Removes from the table and from the savedstrategyfile
                List<string> newFile = new List<string>();
                string[] savedStrategies = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SavedStrategies.txt"));
                foreach (string strategy in savedStrategies) {
                    string[] splitStrat = strategy.Split(',');
                    if (splitStrat.Length == 2) {
                        if (splitStrat[1] == className) {
                            MessageBox.Show(className + " was removed from saved files!");
                        } else {
                            newFile.Add(splitStrat[0] + "," + splitStrat[1]);
                        }
                    }
                }

                File.WriteAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"SavedStrategies.txt"), newFile);
            } catch {
                MessageBox.Show("Couldn't remove from saved files, likely because SavedStrategies.txt was unreachable");
            }

            strategySelector.FileScrollPanel.Children.Remove(fileview);
        }
        private void AttemptSelect(object sender, RoutedEventArgs e) {
            if (strategySelector.SelectStrategy(strategy)) {
                
            }
        }
    }
}
