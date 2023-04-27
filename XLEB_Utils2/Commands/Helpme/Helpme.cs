using CommandSystem;
using Exiled.API.Features;
using System;
using PlayerRoles;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Helpme : ICommand
    {
        public string Command { get; } = "helpme";

        public string[] Aliases { get; }

        public string Description { get; } = "Чинит спавн";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);
            if (Round.ElapsedTime.TotalSeconds< 20)
            {
                RoleTypeId plrole = pl.Role.Type;
                pl.ClearInventory();
                pl.RoleManager.ServerSetRole(plrole, RoleChangeReason.RoundStart);
                response = $"Игрок {pl.Nickname} переспавнился";
                return true;
            }
            else 
            {
                response = $"Время вышло. Только .kill";
                return false;
            }

        }

    }
}
