using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using PlayerRoles;

namespace XLEB_Utils2.Commands
{

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Kill : ParentCommand
    {
        public Kill() => LoadGeneratedCommands();

        public override string Command { get; } = "kill";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Убивает тебя";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
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