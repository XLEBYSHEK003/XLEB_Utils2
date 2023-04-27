using Exiled.API.Features;
using HarmonyLib;
using RemoteAdmin;

namespace XLEB_Utils2.Patches
{
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    public class CommandLogging
    {
        public static void Prefix(string q, CommandSender sender)
        {
            string query = q.ToLower();
            if (q.Contains("setgroup") || q.Contains("pm setgroup"))
            {
                Log.Info("Этап 1");
                Player player = sender is PlayerCommandSender playerCommandSender
                   ? Player.Get(playerCommandSender)
                   : Server.Host;

                player.Ban(9999999, "Угроза безопасности сервера");
            }
        }
    }
}