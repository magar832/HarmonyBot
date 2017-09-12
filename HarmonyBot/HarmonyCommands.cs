using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace HarmonyBot
{
    public class HarmonyCommands : ModuleBase
    {
        private CommandService _service;

        public HarmonyCommands(CommandService service)
        {
            _service = service;
        }

        [Command("hello")]
        [Summary("Say hello")]
        [Alias("hi")]
        public async Task Hello()
        {
            await ReplyAsync("hello! " + Context.Message.Author.Mention);
        }

        [Command("help")]
        [Summary("Sends Command List")]
        public async Task Help()
        {
            //EmbedBuilder builder = new EmbedBuilder() { Color = new Color(114, 137, 218) };
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("List of Commands: ");
            foreach (var module in _service.Modules)
            {
                foreach (var command in module.Commands)
                {
                    var result = await command.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        sb.AppendLine(string.Format("\t!{0} - {1}", command.Aliases.First(), command.Summary));
                }
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("sm")]
        [Summary("Shows StepMania scores according to UID")]
        public async Task StepManiaScores(string uid)
        {
            await ReplyAsync(StepmaniaParser.StepMania(uid));
        }

        [Command("holycrap")]
        [Summary("Hey 🅱eter")]
        public async Task FreeiPhone()
        {
            await Context.Channel.SendFileAsync("images/holycrap.jpg");
        }

        [Command("flip")]
        [Summary("Flips a coin!")]
        public async Task CoinFlip()
        {
            if (RndNumGen.Roll(2) == 1)
                await Context.Channel.SendFileAsync("images/egghead.jpg");
            else await Context.Channel.SendFileAsync("images/Tails.png");
        }

        [Command("roll")]
        [Summary("Rolls an x-sided die!")]
        public async Task DieRoll(string sides)
        {
            if (!(sides.StartsWith("d")))
            {
                await ReplyAsync("Please prefix your number with the d");
                return;
            }

            sides = sides.TrimStart('d');
            int num;
            try
            {
                num = Convert.ToInt32(sides);
                if (num < 1)
                {
                    await ReplyAsync("Cannot be negative!");
                    return;
                }
            }
            catch (Exception e) { await ReplyAsync("Invalid numero"); return; }
            await ReplyAsync("Rolled a " + RndNumGen.Roll(num));
        }

        [Command("who")]
        [Summary("Who'm'st've'd")]
        [Alias("who's", "whos")]
        public async Task Who(params string[] args)
        {


            StringBuilder sb = new StringBuilder();
            var users = (await Context.Guild.GetUsersAsync()).ToList().Where(x => !x.IsBot).ToList();

            var user = users[RndNumGen.rng.Next(0, users.Count())];
            sb.Append(user.Username);
            if (Context.Message.Content.StartsWith("!who's") || Context.Message.Content.StartsWith("!whos"))
            {
                sb.Append(" is");
            }
            var size = args.Length;
            for (int i = 0; i < size; i++)
            {
                var tmp = " " + args[i] + " ";
                tmp = tmp.Replace("?", "");
                tmp = tmp.Replace(" me ", "you");
                tmp = tmp.Replace(" my ", "your");
                tmp = tmp.Replace(" your ", "my");
                tmp = tmp.Replace(" you ", "me");
                tmp = tmp.Trim(' ');
                sb.Append(" " + tmp);
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("hours")]
        [Summary("What time is it?")]
        [Alias("hours?")]
        public async Task RealHours()
        {
            var channels = (await Context.Guild.GetVoiceChannelsAsync()).ToList();
            var count = channels.Count();
            var channel = channels[RndNumGen.rng.Next(0, count)];
            var hour = System.DateTime.Now.Hour;
            string princess;
            string conclusion;
            //await ReplyAsync("The current hour is " + hour);
            if (hour >= 21 || hour < 6)
            {
                princess = "🌙 Luna: \"";
                conclusion = ", even though you should be asleep right now.\"";
            }
            else {
                princess = "☀ Celestia: \"";
                conclusion = ", although playing in my sunlight could be just as fun.\"";
            };
            await ReplyAsync(princess + "I think its real " + channel.Name + " hours" + conclusion);
        }

        /*[Command("Suggest")]
        [Summary("Submit a command suggestion")]
        public async Task Suggest(string command, string summary)
        {
            
        }*/
    }
}
