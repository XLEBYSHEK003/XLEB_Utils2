using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using PlayerRoles;
using InventorySystem.Items.ThrowableProjectiles;
using Exiled.API.Features.Items;

namespace XLEB_Utils2.Commands
{

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Ball : ParentCommand
    {
        public Ball() => LoadGeneratedCommands();

        public override string Command { get; } = "ball";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Спавнит мячик возле игрока.";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("ball.xleb"))
            {
                response = "У вас нет права использовать эту команду";
                return false;
            }

            if (arguments.Count == 0)
            {
                Player pl = Player.Get(sender);

                ((ExplosiveGrenade)Item.Create(ItemType.SCP018)).SpawnActive(pl.Position, pl);
                response = "Был заспавнен мячик";
                return true;
            }
            else 
            {
                Player pl = Player.Get((arguments.At(0)));
                response = "Был заспавнен мячик";
                return true;
            }
            response = "Был заспавнен мячик";
            return true;
        }
    }
}