using Newtonsoft.Json;

namespace Crumblin__Bot
{
    public static class SubscribedGuilds
    {
        public static List<SubscribedGuild> Guilds { get; set; } 
        private const string SUB_LIST_FILENAME = "subscribedGuilds.json";

        static SubscribedGuilds()
        {
            // Initialize a new list of Guilds
            Guilds = new List<SubscribedGuild>();

            // If we don't have a file to pull from, create it.
            if (!File.Exists(SUB_LIST_FILENAME))
                SaveGuilds();

            // Deserialize the list from file.
            Guilds = JsonConvert.DeserializeObject<List<SubscribedGuild>>(File.ReadAllText(SUB_LIST_FILENAME));
        }

        public static void SaveGuilds()
        {
            // Serialize the list into a JSON file and save it.
            string jsonText = JsonConvert.SerializeObject(Guilds);
            File.WriteAllText(SUB_LIST_FILENAME, jsonText);
        }
    }
}
