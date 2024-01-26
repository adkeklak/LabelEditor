using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Aplikacja
{
    [JsonObject(MemberSerialization.OptIn)]
    class RoomMembersObject : IContentObject, INotifyPropertyChanged
    {
        [JsonProperty]
        public double PositionX { get; set; } = 0;
        [JsonProperty]
        public double PositionY { get; set; } = 20;

        [JsonProperty]
        public double SizeX { get; set; } = 40;
        [JsonProperty]
        public double SizeY { get; set; } = 40;
        [JsonProperty]
        public string RoomMembersObjectText { get; set; } = "Empty";

        [JsonProperty]
        public double FontSize { get; set; } = 15;
        [JsonProperty]
        public string Font { get; set; } = "Segoe UI";
        [JsonProperty]
        public Brush Color { get; set; } = Brushes.Black;

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
}
