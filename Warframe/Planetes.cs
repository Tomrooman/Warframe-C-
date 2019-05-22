using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Warframe
{
    public class Planetes
    {

        public String[] planetes = {
        "Cérès",
        "Epave orokin",
        "Eris",
        "Europe",
        "Forteresse kuva",
        "Jupiter",
        "Lua",
        "Mars",
        "Mercure",
        "Neant",
        "Neptune",
        "Phobos",
        "Pluton",
        "Saturne",
        "Sedna",
        "Terre",
        "Uranus",
        "Vénus"
    };

        public String[,] informations = new string[18, 6] {
        {"Faction : Grineer", "Niveaux : 12 - 25", "Plaque d'alliage", "Circuits", "Cellule orokin", "Ampoule de détonite"}, //Cérès
        {"Faction : Infesté", "Niveaux : 25 - 35", "Nano spores", "Echantillon mutagène", "Cellule orokin", "Neurodes"}, //Epave
        {"Faction : Infesté", "Niveaux : 34 - 45", "Nano spores", "Plastides", "Neurodes", "Echantillon mutagène"},  //Eris
        {"Faction : Corpus", "Niveaux : 18 - 33", "Morphics", "Rubedo", "Echantillon de fieldron", "Module de contrôle"}, //Europe
        {"Faction : Grineer", "Niveaux : 28 - 37", "Récupération", "Circuits", "Capteurs neuronaux", "Ampoule de détonite"}, //Forteresse kuva
        {"Faction : Corpus", "Niveaux : 15 - 30", "Récupération", "Echantillon de fieldron", "Capteurs neuronaux", "Plaque d'alliage"},  //Jupiter
        {"Faction : Corpus", "Niveaux : 25 - 30", "Ferrite", "Rubedo", "Neurodes", "Ampoule de détonite"}, //Lua
        {"Faction : Grineer", "Niveaux : 8 - 20", "Morphics", "Récupération", "Gallium", "Echantillon de fieldron"}, //Mars
        {"Faction : Infesté et Grineer", "Niveaux : 8 - 11", "Morphics", "Ferrite", "Pack polymère", "Ampoule de détonite"},  //Mercure
        {"Faction : Orokin", "Niveaux : 10 - 50", "Ferrite", "Rubedo", "Cristal d'argon", "Module de contrôle"}, //Neant
        {"Faction : Corpus", "Niveaux : 28 - 40", "Nano spores", "Ferrite", "Module de contrôle", "Echantillon de fieldron"}, //Neptune
        {"Faction : Corpus", "Niveaux : 10 - 25", "Rubedo", "Plaque d'alliage", "Récupération", "Ampoule de détonite"},  //Phobos
        {"Faction : Corpus", "Niveaux : 30 - 40", "Rubedo", "Morphics", "Plastides", "Plaque d'alliage"}, //Pluton
        {"Faction : Grineer", "Niveaux : 21 - 26", "Nano spores", "Plastides", "Cellule orokin", "Ampoule de détonite"}, //Saturne
        {"Faction : Grineer", "Niveaux : 30 - 40", "Rubedo", "Plaque d'alliage", "Récupération", "Ampoule de détonite"},  //Sedna
        {"Faction : Grineer", "Niveaux : 1 - 6", "Ferrite", "Rubedo", "Neurodes", "Ampoule de détonite" }, //Terre
        {"Faction : Grineer", "Niveaux : 24 - 30", "Pack polymère", "Plastides", "Gallium", "Ampoule de détonite"}, //Uranus
        {"Faction : Corpus", "Niveaux : 3 - 11", "Plaque d'alliage", "Pack polymère", "Circuits", "Echantillon de fieldron"}   //Vénus
    };

        public Border myBorderInPlanetes;
        public String current_skin;
        public Helper helper = new Helper();
        BrushConverter converter = new BrushConverter();
        dynamic json_file;

        public void Show_planetes(Border myBorder, String skin)
        {
            json_file = helper.Get_json_file();
            current_skin = skin;
            myBorderInPlanetes = myBorder;
            if (current_skin == "night")
            {
                myBorderInPlanetes.Background = (Brush)converter.ConvertFromString("#191919");
            }
            int row = 0;
            int column = 0;
            RowDefinition myRow;
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 1, 4);
            for (int i=0; i<planetes.Length; i++)
            {
                if (i == 0 || i % 4 == 0)
                {
                    column = 0;
                    row++;
                    myRow = new RowDefinition();
                    myGrid.RowDefinitions.Add(myRow);
                }
                (Label myLabel, Border myPlanete) = helper.GenerateBorder(planetes[i], 7, (Brush)converter.ConvertFromString("#C2C2C2"), row, column);
                myLabel.MouseDown += Show_informations_planete;
                myPlanete.MouseDown += Show_informations_planete;
                myGrid.Children.Add(myPlanete);
                column++;
            }
            myRow = new RowDefinition();
            myGrid.RowDefinitions.Add(myRow);

            myBorderInPlanetes.Child = myGrid;
        }

        void Show_informations_planete(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                Label planete = sender as Label;
                int index = 0;

                for (var i = 0; i < planetes.Length; i++)
                {
                    if (planetes[i] == planete.Content.ToString())
                    {
                        index = i;
                    }
                }

                Grid myGrid = new Grid();
                helper.generateGridConfig(myGrid, 5, 4);
                TextBlock planete_name = SetTitleName(planete.Content.ToString());
                Border leftBorder = LeftBorder(planete.Content.ToString(), index);
                Border rightBorder = RightBorder(planete.Content.ToString(), index);
                if (json_file["skin"] == "night")
                {
                    leftBorder.BorderBrush = (Brush)converter.ConvertFromString("#C2C2C2");
                    rightBorder.BorderBrush = (Brush)converter.ConvertFromString("#C2C2C2");
                }
                else
                {
                    leftBorder.BorderBrush = Brushes.Black;
                    rightBorder.BorderBrush = Brushes.Black;
                }
                myGrid.Children.Add(leftBorder);
                myGrid.Children.Add(rightBorder);
                myGrid.Children.Add(planete_name);
                myBorderInPlanetes.Child = myGrid;
            }
            
        }

        public Border LeftBorder(string planete, int index)
        {
            Border infosBorder = new Border();
            Grid.SetRow(infosBorder, 1);
            Grid.SetRowSpan(infosBorder, 3);
            Grid.SetColumn(infosBorder, 0);
            Grid.SetColumnSpan(infosBorder, 2);
            infosBorder.BorderThickness = new Thickness(1);
            infosBorder.BorderBrush = Brushes.Black;
            infosBorder.Margin = new Thickness(10);
            Grid myGrid = new Grid();
            helper.generateGridConfig(myGrid, 5, 4);

            TextBlock BorderLeftTitle = SetLeftBorderName();
            myGrid.Children.Add(BorderLeftTitle);
            myGrid = AddBorderLeftContent(myGrid, planete, index);

            infosBorder.Child = myGrid;
            return infosBorder;
        }

        public TextBlock SetLeftBorderName()
        {
            TextBlock BorderLeftTitle = new TextBlock();
            BorderLeftTitle.Text = "Informations";
            if (current_skin == "night")
            {
                BorderLeftTitle.Foreground = Brushes.White;
            }
            BorderLeftTitle.FontSize = 20;
            BorderLeftTitle.HorizontalAlignment = HorizontalAlignment.Center;
            BorderLeftTitle.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(BorderLeftTitle, 0);
            Grid.SetRowSpan(BorderLeftTitle, 2);
            Grid.SetColumnSpan(BorderLeftTitle, 4);
            return BorderLeftTitle;
        }

        public Grid AddBorderLeftContent(Grid myGrid, string planete, int index)
        {
            for (var i = 0; i < 2; i++) {
                TextBlock text = new TextBlock();
                text.Text = informations[index, i];
                if (current_skin == "night")
                {
                    text.Foreground = Brushes.White;
                }
                text.FontSize = 15;
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(text, i+2);
                Grid.SetColumn(text, 1);
                Grid.SetColumnSpan(text, 3);
                myGrid.Children.Add(text);
            }
            
            return myGrid;
        }

        public Border RightBorder(string planete, int index)
        {
            Border ressourcesBorder = new Border();
            Grid.SetRow(ressourcesBorder, 1);
            Grid.SetRowSpan(ressourcesBorder, 3);
            Grid.SetColumn(ressourcesBorder, 2);
            Grid.SetColumnSpan(ressourcesBorder, 2);
            ressourcesBorder.BorderThickness = new Thickness(1);
            ressourcesBorder.BorderBrush = Brushes.Black;
            ressourcesBorder.Margin = new Thickness(10);
            Grid myGrid = new Grid();
            helper.generateGridConfig(myGrid, 8, 4);

            TextBlock BorderRightTitle = SetRightBorderName();
            myGrid.Children.Add(BorderRightTitle);
            myGrid = AddBorderRightContent(myGrid, planete, index);

            ressourcesBorder.Child = myGrid;
            return ressourcesBorder;
        }

        public TextBlock SetRightBorderName()
        {
            TextBlock BorderRightTitle = new TextBlock();
            BorderRightTitle.Text = "Ressources";
            if (current_skin == "night")
            {
                BorderRightTitle.Foreground = Brushes.White;
            }
            BorderRightTitle.Margin = new Thickness(0, 0, 0, 13);
            BorderRightTitle.FontSize = 20;
            BorderRightTitle.HorizontalAlignment = HorizontalAlignment.Center;
            BorderRightTitle.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(BorderRightTitle, 1);
            Grid.SetRowSpan(BorderRightTitle, 2);
            Grid.SetColumnSpan(BorderRightTitle, 4);
            return BorderRightTitle;
        }

        public Grid AddBorderRightContent(Grid myGrid, string planete, int index)
        {
            for (var i = 2; i < 6; i++)
            {
                TextBlock text = new TextBlock();
                text.Text = " - " + informations[index, i];
                if (current_skin == "night")
                {
                    text.Foreground = Brushes.White;
                }
                text.FontSize = 15;
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(text, i+1);
                Grid.SetColumn(text, 1);
                Grid.SetColumnSpan(text, 3);
                myGrid.Children.Add(text);
            }

            return myGrid;
        }

        public TextBlock SetTitleName(String title)
        {
            TextBlock planete_name = new TextBlock();
            planete_name.Text = title;
            if (current_skin == "night")
            {
                planete_name.Foreground = Brushes.White;
            }
            planete_name.Margin = new Thickness(5);
            planete_name.FontSize = 40;
            planete_name.HorizontalAlignment = HorizontalAlignment.Center;
            planete_name.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(planete_name, 0);
            Grid.SetColumnSpan(planete_name, 4);
            return planete_name;
        }
    }
}
