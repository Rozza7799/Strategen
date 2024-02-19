using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Strategen {
    /// <summary>
    /// Interaction logic for StrategySelector.xaml
    /// </summary>
    public partial class StrategySelector {
        public Grid baseFileIcon;

        public StrategyBase redStrategy, blueStrategy;
        public bool redStrategyOpen = true, blueStrategyOpen = true;


        public StrategySelector() {
            InitializeComponent();
            baseFileIcon = StrategyFileIcon;
            baseFileIcon.Visibility = Visibility.Hidden;
        }

        public void AddStrategyFile(Grid file) {
            file.Background = baseFileIcon.Background;
            FileScrollPanel.Children.Add(file);
        }

        public void PlayMatch(object sender, RoutedEventArgs e) {
            if (!redStrategyOpen && !blueStrategyOpen) {
                Match m = new Match(redStrategy, blueStrategy);
                m.Show();
                this.Close();
            }
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();

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

            try {
                new StrategyFile(this, fileContents, className);
            } catch {
                MessageBox.Show("Strategy failed to load. \nEnsure that the code loads properly in editor or try troubleshooting fixes.");
            }
        }

        public bool SelectStrategy(StrategyBase strategy) {
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
            blueStrategyTextBlock.Text = "Empty";
            redStrategyOpen = true;
        }
        public void RemoveBlueStrategy(object sender, RoutedEventArgs e) {
            PlayMatchBtn.Opacity = 0.5;
            redStrategyTextBlock.Text = "Empty";
            blueStrategyOpen = true;
        }
    }
    public class StrategyFile {
        Grid fileview;
        private StrategyBase strategy;
        public string fileLocation;
        public string name = "Unknown";
        public string status = "Unloaded";
        public string author = "Unknown";
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
            author = strategy.author;
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
            removeButton.Click += DeleteStrategy;

            fileview.Children.Add(nameTextBlock);
            fileview.Children.Add(authorTextBlock);
            fileview.Children.Add(removeButton);
            strategySelector.AddStrategyFile(fileview);
        }

        private void DeleteStrategy(object sender, RoutedEventArgs e) {
            strategySelector.FileScrollPanel.Children.Remove(fileview);
        }
        private void AttemptSelect(object sender, RoutedEventArgs e) {
            if (strategySelector.SelectStrategy(strategy)) {

            }
        }
    }
}
