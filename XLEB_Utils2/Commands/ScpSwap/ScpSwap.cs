using Exiled.API.Features;
using CommandSystem;
using System.Linq;
using System;

namespace XLEB_Utils2.Commands.ScpSwap
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ScpSwap : ICommand
    {
        ScpSwapComponent swapComponent = new ScpSwapComponent();
        public string Command { get; } = "scpswap";

        public string[] Aliases { get; }

        public string Description { get; } = "Меняет класс игрока, который играет за scp";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);

            if (pl.Role.Side != Exiled.API.Enums.Side.Scp)
            {
                response = "Вы не SCP!";
                return false;
            }

            if (Round.ElapsedTime.Seconds > 30)
            {
                response = "Прошло уже много времени!";
                return false;
            }

            if (swapComponent.ListUsingCommandUsers.Contains(pl.UserId)) 
            {
                response = "Команда уже была использована!";
                return false;       
            }

            swapComponent.ListUsingCommandUsers.Add(pl.UserId);

            var FreeScpPlace = swapComponent.TotalScpInRound.Intersect(swapComponent.ScpRoles);
            if (FreeScpPlace.Count() == 0) 
            {
                response = "Нет мест для респавна!";
                return false;
            }

            pl.Role.Set(FreeScpPlace.ToList().RandomItem(), Exiled.API.Enums.SpawnReason.Respawn);
            response = $"Игрок {pl.Nickname} переспавнился";
            return true;
        }
    }
}
