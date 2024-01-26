using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja
{
    [JsonObject(MemberSerialization.OptIn)]
    internal interface IContentObject
    {
        event PropertyChangedEventHandler PropertyChanged;

        public void Serialize();
        public void SetPosition(int x, int y);
        public void SetSize(int x, int y);
    }
}
