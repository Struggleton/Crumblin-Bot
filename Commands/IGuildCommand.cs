using Discord.WebSocket;

namespace Crumblin__Bot.Commands
{
    public interface IGuildCommand
    {
        string CommandName { get; set; }
        string CommandDescription { get; set; }

        Task HandleCommand(SocketSlashCommand command);
    }
}
