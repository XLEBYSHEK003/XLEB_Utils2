using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using System;
using UnityEngine;


namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class LobbyRoom : ICommand
    {
        private readonly Plugin _plugin;
        public LobbyRoom(Plugin plugin) => _plugin = plugin;

        private SchematicObject LobbyBackroom;

        public string Command { get; } = "lobbyroom";

        public string[] Aliases { get; } = new string[] { "lb" };

        public string Description { get; } = "Создаёт/удаляет лобби при ожидании";

        public void LoadGeneratedCommands() { }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("room.xleb"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Использование:\nlobbyroom spawn или delete";
                return false;
            }

            switch (arguments.At(0))
            {
                case "spawn":
                    LobbyBackroom = ObjectSpawner.SpawnSchematic(_plugin.Config.LobbyBuilding.SchematicName, new Vector3(_plugin.Config.LobbyBuilding.x, _plugin.Config.LobbyBuilding.y, _plugin.Config.LobbyBuilding.z));
                    response = $"Успешно заспавнена!";
                    return true;
                case "delete":
                    LobbyBackroom.Destroy();
                    response = $"Успешно удалено!";
                    return true;
            }

            response = $"Успешно! Команду вызвал {Player.Get(sender).Nickname}";
            return true;
        }

    }
}
