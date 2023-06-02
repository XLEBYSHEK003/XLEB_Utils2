using Exiled.Events.EventArgs.Player;
using LiteDB;
using MEC;

namespace XLEB_Utils2.DonateControl
{
    public class DonateControlEventHandler
    {
        private readonly Plugin _plugin;
        public DonateControlEventHandler(Plugin plugin) => _plugin = plugin;

        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            using (var db = new LiteDatabase(_plugin.Config.DataBaseName)) 
            {
                var donatorsCollection = db.GetCollection<DonateUser>("donateuser");

                var donator = donatorsCollection.FindOne(x => x.SteamId == ev.Player.UserId);

                if (donator != null)
                {
                    Timing.CallDelayed(5, () => ev.Player.RankName = donator.PrefixName);
                    Timing.CallDelayed(7, () => ev.Player.RankColor = donator.PrefixColor);
                }
            }
        }
    }
}
