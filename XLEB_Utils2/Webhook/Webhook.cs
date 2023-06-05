using Exiled.API.Features;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace XLEB_Utils2.Webhook
{
    public class Webhook
    {
        public static void sendDiscordWebhook(string URL, string text, string title, string avatarurl, string imageurl, string content)
        {
            var message = new
            {
                content = content,
                //  avatar_url = avatarurl,
                embeds = new[] 
                { 
                    new 
                    {
                        color = 0x00ff00,
                        title = title,
                        description = text,
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
