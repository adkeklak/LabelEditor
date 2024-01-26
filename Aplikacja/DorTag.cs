using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class DorTag
    {
        [JsonProperty]
        string Name { get; set; }
        [JsonProperty]
        public Collection<IContentObject> ContentObjects { get; set; }

        public DorTag(string name) 
        {
            Name = name;
            ContentObjects = new Collection<IContentObject>();
            ContentObjects.Add(new LogoObject());
            ContentObjects.Add(new RoomNumberObject());
            ContentObjects.Add(new RoomMembersObject());

        }
        public string SerializeToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(this, settings);
        }

        public static DorTag DeserializeFromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.DeserializeObject<DorTag>(json, settings);
        }
    }
}
