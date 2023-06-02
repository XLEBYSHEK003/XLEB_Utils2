using System;

namespace XLEB_Utils2.DonateControl
{
    public class DonateUser
    {
        public string SteamId { get; set; }
        public DateTime TimePurchase { get; set; }
        public DateTime EndTimePurchase { get; set; }
        public string PrefixName { get; set; }
        public string PrefixColor { get; set; }
        public string NameGroup { get; set; }
    }
}
