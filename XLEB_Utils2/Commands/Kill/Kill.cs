using CommandSystem;
using Exiled.API.Features;
using System;
using PlayerRoles;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Kill : ICommand
    {
        public string Command { get; } = "kill";

        public string[] Aliases { get; }

        public string Description { get; } = "Убивает тебя";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);

            if (pl.Role.Type == RoleTypeId.Scp0492) 
            {
                response = "За зомби запрещено делать суицид!";
                return false;
            }

            pl.Kill("Самоубился");
            response = $"Игрок {pl.Nickname} самоубился";
            return true;
            
        }
    }
}