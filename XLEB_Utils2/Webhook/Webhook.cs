using System.Net.Http;
using System.Text;
using Exiled.API.Features;
using Newtonsoft.Json;

namespace XLEB_Utils2.Webhook
{
    public class Webhook
    {
        public static void sendDiscordWebhook(string URL, string content, string title, string avatarurl, string imageurl)
        {
            var message = new
            {
                //  avatar_url = avatarurl,
                embeds = new[] 
                { 
                    new 
                    {
                        color = 0x00ff00,
                        title = title,
                        description = content,
                        image = new {
                        url = imageurl}
                    }
                }
            };

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(message);

            var response = client.PostAsync(
            URL, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
            {
                Log.Error("Невозможно отправить вебхук");  
            }
        }
    }
}
