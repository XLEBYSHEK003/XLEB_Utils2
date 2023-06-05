using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using CommandSystem;
using System;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class EventNotification : ICommand
    {
        public string Command { get; } = "eventnotification";

        private string text;

        public string[] Aliases { get; }

        public string Description { get; } = "Отправляет на сервер ембенд с уопминанием роли и инфомации о ивенте";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("eventolog.xleb"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Использование:\neventnotification сервер, название ивента, ивентолог";
                return false;
            }

            if (Plugin.Singleton.Config.EventWebhookNotificationURL == "https://")
            {
                response = "Неверная конфигурация";
                return false;
            }

            text = "Сервер:"+ arguments.At(0) + "\n Название:" + arguments.At(1) + "\n Ивентолог:" + arguments.At(2);

            Webhook.Webhook.sendDiscordWebhook(Plugin.Singleton.Config.EventWebhookNotificationURL, text, "Ивент", "", "", $"<@&{Plugin.Singleton.Config.EventRoleId}>");

            response = $"Успешно! Команду вызвал {Player.Get(sender).Nickname}";
            return true;
        }
    }
}
