# Overview 
This application is about helping build expectations for the future and making it easy for parents to add new event images & descriptions for visits to say new therapists/doctors/people or for visiting new places like the aquarium in order to build out daily and weekly visual schedules for their child.

# Description
Parents and care givers can access the Aut-bot scheduler through a web app built using the Microsoft Bot Framework.
The user makes a request to the bot, through text or speech, to add an item to their child's schedule.
Requests are processsed using LUIS, to classify the tile parameters (event, date & time).
If parameters are missing, the bot will prompt the user to clarify their request to specifiy any of event, date or time.
The bot then prompts the user for an image, either via an image upload, image library, or Bing image search.
    - If the user selects image upload, they will be prompted to upload an image.
    - If the user selects image library, the will be prompted with pictures that match the event they described in their original request.
    - If the user selects Bing image search, the bot will connect to the Bing Image Getter API and suggest 3 images that might match the event described in the original request. The user then selects one of the three images.
Once an image has been selected, the bot prompts the user to add or edit the image tags.
After that, the activity tile is generated and pushed into the SQL database.
The database is connected to the schedule server, which populates the tiles into the web app.
When the child opend the scheduler web app, they will see the tiles displayed under the appropriate day of the week.