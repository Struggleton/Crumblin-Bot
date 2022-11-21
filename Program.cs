using Crumblin__Bot.Commands;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Quartz;
using Quartz.Impl;

namespace Crumblin__Bot
{
    internal class Program
    {
        private const string TOKEN_FILENAME = "token.txt";

        private List<IGuildCommand> GuildCommands;
        private DiscordSocketClient _client;

        // Do this so we can be in an async context
        public static Task Main(string[] args) => new Program().MainAsync();

        public Program()
        {
            // Get all implemented IGuildCommand classes
            var type = typeof(IGuildCommand);
            var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            // Instantiate the GuildCommands
            GuildCommands = types.Select(t => Activator.CreateInstance(t) as IGuildCommand).ToList();
        }


        public async Task MainAsync()
        {
            // Create a new DiscordClient and subscribe to
            // our events.
            DiscordSocketConfig config = new()
            {
                UseInteractionSnowflakeDate = false
            };

            _client = new DiscordSocketClient(config);
            _client.SlashCommandExecuted += Client_SlashCommandHandler;
            _client.Ready += Client_Ready;
            _client.Log += Log;
            
            // Load token from file
            var token = File.ReadAllText(TOKEN_FILENAME);

            // Log into the Discord bot and start.
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(Timeout.Infinite);
        }

        private async Task PostMenuWeekly()
        {
            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // Create a new Job to post cookie updates.
            IJobDetail job = JobBuilder.Create<MenuJob>()
            .WithIdentity(name: "cookieUpdates", group: "baseGroup")
            .Build();

            // Pass the Discord client to the Job we created
            job.JobDataMap.Put("client", _client);

            // Create a cronTrigger to trigger the job
            // Every monday at 10.
            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "baseGroup")
            .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 10 ? * MON *"))
            .Build();

            // Schedule the job and start it.
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        private async Task Client_SlashCommandHandler(SocketSlashCommand command)
        {
            // Get the relevant command interface 
            IGuildCommand commandType = GuildCommands.FirstOrDefault(x => x.CommandName == command.Data.Name);

            // If the command exists, execute it.
            if (commandType != null) 
            {
                Task.Run(async () => commandType.HandleCommand(command));
            }
        }

        private async Task Client_Ready()
        {
            // Get all the guilds we are currently connected to.
            Task.Run(async () => {
            foreach (var guild in _client.Guilds)
            {
                // Pull all of the commands so we can create a SlashCommand for each
                foreach (IGuildCommand command in GuildCommands)
                {
                    // Create a guild command
                    SlashCommandBuilder guildCommand = new SlashCommandBuilder();

                    // Assign properties to it
                    guildCommand.WithName(command.CommandName);
                    guildCommand.WithDescription(command.CommandDescription);


                    SlashCommandBuilder globalCommand = new SlashCommandBuilder();
                    // Assign properties to it
                    globalCommand.WithName(command.CommandName);
                    globalCommand.WithDescription(command.CommandDescription);
                    // Try to build and create application command
                    try
                    {
                        await guild.CreateApplicationCommandAsync(guildCommand.Build());
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                    }

                    catch (ApplicationCommandException exception)
                    {
                        // Print the exception to the console
                        Log(new LogMessage(LogSeverity.Error, command.CommandName, exception.Message, exception));
                    }
                }
            }

            // Schedule weekly cookie updates to 
            // Guilds that are subscribed them.
            PostMenuWeekly();
            });
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}