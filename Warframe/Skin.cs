using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Warframe
{

    public class Skin
    {

        public Border myBorderInSkin;
        TextBlock text;
        RadioButton nuit;
        RadioButton jour;
        dynamic json_file;
        string json;
        DockPanel myDockPanel;
        public Helper helper = new Helper();
        BrushConverter converter = new BrushConverter();

        public void Show_skin(Border myBorder, DockPanel Dockpanel)
        {
            myDockPanel = Dockpanel;
            json_file = helper.Get_json_file();
            myBorderInSkin = myBorder;
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 5, 4);
            text = new TextBlock();
            text.Text = "Choisis ton thème";
            text.FontSize = 40;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(text, 0);
            Grid.SetRowSpan(text, 2);
            Grid.SetColumn(text, 0);
            Grid.SetColumnSpan(text, 4);
            jour = new RadioButton();
            jour.Content = "Jour";
            jour.FontSize = 20;
            if (json_file["skin"].ToString() == "day")
            {
                jour.IsChecked = true;
            }
            jour.HorizontalAlignment = HorizontalAlignment.Center;
            jour.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(jour, 2);
            Grid.SetColumnSpan(jour, 2);
            nuit = new RadioButton();
            nuit.Content = "Nuit";
            nuit.FontSize = 20;
            if (json_file["skin"].ToString() == "night")
            {
                text.Foreground = Brushes.White;
                jour.Foreground = Brushes.White;
                nuit.Foreground = Brushes.White;
                nuit.IsChecked = true;
            }
            nuit.HorizontalAlignment = HorizontalAlignment.Center;
            nuit.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(nuit, 2);
            Grid.SetColumn(nuit, 2);
            Grid.SetColumnSpan(nuit, 2);
            Button confirm = new Button();
            confirm.Content = "Confirmer";
            confirm.HorizontalAlignment = HorizontalAlignment.Center;
            confirm.VerticalAlignment = VerticalAlignment.Center;
            confirm.Click += change_skin;
            confirm.Padding = new Thickness(10);
            Grid.SetRow(confirm, 3);
            Grid.SetColumn(confirm, 1);
            Grid.SetColumnSpan(confirm, 2);
            myGrid.Children.Add(text);
            myGrid.Children.Add(jour);
            myGrid.Children.Add(nuit);
            myGrid.Children.Add(confirm);
            myBorderInSkin.Child = myGrid;
        }

        public void change_skin(object sender, RoutedEventArgs e)
        {
            if (nuit.IsChecked == true)
            {
                json_file["skin"] = "night";
            }
            if (jour.IsChecked == true)
            {
                json_file["skin"] = "day";
            }
            string convertedToJSON = JsonConvert.SerializeObject(json_file);
            System.IO.File.WriteAllText("warframe.json", convertedToJSON);
            using (StreamReader r = new StreamReader("warframe.json"))
            {
                json = r.ReadToEnd();

                json_file = JsonConvert.DeserializeObject<dynamic>(json);
            }
            if (json_file["skin"].ToString() == "night")
            {
                text.Foreground = Brushes.White;
                jour.Foreground = Brushes.White;
                nuit.Foreground = Brushes.White;
                
                myBorderInSkin.Background = (Brush)converter.ConvertFromString("#191919");
            }
            else
            {
                text.Foreground = Brushes.Black;
                jour.Foreground = Brushes.Black;
                nuit.Foreground = Brushes.Black;
                myBorderInSkin.Background = Brushes.White;
            }
            myDockPanel.UpdateLayout();
        }
    }
}
