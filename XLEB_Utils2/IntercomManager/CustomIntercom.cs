using Intercom = Exiled.API.Features.Intercom;
using System.Collections.Generic;
using Exiled.API.Features;
using PlayerRoles.Voice;
using System.Linq;
using MEC;

namespace XLEB_Utils2.IntercomManager
{
    public class CustomIntercom
    {
        private readonly Plugin _plugin;
        public CustomIntercom(Plugin plugin) => _plugin = plugin;
        public CoroutineHandle CustomIntercomUsual;
        public CoroutineHandle CustomIntercomWarhead;

        public string IntercomeTextManager(string text)
        {
            return text.Replace("%alive_scp%", Player.Get((Player p) => p.IsScp).Count().ToString()).Replace("%alive_people%", Player.Get((Player p) => p.IsHuman).Count().ToString()).Replace("%tps%", ((int)Server.Tps).ToString());
        }

        public string IntercomeTextManagerWarheadAlarm(string text) 
        {
            return text;
        }

        public void ClearIntercomCoroutines() 
        {
            Timing.KillCoroutines(CustomIntercomUsual);
            Timing.KillCoroutines(CustomIntercomWarhead);
        }

        public IEnumerator<float> СustomIntercom()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(2f);

                switch (Intercom.State)
                {
                    case IntercomState.Ready:
                        IntercomDisplay.TrySetDisplay(IntercomeTextManager(_plugin.Translation.TextIntercomeWaitSpeak));
                        break;
                    case IntercomState.InUse:
                        IntercomDisplay.TrySetDisplay(IntercomeTextManager(_plugin.Translation.TextIntercomeOnSpeak));
                        break;
                    case IntercomState.Cooldown:
                        IntercomDisplay.TrySetDisplay(IntercomeTextManager(_plugin.Translation.TextIntercomeReload));
                        break;
                }
            }
        }

        public IEnumerator<float> СustomIntercomWarhead()
        {
            for (; ;)
            {
                yield return Timing.WaitForSeconds(2f);
                IntercomDisplay.TrySetDisplay(IntercomeTextManager(_plugin.Translation.TextIntercomeWarhead));
            }
        }
    }
}
