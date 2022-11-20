![My cute crumbl cookie icon](https://raw.githubusercontent.com/Struggleton/Crumblin-Bot/master/icon.png)

# Crumblin' Bot
A Discord bot that pulls cookie menus from Crumbl Cookie (the best cookie place ever!) and posts them in nice little embeds on Discord.

I wrote this as a little challenge to myself - I learned a lot about interfaces, some HTTP calls and some scheduling stuff. This bot does some cool stuff with reflection to pull implemented interface classes and automates a lot :)

Commands are:

## /subscribe
	Subscribe channel to the weekly menu posting.
	
## /unsubscribe
	Unsubscribe from the weekly menu posting.
	
## /menu
	Post this week's cookie menu!
	
The bot will post cookie menus every Monday at 10am.

Libraries used include:

    * Discord.NET
    * Quartz