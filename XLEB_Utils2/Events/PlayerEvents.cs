using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp330;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using MEC;

namespace XLEB_Utils2.Events
{
    public class PlayerEvents
    {
        private readonly Plugin _plugin;
        public PlayerEvents(Plugin plugin) => _plugin = plugin;

        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            if (_plugin.Config.EnableDonatorsRank)
            {
                if (ev.Player.UserId != null && _plugin.Config.PrefixesList.ContainsKey(ev.Player.UserId)) 
                {
                    Timing.CallDelayed(5, () => ev.Player.RankName = _plugin.Config.PrefixesList[ev.Player.UserId].PrefixName);
                    Timing.CallDelayed(7, () => ev.Player.RankColor = _plugin.Config.PrefixesList[ev.Player.UserId].PrefixColor);
                }
            }
        }

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            if (ev.Player != null && _plugin.Config.AntiCuffHurtOn && ev.Player.IsCuffed && ev.Attacker != null && ev.Attacker.Role.Team != Team.SCPs)
            {
                ev.IsAllowed = false;
                ev.Attacker.ShowHint(_plugin.Translation.MessageForAttacker, 2f);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev) 
        {
            if (_plugin.Config.HealthValues.ContainsKey(ev.NewRole)) 
            {
                ev.Player.Health = _plugin.Config.HealthValues[ev.NewRole];
                ev.Player.MaxHealth = _plugin.Config.HealthValues[ev.NewRole];
            }   
        }

        public void OnUsingBattery(UsingRadioBatteryEventArgs ev) 
        {
            if (_plugin.Config.UnlimitedBattery)
                ev.IsAllowed = false;          
        }

        public void OnSCP095AddTarget(AddingTargetEventArgs ev) 
        {
            ev.Target.ShowHint(_plugin.Translation.AddingTarget096);
        }

        public void OnEatingCandy(EatingScp330EventArgs ev) 
        {
            if (ev.Candy.Kind == CandyKindID.Pink && _plugin.Config.DisablePinkCandyNameGroup.Contains(ev.Player.GroupName))
                ev.IsAllowed = false;
        }

        public void OnDied(DiedEventArgs ev) 
        {
            if (ev.Attacker != null) 
                ev.Attacker.ShowHint(_plugin.Translation.KillMessage.Replace("%PlayerName%", ev.Player.Nickname), 2f);  
        }
    }
}
