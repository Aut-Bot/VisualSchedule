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
using System.Net;
using System.Collections.Specialized;
using System.Net.Http;


namespace AutBot
{   
    
    [Serializable]
    public class AddTileDialog :IDialog<object>
    {   
        string desccription { get; set; }
        string timeslot { get; set; }
        public AddTileDialog(string name, string timeslot)
        {
            this.desccription = name;
            this.timeslot = timeslot;

        }
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
                //Call the API
                using (var client = new HttpClient())
                {
                    Schedule p = new Schedule { timeSlot = this.timeslot, description = this.desccription };
                    client.BaseAddress = new Uri("http://visualscheduler.azurewebsites.net");
                    var response = client.PostAsJsonAsync("api/Calendar",p).Result;

                }

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