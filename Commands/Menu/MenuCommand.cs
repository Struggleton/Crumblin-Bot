using Discord;
using Discord.WebSocket;
namespace Crumblin__Bot.Commands
{
    public class MenuCommand : IGuildCommand
    {
        public string CommandName { get; set; } = "menu";
        public string CommandDescription { get; set; } = "Displays the current Crumbl Cookie menu.";

        public MenuCommand()
        {

        }

        public async Task HandleCommand(SocketSlashCommand command)
        {
            // Create the main message
            EmbedBuilder mainMessageEmbed = CreateMainMessageEmbed();

            // Post the video associated with the Crumbl cookie website.
            await command.RespondAsync(CookieMenu.PageProperties.Urls.Desktop);

            // Send the message with our created embed.
            await command.Channel.SendMessageAsync(embed: mainMessageEmbed.Build());

            // Wait so people can read the embed
            await Task.Delay(1000);

            // Get the list of cookie embeds and send them.
            List<EmbedBuilder> cookieEmbeds = await CookieMenu.CreateCookieEmbeds();
            foreach (EmbedBuilder embed in cookieEmbeds)
            {
                // Send cookie embed and build it.
                await command.Channel.SendMessageAsync(embed: embed.Build());

                // Wait 1.5 seconds for people to be able to read it.
                await Task.Delay(1500);
            }

            // Create a follow up message and send it.
            EmbedBuilder followUpEmbed = CreateFollowUpEmbed();
            await command.Channel.SendMessageAsync(embed: followUpEmbed.Build());
        }

        public static EmbedBuilder CreateMainMessageEmbed()
        {
            EmbedBuilder mainMessageEmbed = new EmbedBuilder();
            mainMessageEmbed.WithTitle("Cookie menus inbound!")
                  .WithDescription("Get ready for goodness - This is this week's cookie menu!")
                  .WithFooter(footer => footer.Text = "Bot written by @Struggleton.")
                  .WithUrl("https://crumblcookies.com/")
                  .WithColor(Color.LightOrange)
                  .WithCurrentTimestamp();

            return mainMessageEmbed;
        }

        public static EmbedBuilder CreateFollowUpEmbed()
        {
            EmbedBuilder followUpEmbed = new EmbedBuilder();
            followUpEmbed.WithTitle("Cookie menu posted!")
                  .WithDescription("What cookies are you about to get this week?")
                  .WithFooter(footer => footer.Text = "Bot written by @Struggleton.")
                  .WithUrl("https://crumblcookies.com/")
                  .WithColor(Color.LightOrange)
                  .WithCurrentTimestamp();

            return followUpEmbed;
        }
    }
}
