using Discord.WebSocket;

namespace Crumblin__Bot.Commands
{
    public class UnsubscribeCommand : IGuildCommand
    {
        public string CommandName { get; set; } = "unsubscribe";
        public string CommandDescription { get; set; } = "Unsubscribe from menu posts in this channel.";

        public UnsubscribeCommand()
        {

        }

        public async Task HandleCommand(SocketSlashCommand command)
        {
            // Get the guild/channel ID from the command.
            ulong GuildID = (ulong)command.GuildId;
            ulong ChannelID = (ulong)command.ChannelId;

            // Check to see if the guild is in the subscription list.
            int index = SubscribedGuilds.Guilds.FindIndex(x => x.GuildID == GuildID 
                                                           && x.ChannelID == ChannelID);

            // If the guild exists, remove it from the list and update the file.
            // If not tell the user that it does not exist in the list.
            if (index >= 0) 
            {
                SubscribedGuilds.Guilds.RemoveAt(index);
                SubscribedGuilds.SaveGuilds();

                command.RespondAsync("This channel will no longer receive cookie updates.");
            }

            else
            {
                command.RespondAsync("This channel is not subscribed to cookie menu updates.");
            }
        }
    }
}
