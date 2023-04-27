﻿using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using UnityEngine;

namespace XLEB_Utils2.Commands
{
    using Exiled.API.Features.Pickups;
    using PlayerRoles;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class DropSize : ICommand
    {
        public string Command { get; } = "dropsize";

        public string[] Aliases { get; } = new string[] { "drops" };

        public string Description { get; } = "Выбрасывает предмет нужного размера!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("fx.size"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count < 3)
            {
                response = "Использование:\ndropsize ((id игрока / ник) или (all / *)) (ItemType) (size)\ndropsizee ((id игрока / ник) или (all / *)) (ItemType) (x size) (y size) (z size)";
                return false;
            }

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    if (arguments.Count < 3)
                    {
                        response = "Использование: dropsizee (all / * id) (ItemType) ((size) or (x size) (y size) (z size))";
                        return false;
                    }

                    if (!Enum.TryParse(arguments.At(1), true, out ItemType type))
                    {
                        response = $"Неверное значение для предмета: {arguments.At(1)}";
                        return false;
                    }

                    switch (arguments.Count)
                    {
                        case 3:
                            if (!float.TryParse(arguments.At(2), out float size))
                            {
                                response = $"Invalid value for item scale: {arguments.At(2)}";
                                return false;
                            }
                            SpawnItem(type, size, out string msg);
                            response = msg;
                            return true;
                        case 5:
                            if (!float.TryParse(arguments.At(2), out float xval))
                            {
                                response = $"Invalid value for item scale: {arguments.At(2)}";
                                return false;
                            }

                            if (!float.TryParse(arguments.At(3), out float yval))
                            {
                                response = $"Invalid value for item scale: {arguments.At(3)}";
                                return false;
                            }

                            if (!float.TryParse(arguments.At(4), out float zval))
                            {
                                response = $"Invalid value for item scale: {arguments.At(4)}";
                                return false;
                            }
                            SpawnItem(type, xval, yval, zval, out string message);
                            response = message;
                            return true;
                        default:
                            response = "\nИспользование:\ndrops (all / *) (ItemType) (size) \ndrops (all / *) (ItemType) (x size) (y size) (z size)";
                            return false;
                    }
                default:
                    if (arguments.Count < 3)
                    {
                        response = "Usage: dropsizee (player id / name) (ItemType) ((size) or (x size) (y size) (z size))";
                        return false;
                    }

                    Player ply = Player.Get(arguments.At(0));
                    if (ply == null)
                    {
                        response = $"Player not found: {arguments.At(0)}";
                        return true;
                    }

                    if (!Enum.TryParse(arguments.At(1), true, out ItemType T))
                    {
                        response = $"Invalid value for item name: {arguments.At(1)}";
                        return false;
                    }

                    switch (arguments.Count)
                    {
                        case 3:
                            if (!float.TryParse(arguments.At(2), out float size))
                            {
                                response = $"Invalid value for item scale: {arguments.At(2)}";
                                return false;
                            }
                            SpawnItem(ply, T, size, out string msg);
                            response = msg;
                            return true;
                        case 5:
                            if (!float.TryParse(arguments.At(2), out float xval))
                            {
                                response = $"Invalid value for item scale: {arguments.At(2)}";
                                return false;
                            }

                            if (!float.TryParse(arguments.At(3), out float yval))
                            {
                                response = $"Invalid value for item scale: {arguments.At(3)}";
                                return false;
                            }

                            if (!float.TryParse(arguments.At(4), out float zval))
                            {
                                response = $"Invalid value for item scale: {arguments.At(4)}";
                                return false;
                            }
                            SpawnItem(ply, T, xval, yval, zval, out string message);
                            response = message;
                            return true;
                        default:
                            response = "\nUsage:\ndropsize (player id / name) (ItemType) (size) \ndropsize (player id / name) (ItemType) (x size) (y size) (z size)";
                            return false;
                    }
            }
        }

        private void SpawnItem(ItemType type, float size, out string message)
        {
            foreach (Player ply in Player.List)
            {
                if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                    continue;

                Pickup.CreateAndSpawn(type, ply.Position, default, ply).Scale = Vector3.one * size;
            }
            message = $"Spawned in a {type.ToString()} that is a size of {size} at every player's position (\"Yay! Items with sizes!\" - Galaxy119)";
        }

        private void SpawnItem(ItemType type, float x, float y, float z, out string message)
        {
            foreach (Player ply in Player.List)
            {
                if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                    continue;

                Pickup.CreateAndSpawn(type, ply.Position, default, ply).Scale = new Vector3(x, y, z);
            }
            message = $"Spawned in a {type.ToString()} that is {x}x{y}x{z} at every player's position (\"Yay! Items with sizes!\" - Galaxy119)";
        }

        private void SpawnItem(Player ply, ItemType type, float size, out string message)
        {
            Pickup.CreateAndSpawn(type, ply.Position, default, ply).Scale = Vector3.one * size;
            message = $"Spawned in a {type.ToString()} that is a size of {size} at {ply.Nickname}'s position (\"Yay! Items with sizes!\" - Galaxy119)";
        }

        private void SpawnItem(Player ply, ItemType type, float x, float y, float z, out string message)
        {
            Pickup.CreateAndSpawn(type, ply.Position, default, ply).Scale = new Vector3(x, y, z);
            message = $"Spawned in a {type.ToString()} that is {x}x{y}x{z} at {ply.Nickname}'s position (\"Yay! Items with sizes!\" - Galaxy119)";
        }
    }
}