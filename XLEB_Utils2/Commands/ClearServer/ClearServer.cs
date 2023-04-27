/*using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MapEditorReborn.API.Features.Objects;
using System;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ClearServer : ParentCommand
    {
        private readonly Plugin _plugin;
        public ClearServer(Plugin plugin) => _plugin = plugin;
        public ClearServer() => LoadGeneratedCommands();

        public override string Command { get; } = "clearserver";

        public override string[] Aliases { get; } = new string[] { "cs" };

        public override string Description { get; } = "Запрещает или разрешает очистку карты";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("eventolog.tools"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Использование:\nclearserver false или true";
                return false;
            }

            switch (arguments.At(0))
            {
                case "false":
                    _plugin.ServerEvents.ChangeClearServerStatus(false);
                    response = $"Успешно запрещено!";
                    return true;
                case "true":
                    _plugin.ServerEvents.ChangeClearServerStatus(true);
                    response = $"Успешно разрешено!";
                    return true;
            }

            response = $"Успешно! Команду вызвал {Player.Get(sender).Nickname}";
            return true;
        }
    }
}
*/