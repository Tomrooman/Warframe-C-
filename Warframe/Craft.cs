using Newtonsoft.Json;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace Warframe
{
    public class Craft
    {
        public String current_skin;
        public Border myBorderInCraft;
        dynamic json_file;
        public Helper helper = new Helper();
        string selected_craft;
        TextBox name;
        TextBox minutes;
        TextBox hours;
        TextBox days;
        Timer timer;
        Timer effectTimer;
        List<Label> time_list;
        List<string> end_list;
        List<Border> CraftBorderArray;
        Grid CraftListGrid;
        BrushConverter converter = new BrushConverter();
        int count = 0;

        public void Show_craft(Border myBorder, Timer timerReceive)
        {
            timer = timerReceive;
            current_skin = helper.Get_json_file()["skin"];
            myBorderInCraft = myBorder;
            json_file = helper.Get_json_file();

            if (current_skin == "night")
            {
                myBorderInCraft.Background = (Brush)converter.ConvertFromString("#191919");
            }
            CraftBorderArray = new List<Border>();
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 3, 4);
            Button myButton = helper.generateButton(myGrid, "Rajouter un craft", 5, new int[]{1, 1}, new int[] {99}, new string[] {"empty"});
            myButton.Click += Add_craft_form;
            Button myButton2 = helper.generateButton(myGrid, "Afficher les crafts", 5, new int[] { 1, 2 }, new int[] { 99 }, new string[] { "empty" });
            myButton2.Click += Show_craft_list;
            myBorderInCraft.Child = myGrid;
        }

        public void Callback(object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { Update_countdown(); }));
        }

        public void Update_countdown()
        {
            int count = 0;
            foreach (string finished in end_list)
            {
                string countdown = Calcul_countdown(finished);
                time_list[count].Content = countdown;
                count++;
            }
        }

        public void Callback_fadeIn(object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { Fade_in_Effect(); }));
        }

        public void Fade_in_Effect()
        {
            if (count >= CraftBorderArray.Count)
            {
                effectTimer.Enabled = false;
                effectTimer.Stop();
                count = 0;
            }
            if (CraftBorderArray[count].Opacity.ToString() == "1")
            {
                count++;
            }
            else
            {
                CraftBorderArray[count].Opacity += 0.1;
            }
        }

        public List<dynamic> Sort_craft()
        {
            List<dynamic> json_craft_sorted = new List<dynamic>();

            foreach (dynamic oneCraft in json_file["craft"])
            {
                json_craft_sorted.Add(oneCraft);
            }
            return json_craft_sorted;
        }

        public void Show_craft_list(object sender, RoutedEventArgs e)
        {
            CraftListGrid = new Grid();
            myBorderInCraft.Child = CraftListGrid;
            RowDefinition myRow;
            CraftListGrid.ShowGridLines = false;
            helper.generateGridConfig(CraftListGrid, 3, 4);
            int column = 0;
            int row = 0;
            int i = 0;
            time_list = new List<Label>();
            end_list = new List<string>();
            List<dynamic> json_craft_sorted = Sort_craft();
            foreach (dynamic oneCraft in json_craft_sorted)
            {
                string[] data = Take_craft_data(oneCraft.ToString());
;               if (i == 0 || i % 4 == 0)
                {
                    if (i == 0)
                    {
                        row++;
                    }
                    else
                    {
                        row = row + 2;
                    }
                    column = 0;
                    myRow = new RowDefinition();
                    CraftListGrid.RowDefinitions.Add(myRow);
                }
                CraftListGrid.Children.Add(CraftListBorder(data, row, column));
                column++;
                i++;
            }
            myRow = new RowDefinition();
            CraftListGrid.RowDefinitions.Add(myRow);
            timer.Interval = 1000;
            timer.Elapsed += Callback;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            // Scrollbar
            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            myScrollViewer.Content = CraftListGrid;
            //
            myBorderInCraft.Child = myScrollViewer;
            effectTimer = new Timer();
            effectTimer.Interval = 30;
            effectTimer.Elapsed += Callback_fadeIn;
            effectTimer.AutoReset = true;
            effectTimer.Enabled = true;
            effectTimer.Start();
        }

        public string[] Take_craft_data(string oneCraft)
        {
            int virgule = oneCraft.IndexOf(',') - 1;
            int length = oneCraft.Split(':')[1].IndexOf(",") - 2;
            string title = oneCraft.Split(':')[1].Substring(1, length);
            string finished = oneCraft.Split(',')[1];
            finished = finished.Substring(finished.IndexOf(":") + 2, 19);
            return new string []{title, finished};
        }

        void ClickCraft(object sender, RoutedEventArgs e, string[] data)
        {
            selected_craft = data[0];

            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 5, 6);
            SetCenterName(myGrid);
            Button myNoButton = helper.generateButton(myGrid, "Non", 10, new int[] { 3, 2 }, new int[] { 99 }, new string[] { "empty" });
            Button myYesButton = helper.generateButton(myGrid, "Oui", 10, new int[] { 3, 3 }, new int[] { 99 }, new string[] { "empty" });
            myNoButton.Click += Show_craft_list;
            myYesButton.Click += Handle_remove_craft;
            Border centerBorder = CenterBorder(data);
            myGrid.Children.Add(centerBorder);
            myBorderInCraft.Child = myGrid;
        }

        public void Handle_remove_craft(object sender, RoutedEventArgs e)
        {
            int remove_count = 0;
            int found = 0;

            foreach (dynamic one_craft in json_file["craft"])
            {
                string oneCraft_name = Take_craft_data(one_craft.ToString())[0];
                if (oneCraft_name == selected_craft)
                {
                    found = remove_count;
                }
                remove_count++;
            }
            json_file["craft"].RemoveAt(found);
            string convertedToJSON = JsonConvert.SerializeObject(json_file);
            System.IO.File.WriteAllText("warframe.json", convertedToJSON);
            Show_craft_list(sender, e);
        }

        public void SetCenterName(Grid myGrid)
        {
            TextBlock title = new TextBlock();
            title.Text = "Supprimer ?";
            if (current_skin == "night")
            {
                title.Foreground = Brushes.White;
            }
            title.FontSize = 40;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(title, 0);
            Grid.SetColumn(title, 2);
            Grid.SetColumnSpan(title, 2);
            myGrid.Children.Add(title);
        }

        public Border CraftListBorder(string[] data, int row, int column)
        {
            Border CraftBorder = new Border();
            Grid.SetRow(CraftBorder, row);
            Grid.SetColumn(CraftBorder, column);
            Grid.SetRowSpan(CraftBorder, 2);
            
            CraftBorder.BorderThickness = new Thickness(1);
            if (json_file["skin"] == "night")
            {
                CraftBorder.BorderBrush = (Brush)converter.ConvertFromString("#C2C2C2");
            }
            else
            {
                CraftBorder.BorderBrush = Brushes.Black;
            }
            CraftBorder.Margin = new Thickness(10);
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 3, 4);
            myGrid.MouseDown += (sender, e) => ClickCraft(sender, e, data);

            Label CenterTitle = SetCenterBorderName(data[0]);
            myGrid.Children.Add(CenterTitle);
            myGrid = AddBorderCenterContent(myGrid, data[1], "little");
            CraftBorder.Child = myGrid;
            CraftBorder.Opacity = 0;
            CraftBorderArray.Add(CraftBorder);
            return CraftBorder;
        }

        public Border CenterBorder(string[] data)
        {
            Border centerBorder = new Border();
            Grid.SetRow(centerBorder, 1);
            Grid.SetRowSpan(centerBorder, 2);
            Grid.SetColumn(centerBorder, 2);
            Grid.SetColumnSpan(centerBorder, 2);
            centerBorder.BorderThickness = new Thickness(1);
            if (json_file["skin"] == "night")
            {
                centerBorder.BorderBrush = (Brush)converter.ConvertFromString("#C2C2C2");
            }
            else
            {
                centerBorder.BorderBrush = Brushes.Black;
            }
            
            centerBorder.Margin = new Thickness(10);
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 3, 4);

            Label CenterTitle = SetCenterBorderName(data[0]);
            myGrid.Children.Add(CenterTitle);
            myGrid = AddBorderCenterContent(myGrid, data[1]);

            centerBorder.Child = myGrid;
            return centerBorder;
        }

        public Label SetCenterBorderName(string craft_name)
        {
            Label BorderCenterTitle = new Label();
            BorderCenterTitle.Content = craft_name;
            if (current_skin == "night")
            {
                BorderCenterTitle.Foreground = Brushes.White;
            }
            BorderCenterTitle.FontSize = 20;
            Grid.SetRow(BorderCenterTitle, 0);
            Grid.SetColumnSpan(BorderCenterTitle, 4);
            BorderCenterTitle.HorizontalContentAlignment = HorizontalAlignment.Center;
            BorderCenterTitle.VerticalContentAlignment = VerticalAlignment.Top;
            return BorderCenterTitle;
        }

        public string Calcul_countdown(string finished)
        {
            string countdown_value = "";
            DateTime localDate = DateTime.Now;
            DateTime finished_time = DateTime.Parse(finished.ToString());
            if ((finished_time - localDate).ToString().Substring(0, 1) == "-")
            {
                return "Craft terminé !";
            }
            else
            {
                if ((finished_time - localDate).Days != 0)
                {
                    countdown_value = (finished_time - localDate).Days + ":";
                }
                if ((finished_time - localDate).Hours != 0)
                {
                    countdown_value += (finished_time - localDate).Hours + ":";
                }
                if ((finished_time - localDate).Minutes != 0)
                {
                    countdown_value += (finished_time - localDate).Minutes + ":";
                }
                if ((finished_time - localDate).Seconds != 0)
                {
                    countdown_value += (finished_time - localDate).Seconds.ToString();
                }
                else
                {
                    countdown_value += "0";
                }
                return "Temps restants : " + countdown_value;
            }
        }

        public Grid AddBorderCenterContent(Grid myGrid, string finished, string size = "little")
        {
            Label time = new Label();
            string countdown = Calcul_countdown(finished);
            time.Content = countdown;
            Label end = new Label();
            end.Content = "Terminer le " + finished;
            time.HorizontalContentAlignment = HorizontalAlignment.Center;
            time.VerticalContentAlignment = VerticalAlignment.Center;
            end.HorizontalContentAlignment = HorizontalAlignment.Center;
            end.VerticalContentAlignment = VerticalAlignment.Center;
            if (current_skin == "night")
            {
                time.Foreground = Brushes.White;
                end.Foreground = Brushes.White;
            }
            if (size == "little")
            {
                time.FontSize = 14;
                end.FontSize = 12;
            }
            else
            {
                time.FontSize = 15;
                end.FontSize = 15;
            }

            Grid.SetRow(time, 2);
            Grid.SetColumn(time, 0);
            Grid.SetColumnSpan(time, 4);
            Grid.SetRow(end, 1);
            Grid.SetColumn(end, 0);
            Grid.SetColumnSpan(end, 4);

            end_list.Add(finished);
            time_list.Add(time);

            myGrid.Children.Add(time);
            myGrid.Children.Add(end);
            return myGrid;
        }

        public void Add_craft_form(object sender, RoutedEventArgs e)
        {
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = false;
            helper.generateGridConfig(myGrid, 5, 4);
            Button confirmBtn = helper.generateButton(myGrid, "Confirmer", 60, new int[] { 3, 1 }, new int[] { 2, 2 }, new string[] { "empty"});
            confirmBtn.Click += Add_craft;
            Add_craft_form_title(myGrid);
            Add_craft_form_input(myGrid);
            myBorderInCraft.Child = myGrid;
        }

        public void Add_craft_form_title(Grid myGrid)
        {
            TextBlock title = new TextBlock();
            title.Text = "Rajouter un craft";
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.VerticalAlignment = VerticalAlignment.Center;
            if (current_skin == "night")
            {
                title.Foreground = Brushes.White;
            }
            title.FontSize = 40;
            Grid.SetRow(title, 0);
            Grid.SetRowSpan(title, 2);
            Grid.SetColumn(title, 1);
            Grid.SetColumnSpan(title, 2);
            myGrid.Children.Add(title);
        }

        public void Add_craft_form_input(Grid myGrid)
        {
            name = new TextBox();
            name.Width = 200;
            name.Height = 20;
            Grid.SetRow(name, 1);
            Grid.SetRowSpan(name, 2);
            Grid.SetColumn(name, 1);
            Grid.SetColumnSpan(name, 2);
            TextBlock name_label = new TextBlock();
            name_label.Text = "Nom";
            name_label.FontSize = 20;
            name_label.Margin = new Thickness(0, 0, 65, 5);
            name_label.HorizontalAlignment = HorizontalAlignment.Center;
            name_label.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(name_label, 1);
            Grid.SetRowSpan(name_label, 2);
            Grid.SetColumn(name_label, 1);

            days = new TextBox();
            days.Width = 50;
            days.Height = 20;
            Grid.SetRow(days, 2);
            Grid.SetRowSpan(days, 2);
            Grid.SetColumn(days, 0);
            Grid.SetColumnSpan(days, 2);
            TextBlock days_label = new TextBlock();
            days_label.Text = "Jour(s)";
            days_label.FontSize = 20;
            days_label.Margin = new Thickness(90, 0, 0, 5);
            days_label.HorizontalAlignment = HorizontalAlignment.Center;
            days_label.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(days_label, 2);
            Grid.SetRowSpan(days_label, 2);
            Grid.SetColumn(days_label, 0);

            hours = new TextBox();
            hours.Width = 50;
            hours.Height = 20;
            Grid.SetRow(hours, 2);
            Grid.SetRowSpan(hours, 2);
            Grid.SetColumn(hours, 1);
            Grid.SetColumnSpan(hours, 2);
            TextBlock hours_label = new TextBlock();
            hours_label.Text = "Heure(s)";
            hours_label.FontSize = 20;
            hours_label.Margin = new Thickness(77, 0, 0, 5);
            hours_label.HorizontalAlignment = HorizontalAlignment.Center;
            hours_label.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(hours_label, 2);
            Grid.SetRowSpan(hours_label, 2);
            Grid.SetColumn(hours_label, 1);

            minutes = new TextBox();
            minutes.Width = 50;
            minutes.Height = 20;
            Grid.SetRow(minutes, 2);
            Grid.SetRowSpan(minutes, 2);
            Grid.SetColumn(minutes, 2);
            Grid.SetColumnSpan(minutes, 2);
            TextBlock minutes_label = new TextBlock();
            minutes_label.Text = "Minute(s)";
            minutes_label.FontSize = 20;
            minutes_label.Margin = new Thickness(70, 0, 0, 5);
            minutes_label.HorizontalAlignment = HorizontalAlignment.Center;
            minutes_label.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(minutes_label, 2);
            Grid.SetRowSpan(minutes_label, 2);
            Grid.SetColumn(minutes_label, 2);

            if (current_skin == "night")
            {
                name_label.Foreground = Brushes.White;
                days_label.Foreground = Brushes.White;
                hours_label.Foreground = Brushes.White;
                minutes_label.Foreground = Brushes.White;
            }
            myGrid.Children.Add(name_label);
            myGrid.Children.Add(name);
            myGrid.Children.Add(days_label);
            myGrid.Children.Add(days);
            myGrid.Children.Add(hours_label);
            myGrid.Children.Add(hours);
            myGrid.Children.Add(minutes_label);
            myGrid.Children.Add(minutes);
        }

        public void Add_craft(object sender, RoutedEventArgs e)
        {
            if (name.Text.Length != 0 && (days.Text.Length != 0 || hours.Text.Length != 0 || minutes.Text.Length != 0))
            {
                int duration = Calcul_duration();
                DateTime localDate = DateTime.Now;
                Craft_object one_craft_object = new Craft_object {Title = name.Text, Finished = localDate.AddMinutes(duration).ToString() };
                string converted_one_craft = JsonConvert.SerializeObject(one_craft_object);
                json_file["craft"].Add(converted_one_craft);
                string convertedToJSON = JsonConvert.SerializeObject(json_file);
                System.IO.File.WriteAllText("warframe.json", convertedToJSON);
                CraftBorderArray = new List<Border>();
                json_file = helper.Get_json_file();
                Show_craft_list(sender, e);
            }
        }

        public int Calcul_duration()
        {
            int duration = 0;
            if (days.Text.Length != 0)
            {
                duration = int.Parse(days.Text) * 24 * 60;
            }
            if (hours.Text.Length != 0)
            {
                duration = duration + int.Parse(hours.Text) * 60;
            }
            if (minutes.Text.Length != 0)
            {
                duration = duration + int.Parse(minutes.Text);
            }
            return duration;
        }
    }
}

public class Craft_object
{
    public string Title { get; set; }
    public string Finished { get; set; }
}
