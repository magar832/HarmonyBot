using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net.WebSockets;
using System.Reflection;
namespace HarmonyBot
{
    public class Program
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IServiceProvider services;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        //Async Main: Start Here
        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            
            string token = "//token here"; //Keep Private!

            services = new ServiceCollection()
                .BuildServiceProvider();

            await InitCommands();

            await client.LoginAsync(TokenType.Bot, token); //login to discord
            await client.StartAsync(); //start the async
            client.Log += Log;
            commands.Log += Log;
            //Run infinitely
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            //Can be improved with a switch
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        
   

        public async Task InitCommands()
        {
            //Hook MessRec event 
            client.MessageReceived += HandleCommand;
            //Look for commands in the assembly and load them
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage msg)
        {
            // Don't process the command if it was a System Message
            var message = msg as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 1;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await commands.ExecuteAsync(context, argPos /*service*/);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
