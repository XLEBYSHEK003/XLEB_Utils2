using PlayerRoles;
using System.Collections.Generic;

namespace XLEB_Utils2.Commands.ScpSwap
{
    public class ScpSwapComponent
    {
        public List<string> ListUsingCommandUsers = new List<string>();

        public List<RoleTypeId> TotalScpInRound = new List<RoleTypeId>();

        public List<RoleTypeId> ScpRoles = new List<RoleTypeId>
        {
            RoleTypeId.Scp173,
            RoleTypeId.Scp106,
            RoleTypeId.Scp049,
            RoleTypeId.Scp096,
            RoleTypeId.Scp939,
            RoleTypeId.Scp079
        };
    }
}
