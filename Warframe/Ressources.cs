using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Warframe
{
    public class Ressources
    {
        public List<string> found;

        public String[] ressources = new string[18] {
        "Ampoule de détonite",
        "Capteurs neuronaux",
        "Cellule orokin",
        "Circuits",
        "Cristal d'argon",
        "Echantillon de fieldron",
        "Echantillon mutagène",
        "Ferrite",
        "Gallium",
        "Module de contrôle",
        "Morphics",
        "Nano spores",
        "Neurodes",
        "Pack polymère",
        "Plaque d'alliage",
        "Plastides",
        "Récupération",
        "Rubedo"
        };
        public String current_skin;
        public Border myBorderInRessources;
        public Planetes planetes = new Planetes();
        public Helper helper = new Helper();
        BrushConverter converter = new BrushConverter();
        dynamic json_file;

        public void Show_ressources(Border myBorder, String skin)
        {
            json_file = helper.Get_json_file();
            current_skin = skin;
            myBorderInRessources = myBorder;
            if (skin == "night")
            {
                myBorderInRessources.Background = (Brush)converter.ConvertFromString("#191919");
            }
            int row = 0;
            int column = 0;
            RowDefinition myRow;
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 1, 4);
            for (int i = 0; i < ressources.Length; i++)
            {
                if (i == 0 || i % 4 == 0)
                {
                    column = 0;
                    row++;
                    myRow = new RowDefinition();
                    myGrid.RowDefinitions.Add(myRow);
                }
                (Label myLabel, Border myRessource) = helper.GenerateBorder(ressources[i], 7, (Brush)converter.ConvertFromString("#C2C2C2"), row, column);
                myLabel.MouseDown += Show_planetes_for_ressources;
                myRessource.MouseDown += Show_planetes_for_ressources;
                //myRessource.Background = (Brush)converter.ConvertFromString("#292929");
                myGrid.Children.Add(myRessource);
                column++;
            }

            myRow = new RowDefinition();
            myGrid.RowDefinitions.Add(myRow);

            myBorderInRessources.Child = myGrid;
        }

        void Show_planetes_for_ressources(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                Label ressource = sender as Label;
                Grid myGrid = new Grid();
                helper.generateGridConfig(myGrid, 8, 4);
                TextBlock ressource_name = SetTitleName(ressource.Content.ToString());
                Border centerBorder = CenterBorder(ressource.Content.ToString());
                if (json_file["skin"] == "night")
                {
                    centerBorder.BorderBrush = (Brush)converter.ConvertFromString("#C2C2C2");
                }
                else
                {
                    centerBorder.BorderBrush = Brushes.Black;
                }
                myGrid.Children.Add(centerBorder);
                myGrid.Children.Add(ressource_name);
                myBorderInRessources.Child = myGrid;
            }
        }

        public Border CenterBorder(string ressource)
        {
            Border centerBorder = new Border();
            Grid.SetRow(centerBorder, 2);
            Grid.SetRowSpan(centerBorder, 5);
            Grid.SetColumn(centerBorder, 1);
            Grid.SetColumnSpan(centerBorder, 2);
            centerBorder.BorderThickness = new Thickness(1);
            centerBorder.BorderBrush = Brushes.Black;
            centerBorder.Margin = new Thickness(10);
            Grid myGrid = new Grid();
            SetCenterGridConfig(myGrid, ressource);

            TextBlock BorderCenterTitle = SetCenterBorderName();
            myGrid.Children.Add(BorderCenterTitle);
            myGrid = AddBorderCenterContent(myGrid);

            centerBorder.Child = myGrid;
            return centerBorder;
        }

        public TextBlock SetCenterBorderName()
        {
            TextBlock BorderCenterTitle = new TextBlock();
            BorderCenterTitle.Text = "Planètes";
            if (current_skin == "night")
            {
                BorderCenterTitle.Foreground = Brushes.White;
            }
            BorderCenterTitle.Margin = new Thickness(0, 0, 0, 13);
            BorderCenterTitle.FontSize = 20;
            BorderCenterTitle.HorizontalAlignment = HorizontalAlignment.Center;
            BorderCenterTitle.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(BorderCenterTitle, 0);
            Grid.SetRowSpan(BorderCenterTitle, 2);
            if (found.ToArray().Length >= 5)
            {
                Grid.SetColumnSpan(BorderCenterTitle, 6);
            }
            else
            {
                Grid.SetColumnSpan(BorderCenterTitle, 4);
            }
            
            return BorderCenterTitle;
        }

        public Grid AddBorderCenterContent(Grid myGrid)
        {
            string[] found_array = found.ToArray();
            int column = 1;
            int row = 2;
                for (var i = 0; i < found_array.Length; i++)
                {
                    TextBlock text = new TextBlock();
                    text.Text = " - " + found_array[i];
                    if (current_skin == "night")
                    {
                        text.Foreground = Brushes.White;
                    }
                    text.FontSize = 15;
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    if (found_array.Length >= 5)
                    {
                        if (i == (found_array.Length / 2) + 1)
                        {
                            column = column + 3;
                            row = 2;
                        }
                        Grid.SetRow(text, row);
                        Grid.SetColumn(text, column);
                        Grid.SetColumnSpan(text, 3);
                        row++;
                    }
                    else
                    {
                        Grid.SetRow(text, i + 2);
                        Grid.SetColumn(text, 1);
                        Grid.SetColumnSpan(text, 3);
                    }
                    myGrid.Children.Add(text);
                }
            
            return myGrid;
        }

        public TextBlock SetTitleName(String title)
        {
            TextBlock ressource_name = new TextBlock();
            ressource_name.Text = title;
            ressource_name.Margin = new Thickness(5);
            ressource_name.FontSize = 40;
            if (current_skin == "night")
            {
                ressource_name.Foreground = Brushes.White;
            }
            ressource_name.HorizontalAlignment = HorizontalAlignment.Center;
            ressource_name.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(ressource_name, 0);
            Grid.SetColumnSpan(ressource_name, 4);
            Grid.SetRowSpan(ressource_name, 2);
            return ressource_name;
        }

        public void SetCenterGridConfig(Grid myGrid, string ressource)
        {
            found = new List<string>();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 3, 4);
            RowDefinition myRow;
            int length = 0;
            for (var x= 0; x<18; x++)
            {
                for (var y = 0; y<6; y++)
                {
                    if (planetes.informations[x ,y] == ressource)
                    {
                        length++;
                        found.Add(planetes.planetes[x]);
                    }
                }
            }
            if (found.ToArray().Length >= 5)
            {
                ColumnDefinition myColDef = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(myColDef);
                myColDef = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(myColDef);
                int row_nb = length / 2;
                for (int i = 0; i < row_nb; i++)
                {
                    myRow = new RowDefinition();
                    myGrid.RowDefinitions.Add(myRow);
                }
                myRow = new RowDefinition();
                myGrid.RowDefinitions.Add(myRow);
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    myRow = new RowDefinition();
                    myGrid.RowDefinitions.Add(myRow);
                }
            }
        }
    }
}
