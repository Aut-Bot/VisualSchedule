namespace AutBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;

    [LuisModel("950f6fb1-1e16-4478-a881-6e99ac10d1d2", "23c46a5193774e40a28d8f0fa451964a")]
    [Serializable]
    public class RootLUISDialog : LuisDialog<object>
    {
        string name;
        DateTime schedule;
        private const string Yes = "Yes";
        private const string No = "No";
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am Aut-Bot, try asking me to add events for your visual Schedule");
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Addtile")]
        public async Task Schedule(IDialogContext context, LuisResult result)
        {
            //await context.PostAsync("I will add a schedule based on entered text "+ result.Query);
            //context.Wait(this.MessageReceived);
            name = string.Empty;
            schedule = DateTime.Now;
            foreach (var entity in result.Entities)
            {
                switch (entity.Type)
                {
                    case "Name":
                        break;
                    case "builtin.datetimeV2.datetime":
                        break;
                }
            }
            this.ShowOptions(context);

        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { Yes, No }, "Do you want to add schedule named {Name} for {Date}", "Not a valid option", 3);
        }
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case Yes:
                        context.Call(new AddTileDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case No:
                        context.Wait(this.MessageReceived);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
                context.Wait(this.MessageReceived);
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceived);
            }
        }


    }
}
