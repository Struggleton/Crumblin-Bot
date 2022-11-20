using Crumblin__Bot.Commands;
using Discord;
using Discord.WebSocket;
using Quartz;

namespace Crumblin__Bot
{
    /// <summary>
    /// A job to send cookie menu updates to subscribed guilds.
    /// </summary>
    public class MenuJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // Update the on file page properties. The menu 
            // has most likely updated
            await CookieMenu.UpdatePageProperties();

            // Pull the discord client out of the provided dictionary.
            DiscordSocketClient client = (DiscordSocketClient)context.MergedJobDataMap["client"];

            foreach (SubscribedGuild guild in SubscribedGuilds.Guilds)
            {
                SocketGuild server = client.GetGuild(guild.GuildID);
                if (server != null)
                {
                    SocketTextChannel channel = server.GetTextChannel(guild.ChannelID);

                    // Create the main message
                    EmbedBuilder mainMessageEmbed = MenuCommand.CreateMainMessageEmbed();

                    // Post the video associated with the Crumbl cookie website.
                    await channel.SendMessageAsync(CookieMenu.PageProperties.Urls.Desktop);

                    // Send the message with our created embed.
                    await channel.SendMessageAsync(embed: mainMessageEmbed.Build());

                    // Wait so people can read the embed
                    await Task.Delay(1000);

                    // Get the list of cookie embeds and send them.
                    List<EmbedBuilder> cookieEmbeds = await CookieMenu.CreateCookieEmbeds();
                    foreach (EmbedBuilder embed in cookieEmbeds)
                    {
                        // Send cookie embed and build it.
                        await channel.SendMessageAsync(embed: embed.Build());

                        // Wait 1.5 seconds for people to be able to read it.
                        await Task.Delay(1500);
                    }

                    // Create a follow up message and send it.
                    EmbedBuilder followUpEmbed = MenuCommand.CreateFollowUpEmbed();
                    await channel.SendMessageAsync(embed: followUpEmbed.Build());
                }
            }
        }
    }
}
