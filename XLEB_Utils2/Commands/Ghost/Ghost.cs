using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using CustomPlayerEffects;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Ghost : ICommand
    {
        public string Command { get; } = "ghost";

        public string[] Aliases { get; }

        public string Description { get; } = "Делает тебя невидимым";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("ghost.xleb"))
            {
                response = "Нет прав для использования команды";
                return false;
            }

            if (arguments.Count == 0) 
            {
                Player pl = Player.Get(sender);

                if (pl.IsEffectActive<Invisible>())
                {
                    pl.DisableEffect<Invisible>();
                    response = "Эффект невидимости выключен!";
                    return true;
                }
                else
                {
                    pl.EnableEffect<Invisible>();
                    response = "Эффект невидимости включен!";
                    return true;
                }
            }

            if (arguments.Count == 1)
            {
                Player pl = Player.Get(arguments.At(0));

                if (pl.IsEffectActive<Invisible>())
                {
                    pl.DisableEffect<Invisible>();
                    response = "Эффект невидимости выключен!";
                    return true;
                }
                else
                {
                    pl.EnableEffect<Invisible>();
                    response = "Эффект невидимости включен!";
                    return true;
                }
            }
            else 
            {
                response = "Неверное выполнение команды!";
                return false;
            }
        }
    }
}