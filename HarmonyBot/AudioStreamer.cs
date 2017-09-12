using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmonyBot
{
    class AudioStreamer : ModuleBase
    {
        private CommandService _service;

        public AudioStreamer(CommandService service)
        {
            _service = service;
        }

        [Command("m.yt")]
        [Summary("Streams YouTube audio to your channel!")]
        public async Task Stream(string url)
        {
            
            await ReplyAsync("Playing song!");
        }
    }
}
