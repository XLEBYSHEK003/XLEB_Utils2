using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using PlayerRoles;
using XLEB_Utils2.Events;
using UnityEngine;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Size : ParentCommand
    {
        public Size() => LoadGeneratedCommands();

        public override string Command { get; } = "size";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Устанавливает размер модели игрока";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("size.xleb"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Использование:\nsize (id игрока / ник) или (all / *)) (x value) (y value) (z value)" +
                    "\nsize reset";
                return false;
            }

            switch (arguments.At(0))
            {
                case "reset":
                    foreach (Player ply in Player.List)
                    {
                        if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                            continue;

                        SetPlayerScale(ply, 1, 1, 1);
                    }

                    response = $"Размер всех игроков был возвращён в нормальное положение!";
                    return true;
                case "*":
                case "all":
                    if (arguments.Count != 4)
                    {
                        response = "Использование: size (all / *) (x) (y) (z)";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(1), out float xval))
                    {
                        response = $"Неверное значение x: {arguments.At(1)}";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(2), out float yval))
                    {
                        response = $"Неверное значение y: {arguments.At(2)}";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(3), out float zval))
                    {
                        response = $"Неверное значение z: {arguments.At(3)}";
                        return false;
                    }

                    foreach (Player ply in Player.List)
                    {
                        if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                            continue;

                        SetPlayerScale(ply, xval, yval, zval);
                    }

                    response = $"Размер всех игроков был изменён на: {xval} {yval} {zval}";
                    return true;
                default:
                    if (arguments.Count != 4)
                    {
                        response = "Использование: size (player id / name) (x) (y) (z)";
                        return false;
                    }

                    Player pl = Player.Get(arguments.At(0));
                    if (pl == null)
                    {
                        response = $"Игрок не найден: {arguments.At(0)}";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(1), out float x))
                    {
                        response = $"Неверное значение x: {arguments.At(1)}";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(2), out float y))
                    {
                        response = $"Неверное значение y: {arguments.At(2)}";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(3), out float z))
                    {
                        response = $"Неверное значение z: {arguments.At(3)}";
                        return false;
                    }

                    SetPlayerScale(pl, x, y, z);
                    response = $"Игрок {pl.Nickname} изменил размер на: {x} {y} {z}";
                    return true;
            }
        }
        
        private static void SetPlayerScale(Player target, float x, float y, float z)
        {
            try
            {
                target.Scale = new Vector3(x, y, z);
            }
            catch (Exception e)
            {
                Log.Info($"Ошибка размера: {e}");
            }
        }
    }
}
