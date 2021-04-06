using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace PolytoriaTools
{
    class Guild
    {
        public Guild(int id, string name, string description, bool verified, int owner_id, DateTime creation_date)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.verified = verified;
            this.owner_id = owner_id;
            this.creation_date = creation_date;
        }

        public static Guild GetGuildById(int id)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/guild/info?id={id}");
            var encoding = Encoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            return new Guild(int.Parse(dict["ID"]), dict["Name"], dict["Description"], dict["Verified"]=="true", int.Parse(dict["OwnerID"]), DateTime.ParseExact(dict["CreatedAt"], "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture));
        }

        public User GetGuildOwner()
        {
            return User.GetUserById(owner_id);
        }

        public int id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public bool verified { get; private set; }
        public int owner_id { get; private set; }
        public DateTime creation_date { get; private set; }
    }
}
