using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PolytoriaTools
{
    class Asset
    {
        public Asset(int id, string name, string description, int creator, string creator_type, string currency, int price, int alt_price, int time_created, int sales, int favorites, string type, string moderation_status, bool is_limited, int total_stock, int time_updated, int onsale_until, int trade_value, int version)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.creator = creator;
            this.creator_type = creator_type;
            this.currency = currency;
            this.price = price;
            this.alt_price = alt_price;
            this.time_created = time_created;
            this.sales = sales;
            this.favorites = favorites;
            this.type = type;
            this.moderation_status = moderation_status;
            this.is_limited = is_limited;
            this.total_stock = total_stock;
            this.time_updated = time_updated;
            this.onsale_until = onsale_until;
            this.trade_value = trade_value;
            this.version = version;
        }

        public override string ToString()
        {
            return name;
        }

        public static Asset GetAssetById(int id)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/asset/info?id={id}");
            var encoding = ASCIIEncoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);
            
            string responseText = reader.ReadToEnd();
            return JSONStringToAsset(responseText);
        }

        public static Asset GetHatByName(string name)
        {
            return GetAssetsFromCatalog("hat", 0, name, 1)[0];
        }

        public static Asset GetToolByName(string name)
        {
            return GetAssetsFromCatalog("tool", 0, name, 1)[0];
        }

        private static Asset JSONStringToAsset(string jsonstring)
        {
            Dictionary<string, string> control_dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonstring);
            return new Asset(Int32.Parse(control_dict["id"]), control_dict["name"], control_dict["description"],
                Int32.Parse(control_dict["creator"]), control_dict["creator_type"], control_dict["currency"],
                Int32.Parse(control_dict["price"]), Int32.Parse(control_dict["price_alt"]), Int32.Parse(control_dict["time_created"]),
                Int32.Parse(control_dict["sales"]), Int32.Parse(control_dict["favourites"]), control_dict["type"], control_dict["moderation_status"],
                Int32.Parse(control_dict["is_limited"]) == 1, Int32.Parse(control_dict["total_stock"]), Int32.Parse(control_dict["time_updated"]),
                Int32.Parse(control_dict["onsale_until"]), Int32.Parse(control_dict["value"]), Int32.Parse(control_dict["version"]));
        }

        private static Asset DictionaryToAsset(Dictionary<string, string> control_dict)
        {
            return new Asset(Int32.Parse(control_dict["id"]), control_dict["name"], control_dict["description"],
                Int32.Parse(control_dict["creator"]), control_dict["creator_type"], control_dict["currency"],
                Int32.Parse(control_dict["price"]), Int32.Parse(control_dict["price_alt"]), Int32.Parse(control_dict["time_created"]),
                Int32.Parse(control_dict["sales"]), Int32.Parse(control_dict["favourites"]), control_dict["type"], control_dict["moderation_status"],
                Int32.Parse(control_dict["is_limited"]) == 1, Int32.Parse(control_dict["total_stock"]), Int32.Parse(control_dict["time_updated"]),
                Int32.Parse(control_dict["onsale_until"]), Int32.Parse(control_dict["value"]), Int32.Parse(control_dict["version"]));
        }

        /// <summary>
        /// Searches for the given criteria in the Polytoria Catalog. Format the query string before passing to query parameter.
        /// </summary>
        /// <param name="type">Asset type (hat, shirt, etc)</param>
        /// <param name="catalog_page">Page of the catalog. Indexing starts from 0.</param>
        /// <param name="query">Formated query string</param>
        /// <param name="assets_per_page">How many objects will be returned at max.</param>
        /// <returns>Array of assets suiting the given criteria</returns>
        public static Asset[] GetAssetsFromCatalog(string type = "hat", int catalog_page = 0, string query = "Jester%20Hat", int assets_per_page = 50)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/asset/catalog?type={type}&page={catalog_page}&q={query}&limit={assets_per_page}");
            var encoding = ASCIIEncoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string>[] dicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(responseText);
            Asset[] assets = new Asset[dicts.Length];
            for (int i = 0; i < dicts.Length; i++)
            {
                assets[i] = DictionaryToAsset(dicts[i]);
            }
            return assets;
        }

        public int ApproxPriceConvert(float ratio = 25f)
        {
            if (currency == "Bricks")
            {
                return (int)(price * ratio);
            } else
            {
                return (int)(price / ratio);
            }
        }

        public static Asset GetMostExpensiveAsset(Asset[] assets)
        {
            Asset expensive = assets[0];
            foreach(Asset asset in assets)
            {
                if(asset.currency == expensive.currency)
                {
                    if(asset.price > expensive.price)
                    {
                        expensive = asset;
                    }
                } else
                {
                    if(asset.ApproxPriceConvert() > expensive.price)
                    {
                        expensive = asset;
                    }
                }
            }
            return expensive;
        }

        #region properties
        public int id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public int creator { get; private set; }
        public string creator_type { get; private set; }
        public string currency { get; private set; }
        public int price { get; private set; }
        public int alt_price { get; protected set; }
        public int time_created { get; private set; }
        public int sales { get; private set; }
        public int favorites { get; private set; }
        public string type { get; private set; }
        public string moderation_status { get; private set; }
        public bool is_limited { get; private set; }
        public int total_stock { get; private set; }
        public int time_updated { get; private set; }
        public int onsale_until { get; private set; }
        public int trade_value { get; private set; }
        public int version { get; private set; }

        #endregion

    }

    class Limited
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public int price { get; private set; }
        public string currency { get; private set; }
        public string type { get; private set; }
        public int value { get; private set; }
        public int stock { get; private set; }
        public bool sold_out { get; private set; }
        public int best_price { get; private set; }

        public Limited(int id, string name, string description, int price, string currency, string type, int value, int stock, bool soldout, int best_price)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
            this.currency = currency;
            this.type = type;
            this.value = value;
            this.stock = stock;
            sold_out = soldout;
        }

        private static Limited DictionaryToLimited(Dictionary<string, string> control_dict)
        {
            return new Limited(Int32.Parse(control_dict["AssetID"]), control_dict["Name"], control_dict["Description"],
                Int32.Parse(control_dict["Price"]), control_dict["Currency"], control_dict["Type"],
                Int32.Parse(control_dict["Value"]), Int32.Parse(control_dict["Stock"]), control_dict["SoldOut"]=="true",
                Int32.Parse(control_dict["BestPrice"]));
        }

        public static Limited[] GetRecentLimiteds(int catalog_page = 0, int limit = 5)
        {
            WebRequest req = WebRequest.Create($"http://api.polytoria.com/asset/limiteds?page={catalog_page}&limit={limit}");
            var encoding = ASCIIEncoding.ASCII;
            var reader = new System.IO.StreamReader(req.GetResponse().GetResponseStream(), encoding);

            string responseText = reader.ReadToEnd();
            Dictionary<string, string>[] dicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(responseText);
            Console.WriteLine(dicts[0]["AssetID"]);
            Limited[] assets = new Limited[dicts.Length];
            for (int i = 0; i < dicts.Length; i++)
            {
                assets[i] = DictionaryToLimited(dicts[i]);
            }
            return assets;
        }
    }
}
