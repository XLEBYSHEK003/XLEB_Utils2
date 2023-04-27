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
                    LobbyBackroom = ObjectSpawner.SpawnSchematic("Backrooms", new Vector3(20f, 940f, -40));
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
