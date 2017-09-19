namespace AutBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;
    [Serializable]
    public class TileQuery
    {
        [Prompt("Please enter  {&}")]
        public string Name { get; set; }
        [Prompt("When do you want to {&}?")]
        public DateTime Schedule { get; set; }

        
    }
}