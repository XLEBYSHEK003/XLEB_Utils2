using Exiled.Permissions.Extensions;
using System.Collections.Generic;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using CommandSystem;
using PlayerRoles;
using UnityEngine;
using System;
using MEC;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Rocket : ICommand
    {
        public string Command { get; } = "rocket";

        public string[] Aliases { get; }

        public string Description { get; } = "Отправляет игроков высоко в небо и взрывает их";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("rocket.xleb"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Использование: rocket ((player id / name) or (all / *)) (speed)";
                return false;
            }

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    if (!float.TryParse(arguments.At(1), out float speed) && speed <= 0)
                    {
                        response = $"Неверная скорость: {arguments.At(1)}";
                        return false;
                    }

                    foreach (Player ply in Player.List)
                        Timing.RunCoroutine(DoRocket(ply, speed));

                    response = "Все взлетели в небо (Мы отправляемся в путешествие на нашем любимом космическом корабле).";
                    return true;

                    default:
                    Player pl = Player.Get(arguments.At(0));
                    if (pl == null)
                    {
                        response = $"Игрок не найден: {arguments.At(0)}";
                        return false;
                    }
                    else if (pl.Role == RoleTypeId.Spectator || pl.Role == RoleTypeId.None)
                    {
                        response = $"Игрок {pl.Nickname} не является допустимым классом для rocket";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(1), out float spd) && spd <= 0)
                    {
                        response = $"Неверное значение скорости: {arguments.At(1)}";
                        return false;
                    }

                    Timing.RunCoroutine(DoRocket(pl, spd));
                    response = $"Игрок {pl.Nickname} был взмыт в небо (мы отправляемся в путешествие на нашем любимом космическом корабле)";
                    return true;
            }
        }

        public static IEnumerator<float> DoRocket(Player player, float speed)
        {
            const int maxAmnt = 50;
            int amnt = 0;
            while (player.Role != RoleTypeId.Spectator)
            {
                player.Position += Vector3.up * speed;
                amnt++;
                if (amnt >= maxAmnt)
                {
                    player.IsGodModeEnabled = false;
                    ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    grenade.FuseTime = 0.8f;
                    grenade.SpawnActive(player.Position, player);
                    player.Kill("Отправились в путешествие на своем любимом космическом корабле.");
                }

                yield return Timing.WaitForOneFrame;
            }
        }
    }
}