using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Aplikacja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DorTag Dor;
        public MainWindow()
        {
            InitializeComponent();

            Dor = new DorTag("dzwi");

            // Access RoomNumberObject
            RoomNumberObject roomNumberObject = (RoomNumberObject)Dor.ContentObjects.FirstOrDefault(obj => obj is RoomNumberObject);

            // Access RoomMembersObject
            RoomMembersObject roomMembersObject = (RoomMembersObject)Dor.ContentObjects.FirstOrDefault(obj => obj is RoomMembersObject);

            // Access LogoObject
            LogoObject logoObject = (LogoObject)Dor.ContentObjects.FirstOrDefault(obj => obj is LogoObject);

            DataContext = new { RoomNumberObject = roomNumberObject, RoomMembersObject = roomMembersObject, LogoObject = logoObject };

        }


        private void NumerSali_Click(object sender, RoutedEventArgs e)
        {
            // Get the instance of RoomNumberObject from the Tag property
            RoomNumberObject roomNumberObject = (RoomNumberObject)((Button)sender).Tag;

            // Create a new window
            Window editWindow = new Window();
            editWindow.Title = "Edit Room Number";
            editWindow.Width = 300;
            editWindow.Height = 450;

            // Create a StackPanel to hold the editing controls
            StackPanel stackPanel = new StackPanel();

            // Create TextBoxes for RoomNumberText, FontSize, Font, and Color components
            TextBox textTextBox = new TextBox();
            textTextBox.Text = roomNumberObject.RoomNumberText;

            TextBox fontSizeTextBox = new TextBox();
            fontSizeTextBox.Text = roomNumberObject.FontSize.ToString();

            TextBox fontTextBox = new TextBox();
            fontTextBox.Text = roomNumberObject.Font;

            TextBox sizeXTextBox = new TextBox();
            sizeXTextBox.Text = roomNumberObject.SizeX.ToString();

            TextBox sizeYTextBox = new TextBox();
            sizeYTextBox.Text = roomNumberObject.SizeY.ToString();

            TextBox positionXTextBox = new TextBox();
            positionXTextBox.Text = roomNumberObject.PositionX.ToString();

            TextBox positionYTextBox = new TextBox();
            positionYTextBox.Text = roomNumberObject.PositionY.ToString();

            TextBox redTextBox = new TextBox();
            redTextBox.Text = ((SolidColorBrush)roomNumberObject.Color).Color.R.ToString();

            TextBox greenTextBox = new TextBox();
            greenTextBox.Text = ((SolidColorBrush)roomNumberObject.Color).Color.G.ToString();

            TextBox blueTextBox = new TextBox();
            blueTextBox.Text = ((SolidColorBrush)roomNumberObject.Color).Color.B.ToString();

            // Create a colored rectangle to visualize the selected color
            Rectangle colorRectangle = new Rectangle();
            colorRectangle.Fill = (SolidColorBrush)roomNumberObject.Color;
            colorRectangle.Width = 50;
            colorRectangle.Height = 20;

            // Create a Confirm button and apply the existing style
            Button confirmButton = new Button();
            confirmButton.Content = "Confirm";
            confirmButton.Style = (Style)Application.Current.Resources["GreenButtonStyle"]; // Apply the style

            confirmButton.Click += (confirmSender, confirmEventArgs) =>
            {
                // Update RoomNumberObject properties based on TextBox values
                roomNumberObject.RoomNumberText = textTextBox.Text;

                // Parse FontSize from TextBox
                if (double.TryParse(fontSizeTextBox.Text, out double fontSize))
                {
                    roomNumberObject.FontSize = fontSize;
                }

                roomNumberObject.Font = fontTextBox.Text;

                // Parse RGB components from TextBoxes
                byte.TryParse(redTextBox.Text, out byte red);
                byte.TryParse(greenTextBox.Text, out byte green);
                byte.TryParse(blueTextBox.Text, out byte blue);

                // Update Color
                roomNumberObject.Color = new SolidColorBrush(Color.FromRgb(red, green, blue));

                // Update the color rectangle
                colorRectangle.Fill = new SolidColorBrush(Color.FromRgb(red, green, blue));

                int.TryParse(sizeXTextBox.Text, out int sizeX);
                int.TryParse(sizeYTextBox.Text, out int sizeY);
                int.TryParse(positionXTextBox.Text, out int positionX);
                int.TryParse(positionYTextBox.Text, out int positionY);

                roomNumberObject.SetPosition(positionX, positionY);
                roomNumberObject.SetSize (sizeX, sizeY);

                // Notify PropertyChanged for data binding
                roomNumberObject.OnPropertyChanged(nameof(RoomNumberObject.RoomNumberText));
                roomNumberObject.OnPropertyChanged(nameof(RoomNumberObject.FontSize));
                roomNumberObject.OnPropertyChanged(nameof(RoomNumberObject.Font));
                roomNumberObject.OnPropertyChanged(nameof(RoomNumberObject.Color));
            };

            // Add TextBoxes and Confirm button to the StackPanel
            stackPanel.Children.Add(new TextBlock { Text = "Room Number Text:" });
            stackPanel.Children.Add(textTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Font Size:" });
            stackPanel.Children.Add(fontSizeTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Font:" });
            stackPanel.Children.Add(fontTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Size X:" });
            stackPanel.Children.Add(sizeXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Size Y:" });
            stackPanel.Children.Add(sizeYTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Position X:" });
            stackPanel.Children.Add(positionXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Position Y:" });
            stackPanel.Children.Add(positionYTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Red:" });
            stackPanel.Children.Add(redTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Green:" });
            stackPanel.Children.Add(greenTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Blue:" });
            stackPanel.Children.Add(blueTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Selected Color:" });
            stackPanel.Children.Add(colorRectangle);

            stackPanel.Children.Add(confirmButton);

            // Set the content of the new window to the StackPanel
            editWindow.Content = stackPanel;

            // Show the new window
            editWindow.ShowDialog();
        }
        private void Nazwiska_Click(object sender, RoutedEventArgs e)
        {
            // Get the instance of RoomMembersObject from the Tag property
            RoomMembersObject roomMembersObject = (RoomMembersObject)((Button)sender).Tag;

            // Create a new window
            Window editWindow = new Window();
            editWindow.Title = "Edit Room Members";
            editWindow.Width = 300;
            editWindow.Height = 450;

            // Create a StackPanel to hold the editing controls
            StackPanel stackPanel = new StackPanel();

            // Create TextBoxes for RoomMembersObjectText, FontSize, Font, and Color components
            TextBox textTextBox = new TextBox();
            textTextBox.Text = roomMembersObject.RoomMembersObjectText;
            textTextBox.AcceptsReturn = true;  // Allow multiline input
            textTextBox.TextWrapping = TextWrapping.Wrap; // Enable text wrapping

            // Handle the PreviewKeyDown event to insert new lines on Enter key press
            textTextBox.PreviewKeyDown += (textSender, textEventArgs) =>
            {
                if (textEventArgs.Key == Key.Enter)
                {
                    textTextBox.Text += Environment.NewLine;
                    textTextBox.CaretIndex = textTextBox.Text.Length; // Move caret to the end
                    textEventArgs.Handled = true;
                }
            };

            TextBox fontSizeTextBox = new TextBox();
            fontSizeTextBox.Text = roomMembersObject.FontSize.ToString();

            TextBox fontTextBox = new TextBox();
            fontTextBox.Text = roomMembersObject.Font;

            TextBox sizeXTextBox = new TextBox();
            sizeXTextBox.Text = roomMembersObject.SizeX.ToString();

            TextBox sizeYTextBox = new TextBox();
            sizeYTextBox.Text = roomMembersObject.SizeY.ToString();

            TextBox positionXTextBox = new TextBox();
            positionXTextBox.Text = roomMembersObject.PositionX.ToString();

            TextBox positionYTextBox = new TextBox();
            positionYTextBox.Text = roomMembersObject.PositionY.ToString();

            TextBox redTextBox = new TextBox();
            redTextBox.Text = ((SolidColorBrush)roomMembersObject.Color).Color.R.ToString();

            TextBox greenTextBox = new TextBox();
            greenTextBox.Text = ((SolidColorBrush)roomMembersObject.Color).Color.G.ToString();

            TextBox blueTextBox = new TextBox();
            blueTextBox.Text = ((SolidColorBrush)roomMembersObject.Color).Color.B.ToString();

            // Create a colored rectangle to visualize the selected color
            Rectangle colorRectangle = new Rectangle();
            colorRectangle.Fill = (SolidColorBrush)roomMembersObject.Color;
            colorRectangle.Width = 50;
            colorRectangle.Height = 20;

            // Create a Confirm button and apply the existing style
            Button confirmButton = new Button();
            confirmButton.Content = "Confirm";
            confirmButton.Style = (Style)Application.Current.Resources["GreenButtonStyle"]; // Apply the style

            confirmButton.Click += (confirmSender, confirmEventArgs) =>
            {
                // Update RoomMembersObject properties based on TextBox values
                roomMembersObject.RoomMembersObjectText = textTextBox.Text;

                // Parse FontSize from TextBox
                if (double.TryParse(fontSizeTextBox.Text, out double fontSize))
                {
                    roomMembersObject.FontSize = fontSize;
                }

                roomMembersObject.Font = fontTextBox.Text;

                // Parse RGB components from TextBoxes
                byte.TryParse(redTextBox.Text, out byte red);
                byte.TryParse(greenTextBox.Text, out byte green);
                byte.TryParse(blueTextBox.Text, out byte blue);

                // Update Color
                roomMembersObject.Color = new SolidColorBrush(Color.FromRgb(red, green, blue));

                // Update the color rectangle
                colorRectangle.Fill = new SolidColorBrush(Color.FromRgb(red, green, blue));

                int.TryParse(sizeXTextBox.Text, out int sizeX);
                int.TryParse(sizeYTextBox.Text, out int sizeY);
                int.TryParse(positionXTextBox.Text, out int positionX);
                int.TryParse(positionYTextBox.Text, out int positionY);

                roomMembersObject.SetPosition(positionX, positionY);
                roomMembersObject.SetSize(sizeX, sizeY);

                // Notify PropertyChanged for data binding
                roomMembersObject.OnPropertyChanged(nameof(RoomMembersObject.RoomMembersObjectText));
                roomMembersObject.OnPropertyChanged(nameof(RoomMembersObject.FontSize));
                roomMembersObject.OnPropertyChanged(nameof(RoomMembersObject.Font));
                roomMembersObject.OnPropertyChanged(nameof(RoomMembersObject.Color));
            };

            // Add TextBoxes and Confirm button to the StackPanel
            stackPanel.Children.Add(new TextBlock { Text = "Room Members Text:" });
            stackPanel.Children.Add(textTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Font Size:" });
            stackPanel.Children.Add(fontSizeTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Font:" });
            stackPanel.Children.Add(fontTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Size X:" });
            stackPanel.Children.Add(sizeXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Size Y:" });
            stackPanel.Children.Add(sizeYTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Position X:" });
            stackPanel.Children.Add(positionXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Position Y:" });
            stackPanel.Children.Add(positionXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Red:" });
            stackPanel.Children.Add(redTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Green:" });
            stackPanel.Children.Add(greenTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Blue:" });
            stackPanel.Children.Add(blueTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Selected Color:" });
            stackPanel.Children.Add(colorRectangle);

            stackPanel.Children.Add(confirmButton);

            // Set the content of the new window to the StackPanel
            editWindow.Content = stackPanel;

            // Show the new window
            editWindow.ShowDialog();
        }
        private void LogoSciezka_Click(object sender, RoutedEventArgs e)
        {
            LogoObject logoObject = (LogoObject)((Button)sender).Tag;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                logoObject.SetImageSource(imagePath);
            }

            OpenLogoEditWindow(logoObject);
        }
        private void OpenLogoEditWindow(LogoObject logoObject)
        {
            // Create a new window
            Window editWindow = new Window();
            editWindow.Title = "Edit Logo";
            editWindow.Width = 300;
            editWindow.Height = 300;

            // Create a StackPanel to hold the editing controls
            StackPanel stackPanel = new StackPanel();

            // Create TextBoxes for SizeX and SizeY properties
            TextBox sizeXTextBox = new TextBox();
            sizeXTextBox.Text = logoObject.SizeX.ToString();

            TextBox sizeYTextBox = new TextBox();
            sizeYTextBox.Text = logoObject.SizeY.ToString();

            TextBox positionXTextBox = new TextBox();
            positionXTextBox.Text = logoObject.PositionX.ToString();

            TextBox positionYTextBox = new TextBox();
            positionYTextBox.Text = logoObject.PositionY.ToString();

            // Create an Image control to visualize the selected logo
            Image logoImage = new Image();
            logoImage.Source = logoObject.Img.Source;
            logoImage.Width = logoObject.SizeX;
            logoImage.Height = logoObject.SizeY;

            // Create a Confirm button and apply the existing style
            Button confirmButton = new Button();
            confirmButton.Content = "Confirm";
            confirmButton.Style = (Style)Application.Current.Resources["GreenButtonStyle"]; // Apply the style

            confirmButton.Click += (confirmSender, confirmEventArgs) =>
            {
                // Parse SizeX and SizeY from TextBoxes
                if (int.TryParse(sizeXTextBox.Text, out int sizeX) && int.TryParse(sizeYTextBox.Text, out int sizeY))
                {
                    logoObject.SetSize(sizeX, sizeY);
                    logoImage.Height= sizeY;
                    logoImage.Width = sizeX;
                }

                if (int.TryParse(positionXTextBox.Text, out int positionX) && int.TryParse(positionYTextBox.Text, out int positionY))
                {
                    logoObject.SetPosition(positionX, positionY);
                }

                // Notify PropertyChanged for data binding
                logoObject.OnPropertyChanged(nameof(LogoObject.SizeX));
                logoObject.OnPropertyChanged(nameof(LogoObject.SizeY));
            };

            // Add TextBoxes, Image, and Confirm button to the StackPanel
            stackPanel.Children.Add(new TextBlock { Text = "SizeX:" });
            stackPanel.Children.Add(sizeXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "SizeY:" });
            stackPanel.Children.Add(sizeYTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "PositionX:" });
            stackPanel.Children.Add(positionXTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "PositionY:" });
            stackPanel.Children.Add(positionYTextBox);

            stackPanel.Children.Add(new TextBlock { Text = "Selected Logo:" });
            stackPanel.Children.Add(logoImage);

            stackPanel.Children.Add(confirmButton);

            // Set the content of the new window to the StackPanel
            editWindow.Content = stackPanel;

            // Show the new window
            editWindow.ShowDialog();
        }
        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of DorTag

            // Serialize DorTag to JSON string
            string json = Dor.SerializeToJson();

            // Show a SaveFileDialog to choose the file path
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "JSON Files (*.json)|*.json";
            saveFileDialog.Title = "Save DorTag to JSON file";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Save the JSON string to the selected file
                File.WriteAllText(saveFileDialog.FileName, json);

            }
        }

        private void Wczytaj_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json";
            openFileDialog.Title = "Load DorTag from JSON file";

            if (openFileDialog.ShowDialog() == true)
            {
                // Read the JSON string from the selected file
                string json = File.ReadAllText(openFileDialog.FileName);

                try
                {
                    // Deserialize the DorTag object from the JSON string
                    DorTag loadedDorTag = DorTag.DeserializeFromJson(json);

                    // Optionally, use the loadedDorTag object as needed in your application
                    // For example, you might want to display the loaded data in your UI or perform other actions.
                }
                catch (Exception ex)
                {
                    // Handle any potential deserialization errors
                    MessageBox.Show($"Error loading DorTag: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Drukuj_Click(object sender, RoutedEventArgs e)
        {
            // Create a PrintDialog
            PrintDialog printDialog = new PrintDialog();

            FlowDocument flowDocumentCopy = flowDocumentViewer.Document;

            //Clone the source document
            var str = XamlWriter.Save(flowDocumentCopy);
            var stringReader = new System.IO.StringReader(str);
            var xmlReader = XmlReader.Create(stringReader);
            var CloneDoc = XamlReader.Load(xmlReader) as FlowDocument;

            //Now print using PrintDialog
            var pd = new PrintDialog();

            if (pd.ShowDialog().Value)
            {
                CloneDoc.PageHeight = pd.PrintableAreaHeight;
                CloneDoc.PageWidth = pd.PrintableAreaWidth;
                IDocumentPaginatorSource idocument = CloneDoc as IDocumentPaginatorSource;

                pd.PrintDocument(idocument.DocumentPaginator, "Printing FlowDocument");
            }
        }

    }
}

