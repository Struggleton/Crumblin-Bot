using Discord.WebSocket;

namespace Crumblin__Bot.Commands
{
    public class SubscribeCommand : IGuildCommand
    {
        public string CommandName { get; set; } = "subscribe";
        public string CommandDescription { get; set; } = "Set this channel for updates on Crumbl Cookie menus.";

        public SubscribeCommand() 
        { 

        }

        public async Task HandleCommand(SocketSlashCommand command)
        {
            // Pull the channel/guild ids from the command
            ulong GuildID = (ulong)command.GuildId;
            ulong ChannelID = (ulong)command.ChannelId;

            // Check to see if the guild/channel exists in the list.
            bool serverExists = SubscribedGuilds.Guilds.Any(x => x.GuildID == GuildID);
            bool channelExists = SubscribedGuilds.Guilds.Any(x => x.ChannelID == ChannelID);

            // If either doesn't exist in the list, create a new entry and add it.
            if (!serverExists || !channelExists)
            {
                SubscribedGuild subscribedGuild = new SubscribedGuild();
                subscribedGuild.ChannelID = ChannelID;
                subscribedGuild.GuildID = GuildID;

                SubscribedGuilds.Guilds.Add(subscribedGuild);
                SubscribedGuilds.SaveGuilds();

                // Post reply to command.
                await command.RespondAsync($"Subscribed to cookie menu updates in #{command.Channel.Name}! " +
                                           $"Menus are posted every Monday at 10AM EST!");
            }
            
            // We're already subscribed, tell the user.
            else
            {
                await command.RespondAsync("This channel is already subscribed to cookie updates!");
            }
            
        }

    }
}
