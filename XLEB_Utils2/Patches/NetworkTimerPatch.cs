using Exiled.API.Features;
using PlayerRoles;
using HarmonyLib;
using GameCore;

namespace XLEB_Utils2.Patches
{
    [HarmonyPatch(typeof(RoundStart), nameof(RoundStart.NetworkTimer), MethodType.Setter)]
    internal class NetworkTimerPatch
    {
        private static void Postfix(RoundStart __instance, ref short value)
        {
            if (value == 1)
            {
                foreach (Player player in Player.List)
                {
                    player.RoleManager.ServerSetRole(RoleTypeId.Spectator, RoleChangeReason.RoundStart);
                }
            }
        }
    }
}
