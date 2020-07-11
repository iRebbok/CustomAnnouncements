using System.Collections.Generic;

using SMRoleType = Smod2.API.RoleType;

namespace CustomAnnouncements
{
	static class RoleConversions
	{
		public static readonly Dictionary<SMRoleType, string> RoleConversionDict = new Dictionary<SMRoleType, string>()
		{
			{ SMRoleType.CHAOS_INSURGENCY, "chaosinsurgency" },
			{ SMRoleType.CLASSD, "classd" },
			{ SMRoleType.FACILITY_GUARD, "facility guard" },
			{ SMRoleType.NTF_CADET, "ninetailedfox" },
			{ SMRoleType.NTF_COMMANDER, "ninetailedfox" },
			{ SMRoleType.NTF_LIEUTENANT, "ninetailedfox" },
			{ SMRoleType.NTF_SCIENTIST, "ninetailedfox" },
			{ SMRoleType.SCIENTIST, "scientist" },
		};
	}
}
