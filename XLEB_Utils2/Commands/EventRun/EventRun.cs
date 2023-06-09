﻿using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using XLEB_Utils2.Events;
using CommandSystem;
using System;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class EventRun : ICommand
    {
        public string Command { get; } = "eventrun";

        public string[] Aliases { get; }

        public string Description { get; } = "Выключает некоторые функции плагина, которые могут помешать ивентам";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("eventolog.xleb"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Использование:\neventrun false или true";
                return false;
            }

            switch (arguments.At(0))
            {
                case "false":
                    ServerEvents.SetOffFunctions(false);
                    response = $"Успешно включено!";
                    return true;
                case "true":
                    ServerEvents.SetOffFunctions(true);
                    response = $"Успешно выключено!";
                    return true;
            }

            response = $"Успешно! Команду вызвал {Player.Get(sender).Nickname}";
            return true;
        }
    }
}