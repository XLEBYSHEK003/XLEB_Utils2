using Exiled.API.Features;
using CommandSystem;
using System;

namespace XLEB_Utils2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class TPS : ICommand
    {
        public string Command { get; } = "tps";

        public string[] Aliases { get; }

        public string Description { get; } = "Показывает текущий TPS";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"Текущий TPS {(int)Server.Tps}";
            return true;
        }
    }
}