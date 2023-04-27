using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace XLEB_Utils2.Commands
{

    [CommandHandler(typeof(ClientCommandHandler))]
    public class TPS : ParentCommand
    {
        public TPS() => LoadGeneratedCommands();

        public override string Command { get; } = "tps";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Показывает текущий TPS";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            response = $"Текущий TPS {(int)Server.Tps}";
            return true;

        }
    }
}