using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Media;

namespace Warframe
{
    public class Helper
    {

        public void generateGridConfig(Grid myGrid, int rows, int columns) {

            for (var i = 0; i < rows; i++)
            {
                RowDefinition myRow = new RowDefinition();
                myGrid.RowDefinitions.Add(myRow);
            }

            for (var i = 0; i < columns; i++)
            {
                ColumnDefinition myCol = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(myCol);
            }

        }

        public dynamic Get_json_file()
        {
            using (StreamReader r = new StreamReader("warframe.json"))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<dynamic>(json);
            }
        }

        public Button generateButton(Grid myGrid, String content, int margin, int[] pos, int[] posSpan, string[] align) {
            Button myButton = new Button();
            myButton.Margin = new Thickness(margin);

            myButton.Content = content;
            Grid.SetColumn(myButton, pos[1]);
            Grid.SetRow(myButton, pos[0]);

            if (posSpan[0] != 99)
            {
                Grid.SetColumnSpan(myButton, posSpan[1]);
                Grid.SetRowSpan(myButton, posSpan[0]);
            }

            if (align[0] != "empty")
            {
                if (align[0] == "center")
                {
                    myButton.HorizontalAlignment = HorizontalAlignment.Center;
                }
                if (align[1] == "center")
                {
                    myButton.VerticalAlignment = VerticalAlignment.Center;
                }
            }

            myGrid.Children.Add(myButton);
            return myButton;
        }

        public (Label, Border) GenerateBorder(string content, int thickness, Brush color, int row, int column)
        {
            Border border = new Border();
            Label label = new Label();
            label.Content = content;
            //label.HorizontalAlignment = HorizontalAlignment.Center;
            //label.VerticalAlignment = VerticalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            border.BorderThickness = new Thickness(1);
            border.Margin = new Thickness(thickness);
            border.CornerRadius = new CornerRadius(15, 15, 15, 15);
            dynamic json_file = Get_json_file();
            if (json_file["skin"] == "night")
            {
                border.BorderBrush = color;
                label.Foreground = color;
            } else
            {
                border.BorderBrush = Brushes.Black;
                label.Foreground = Brushes.Black;
            }
            Grid.SetColumn(border, column);
            Grid.SetRow(border, row);
            border.Child = label;
            return (label, border);
        }

    }
}
