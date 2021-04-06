using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace PolytoriaTools
{
    class Game
    {
        public Game(int id, string name, string description, bool active, int creator_id, DateTime creation_date, DateTime last_update)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.active = active;
            this.creator_id = creator_id;
            this.creation_date = creation_date;
            this.last_update = last_update;
        }

        public User GetCreator()
        {
            return User.GetUserById(creator_id);
        }

        public static Game GetGameById(int id)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/games/info?id={id}");
            var encoding = Encoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            return new Game(int.Parse(dict["ID"]), dict["Name"], dict["Description"], dict["Active"] == "true", int.Parse(dict["CreatorID"]), DateTime.ParseExact(dict["CreatedAt"], "MM-dd-yyyy hh:mm tt", CultureInfo.InvariantCulture), DateTime.ParseExact(dict["UpdatedAt"], "MM-dd-yyyy hh:mm tt", CultureInfo.InvariantCulture));
        }

        public int id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public bool active { get; private set; }
        public int creator_id { get; private set; }
        public DateTime creation_date { get; private set; }
        public DateTime last_update { get; private set; }

    }
}
