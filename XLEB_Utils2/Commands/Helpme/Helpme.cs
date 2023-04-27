using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using PlayerRoles;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Helpme : ParentCommand
    {
        public Helpme() => LoadGeneratedCommands();

        public override string Command { get; } = "helpme";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Чинит спавн";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
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
