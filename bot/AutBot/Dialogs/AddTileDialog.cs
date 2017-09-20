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
using Newtonsoft.Json.Linq;

namespace AutBot
{
    [Serializable]
    public class AddTileDialog :IDialog<object>
    {
        bool isSearch = false;
       
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Okay! let's build the schedule");
            this.ShowOptions(context);
           // context.Done<object>(null);
           
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { "Search", "Upload" }, "How do you want to add image to the activity?", "Not a valid option", 3);
        }
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case "Upload":
                        context.Call(new ReceiveAttachmentDialog(), this.ResumeAfterImageDialog);
                        break;

                    case "Search":
                        //context.Wait(this.MessageReceived);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
                context.Done<object>(null);
            }
        }
        private async Task ResumeAfterImageDialog(IDialogContext context, IAwaitable<object> result)
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
                //context.Wait(mes);
                context.Done<object>(null);
            }
        }

    }
}