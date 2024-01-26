using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Aplikacja
{
    [JsonObject(MemberSerialization.OptIn)]
    class LogoObject : IContentObject, INotifyPropertyChanged
    {

        [JsonProperty]
        public double PositionX { get; set; } = 0;
        [JsonProperty]
        public double PositionY { get; set; } = 0;

        [JsonProperty]
        public double SizeX { get; set; } = 50;
        [JsonProperty]
        public double SizeY { get; set; } = 50;
            
        //[JsonProperty]
        //[JsonConverter(typeof(ImageConverter))]
        public Image Img { get; set; }

        public LogoObject(string imagePath = "logo.gif")
        {
            SetImageSource(imagePath);
        }

        public void SetImageSource(string imagePath)
        {
            Img = new Image();
            BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            Img.Source = bitmap;
            Img.Width = SizeX;
            Img.Height = SizeY;
        }

        public string GetImagePath()
        {
            if (Img.Source is BitmapImage bitmapImage && bitmapImage.UriSource != null)
            {
                // The UriSource property contains the original file path
                return bitmapImage.UriSource.LocalPath;
            }
            else
            {
                // Handle other cases or return null if the path is not available
                return null;
            }
        }

        public void Serialize()
        {
            string json = JsonConvert.SerializeObject(this);
        }

        public void SetPosition(int x, int y)
        {
            this.PositionX = x; this.PositionY = y;

            this.OnPropertyChanged(nameof(RoomNumberObject.PositionX));
            this.OnPropertyChanged(nameof(RoomNumberObject.PositionY));
        }

        public void SetSize(int x, int y)
        {
            this.SizeX = x; this.SizeY = y;

            Img.Width = SizeX;
            Img.Height = SizeY;

            this.OnPropertyChanged(nameof(RoomNumberObject.SizeX));
            this.OnPropertyChanged(nameof(RoomNumberObject.SizeY));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Thickness Margin
        {
            get { return new Thickness(PositionX, PositionY, 0, 0); }
        }
    }


    public class ImageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Image);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Read the JSON data as a byte array
            byte[] imageBytes = serializer.Deserialize<byte[]>(reader);

            // Convert the byte array back to an Image
            if (imageBytes != null && imageBytes.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Convert the Image to a byte array before writing to JSON
            Image image = (Image)value;
            if (image != null)
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    byte[] imageBytes = stream.ToArray();

                    // Write the byte array to JSON
                    serializer.Serialize(writer, imageBytes);
                }
            }
            else
            {
                // Write null if the Image is null
                serializer.Serialize(writer, null);
            }
        }

    }
}
