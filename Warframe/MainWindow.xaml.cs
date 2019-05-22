using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using System.IO;
using System.Timers;

namespace Warframe
{
    public partial class MainWindow : Window
    {

        public Border myBorder = new Border();
        public Planetes planetes = new Planetes();
        public Ressources ressources = new Ressources();
        public Craft craft = new Craft();
        public Skin skin = new Skin();
        public Helper helper = new Helper();
        dynamic json_file;
        string json;
        string current_skin;
        DockPanel myDockPanel = new DockPanel();
        Timer timer = new Timer();
        BrushConverter converter = new BrushConverter();

        public MainWindow()
        {
            InitializeComponent();
            using (StreamReader r = new StreamReader("warframe.json"))
            {
                json = r.ReadToEnd();

                json_file = JsonConvert.DeserializeObject<dynamic>(json);
                current_skin = json_file["skin"].ToString();
            }
            if (json_file["skin"].ToString() == "night")
            {
                myBorder.Background = (Brush)converter.ConvertFromString("#191919");
            }
            else
            {
                myBorder.Background = Brushes.White;
            }
            myDockPanel = GenerateTopPanel(myDockPanel);
            myDockPanel = GenerateBottomPanel(myDockPanel);
            myDockPanel = GenerateCenterPanel(myDockPanel);
            TomWindow.Content = myDockPanel;
        }

        public DockPanel GenerateCenterPanel(DockPanel myDockPanel)
        {
            myBorder.BorderThickness = new Thickness(1);
            DockPanel.SetDock(myBorder, Dock.Top);
            myDockPanel.Children.Add(myBorder);

            return myDockPanel;
        }

        public DockPanel GenerateBottomPanel(DockPanel myDockPanel)
        {
            Border myBorder4 = new Border();
            myBorder4.BorderBrush = Brushes.Black;
            myBorder4.BorderThickness = new Thickness(1);
            DockPanel.SetDock(myBorder4, Dock.Bottom);
            StackPanel myStackPanel2 = new StackPanel();
            myStackPanel2.Orientation = Orientation.Horizontal;
            TextBlock myTextBlock2 = new TextBlock();
            myTextBlock2.Text = "Afficher le prochain craft terminé";
            myTextBlock2.Margin = new Thickness(5);
            myStackPanel2.Children.Add(myTextBlock2);
            myBorder4.Child = myStackPanel2;

            myDockPanel.Children.Add(myBorder4);

            return myDockPanel;
        }

        public DockPanel GenerateTopPanel (DockPanel myDockPanel)
        {
            Border myBorder3 = new Border();
            myBorder3.BorderThickness = new Thickness(1);
            myBorder3.BorderBrush = Brushes.Black;
            DockPanel.SetDock(myBorder3, Dock.Top);
            Grid myGrid = new Grid();
            helper.generateGridConfig(myGrid, 1, 4);
            Button myButton3 = new Button();
            myButton3.Margin = new Thickness(5);
            myButton3.Content = "Craft";
            myButton3.Click += handle_show_craft;
            Grid.SetColumn(myButton3, 0);
            Grid.SetRow(myButton3, 0);
            Button myButton4 = new Button();
            myButton4.Margin = new Thickness(5);
            myButton4.Content = "Planètes";
            myButton4.Click += handle_show_planetes;
            Grid.SetColumn(myButton4, 1);
            Grid.SetRow(myButton4, 0);
            Button myButton5 = new Button();
            myButton5.Margin = new Thickness(5);
            myButton5.Content = "Ressources";
            myButton5.Click += handle_show_ressources;
            Grid.SetColumn(myButton5, 2);
            Grid.SetRow(myButton5, 0);
            Button myButton6 = new Button();
            myButton6.Margin = new Thickness(5);
            myButton6.Content = "Thème";
            myButton6.Click += handle_show_skin;
            Grid.SetColumn(myButton6, 3);
            Grid.SetRow(myButton6, 0);

            myGrid.Children.Add(myButton3);
            myGrid.Children.Add(myButton4);
            myGrid.Children.Add(myButton5);
            myGrid.Children.Add(myButton6);
            myBorder3.Child = myGrid;

            myDockPanel.Children.Add(myBorder3);

            return myDockPanel;
        }

        void handle_show_craft(object sender, RoutedEventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
            }
            craft.Show_craft(myBorder, timer);
        }

        void handle_show_planetes(object sender, RoutedEventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
            }
            planetes.Show_planetes(myBorder, helper.Get_json_file()["skin"].ToString());
        }

        void handle_show_ressources(object sender, RoutedEventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
            }
            ressources.Show_ressources(myBorder, helper.Get_json_file()["skin"].ToString());
        }

        void handle_show_skin(object sender, RoutedEventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
            }
            skin.Show_skin(myBorder, myDockPanel);
        }
    }
}
