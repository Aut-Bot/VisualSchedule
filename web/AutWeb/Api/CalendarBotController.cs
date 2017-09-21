using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutWeb.Data;
using AutWeb.Models.AutModels;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure;

namespace AutWeb.Api
{
    [Produces("application/json")]
    [Route("api/CalendarBot")]
    public class CalendarBotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarBotController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Calendar
        [HttpPost]
        public async Task<IActionResult> PostCalendarItemWithImage(
            [FromBody] CalendarItemWithImage calendarItemWithImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Is image included?
            bool imageIncluded = (calendarItemWithImage.ItemImage != null);
            
            if (imageIncluded)
            {
                // store in blob
                var storedBlobUri = SaveToBlob(
                    calendarItemWithImage.Description, calendarItemWithImage.ItemImage);
                calendarItemWithImage.ImageUrl = storedBlobUri;

                // store URI for DB record
                calendarItemWithImage.Description = "[IMG-UP]" + calendarItemWithImage.Description;
            } else
            {
                // no image in request
                calendarItemWithImage.Description = "[NO-IMG]" + calendarItemWithImage.Description;
                // get image via Bing image search
                // TODO: 
                // store Bing image URI for DB record
                // TODO: 
            }

            // create calendarItem
            var calendarItem = new CalendarItem
            {
                Description = calendarItemWithImage.Description,
                ImageUrl = calendarItemWithImage.ImageUrl,
                TimeSlot = calendarItemWithImage.TimeSlot
            };

            // insert DB calendar item (calendarItem.Id, TimeSlot, ImageUrl, Description)
            _context.CalendarItems.Add(calendarItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCalendarItem", new { id = calendarItem.Id }, calendarItem);
            return Ok(calendarItem);
        }

        private string SaveToBlob(string blobName, byte[] blobData)
        {
            var storageConn = "DefaultEndpointsProtocol=https;AccountName=STORAGE_ACCOUNT_NAME;AccountKey=ACCOUNT_KEY_VALUE;EndpointSuffix=core.windows.net";

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(storageConn));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("photo");

            //Create a new container, if it does not exist
            //var exists = container.CreateIfNotExists(); // doesn't work with .NET Core?

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            //// OPTIONAL: set public permissions
            //container.SetPermissions(
            //    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Upload Blob From Byte Array (Async)
            blockBlob.UploadFromByteArrayAsync(blobData, 0, blobData.Length - 1);
            
            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
        }
    }
}