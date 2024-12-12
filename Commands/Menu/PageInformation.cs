﻿using Discord;
using HtmlAgilityPack;
using Quartz.Util;
using System.Text.Json;

namespace Crumblin__Bot
{
    // These classes were generated by a script
    // from JSON data pulled from https://crumblcookies.com
    // to C# classes.

    public class AllergyInformation
    {
        public string? Description { get; set; }
    }

    public class CalorieInformation
    {
        public string? PerServing { get; set; }

        public string? Total { get; set; }

        public string? Typename { get; set; }
    }

    public class Cookie
    {
        public string? Id { get; set; }

        public string? CookieId { get; set; }

        public string? Status { get; set; }

        public string? NameWithoutPartner { get; set; }

        public string? Name { get; set; }

        public string? AerialImage { get; set; }

        public string? NewAerialImage { get; set; }

        public string? Description { get; set; }

        public string? IconImage { get; set; }

        public bool IsMysteryCookie { get; set; }

        public string? FeaturedPartner { get; set; }

        public string? FeaturedPartnerLogo { get; set; }

        public bool? NewRecipeCallout { get; set; }

        public CalorieInformation? CalorieInformation { get; set; }
    }

    /*
    public class Cookie
    {
        public string? ID { get; set; }
        public string? NameWithoutPartner { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? NewImage { get; set; }
        public string? Description { get; set; }
        public string? AerialImage { get; set; }
        public string? IconImage { get; set; }
        public bool? IsMysteryCookie { get; set; }
        public object? FeaturedPartner { get; set; }
        public object? FeaturedPartnerLogo { get; set; }
        public bool? NewRecipeCallout { get; set; }
        public AllergyInformation? AllergyInformation { get; set; } = new AllergyInformation();
        public CalorieInformation? CalorieInformation { get; set; } = new CalorieInformation();
    }
    */

    public class Icecream
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }

    public class PageProps
    {
        public Urls? Urls { get; set; }
        public Products? Products { get; set; }
    }

    public class Products
    {
        public List<Cookie> Cookies { get; set; }
        public List<Icecream> Icecream { get; set; }
    }


    public class Props
    {
        public PageProps? PageProps { get; set; }
        public bool __N_SSG { get; set; }
    }

    public class Root
    {
        public Props? Props { get; set; }
    }

    public class Urls
    {
        public string? Desktop { get; set; }
    }

    public static class CookieMenu
    {
        private const string REQUEST_LINK = "https://crumblcookies.com/";
        public static PageProps PageProperties { get; set; } = new PageProps();

        // Update pageproperties on init
        static CookieMenu() => UpdatePageProperties();

        public static async Task UpdatePageProperties()
        {
            PageProperties = await GetCookieMenu();
        }

        public static async Task<PageProps> GetCookieMenu()
        {
            HtmlWeb hwObject = new HtmlWeb();
            HtmlDocument htmldocObject = hwObject.Load(REQUEST_LINK);
            HtmlNode pagePropNode = htmldocObject.DocumentNode.Descendants("script").FirstOrDefault(x => x.Id == "__NEXT_DATA__");


            Root? pageRoot = JsonSerializer.Deserialize<Root>(pagePropNode.InnerText, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            return pageRoot.Props.PageProps;
        }

        public static async Task<List<EmbedBuilder>> CreateCookieEmbeds()
        {
            // Create a list of embeds for us to add to.
            List<EmbedBuilder> embeds = new List<EmbedBuilder>();
            foreach (Cookie cookie in PageProperties.Products.Cookies)
            {
                var embed = new EmbedBuilder();
                // Fields can not be empty, so ensure that it isn't.
                string calorieInfo = !String.IsNullOrEmpty(cookie.CalorieInformation.Total) ?
                                     cookie.CalorieInformation.Total : "N/A";

                string imageInfo = !String.IsNullOrEmpty(cookie.AerialImage) ?
                                     cookie.AerialImage : cookie.IconImage;

                embed.AddField("Calorie Information", calorieInfo, true)
                   .WithTitle(cookie.Name)
                   .WithFooter(footer => footer.Text = "Bot written by @Struggleton.")
                   .WithImageUrl(imageInfo)
                   .WithDescription(cookie.Description)
                   .WithUrl("https://crumblcookies.com/")
                   .WithColor(Color.LightOrange)
                   .WithCurrentTimestamp();

                // Add the cookie embed to the list
                embeds.Add(embed);
            }

            // Return the list of embeds
            return embeds;
        }

    }
}
