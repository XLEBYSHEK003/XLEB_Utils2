using Exiled.Permissions.Extensions;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using CommandSystem;
using System;

namespace XLEB_Utils2.Commands
{

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Ball : ICommand
    {

        public string Command { get; } = "ball";

        public string[] Aliases { get; }

        public string Description { get; } = "Спавнит мячик возле игрока.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
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
        }
    }
}