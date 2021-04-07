using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace PolytoriaTools
{
    class UserProto
    {
        public int id { get; protected set; }
        public string username { get; protected set; }

        public UserProto(int id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
    class User : UserProto
    {
        public User(string username, int id, string description, string avatar_hash, string rank, string membership, int time_joined, int last_seen, DateTime date_joined, DateTime date_last_seen, int trade_value) : base(id, username)
        {
            this.username = username;
            this.id = id;
            this.description = description;
            this.avatar_hash = avatar_hash;
            this.rank = rank;
            this.membership = membership;
            this.time_joined = time_joined;
            this.last_seen = last_seen;
            this.date_joined = date_joined;
            this.date_last_seen = date_last_seen;
            this.trade_value = trade_value;
        }

        public static User MostActiveUser(User[] users)
        {
            User mostactive = users[0];
            foreach (User user in users)
            {
                if (!(DateTime.Compare(mostactive.date_last_seen, user.date_last_seen) > 0))
                {
                    mostactive = user;
                }
            }
            return mostactive;
        }

        public static User GetUserById(int id)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/users/user?id={id}");
            var encoding = Encoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            return new User(dict["Username"], int.Parse(dict["ID"]), dict["Description"], dict["AvatarHash"], dict["Rank"], 
                dict["MembershipType"], int.Parse(dict["TimeJoined"]), int.Parse(dict["LastSeen"]), 
                DateTime.ParseExact(dict["DateJoined"], "MM-dd-yyyy", null), 
                DateTime.ParseExact(dict["DateLastSeen"], "MM-dd-yyyy", null), int.Parse(dict["TradeValue"]));
        }

        public static User GetUserByUsername(string username)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/users/getbyusername?username={username}");
            var encoding = Encoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            return new User(dict["Username"], int.Parse(dict["ID"]), dict["Description"], dict["AvatarHash"], dict["Rank"],
                dict["MembershipType"], int.Parse(dict["TimeJoined"]), int.Parse(dict["LastSeen"]),
                DateTime.ParseExact(dict["DateJoined"], "mm-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                DateTime.ParseExact(dict["DateLastSeen"], "mm-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture), int.Parse(dict["TradeValue"]));
        }

        public static UserProto[] GetUserProtoFriends(int mainUserId)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/users/friends?id={mainUserId}");
            var encoding = Encoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string>[] dicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(responseText);
            UserProto[] users = new UserProto[dicts.Length];
            for (int i = 0; i < dicts.Length; i++)
            {
                users[i] = new UserProto(int.Parse(dicts[i]["id"]), dicts[i]["username"]);
            }
            return users;
        }

        public static User[] GetUserFriends(int mainUserId)
        {
            return GetUserFromUserProto(GetUserProtoFriends(mainUserId));
        }

        public static User GetUserFromUserProto(UserProto user)
        {
            return GetUserById(user.id);
        }

        public static User[] GetUserFromUserProto(UserProto[] users)
        {
            User[] newUsers = new User[users.Length];
            for (int i = 0; i < users.Length; i++)
            {
                newUsers[i] = GetUserFromUserProto(users[i]);
            }
            return newUsers;
        }

        public static UserAppearance GetUserAppearance(int userId)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/users/getappearance?id={userId}");
            var encoding = ASCIIEncoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            return new UserAppearance(dict["hash"], int.Parse(dict["head_color"]), int.Parse(dict["torso_color"]),
                int.Parse(dict["leftarm_color"]), int.Parse(dict["rightarm_color"]), int.Parse(dict["leftleg_color"]),
                int.Parse(dict["rightleg_color"]), int.Parse(dict["hatid1"]), int.Parse(dict["hatid2"]), int.Parse(dict["hatid3"]),
                int.Parse(dict["faceid"]), int.Parse(dict["toolid"]), int.Parse(dict["shirtid"]), int.Parse(dict["pantsid"]),
                int.Parse(dict["headid"]), int.Parse(dict["tshirtid"]));

        }

        public UserAppearance GetUserAppearance()
        {
            return User.GetUserAppearance(id);
        }

        public User[] GetUserFriends()
        {
            return User.GetUserFriends(id);
        }

        public User NextUser(int incr = 1)
        {
            return User.GetUserById(id + incr);
        }

        public override string ToString()
        {
            return username;
        }

        #region properties

        public string description { get; private set; }
        public string avatar_hash { get; private set; }
        public string rank { get; private set; }
        public string membership { get; private set; }
        public int time_joined { get; private set; }
        public int last_seen { get; private set; }
        public DateTime date_joined { get; private set; }
        public DateTime date_last_seen { get; private set; }
        public int trade_value { get; private set; }

        #endregion
    }
}
