using Smod2;
using Smod2.Commands;

namespace CustomAnnouncements
{
	class RoundStartCommand : ICommandHandler
	{
		private readonly Plugin plugin;
		private readonly Announcement an;

		public RoundStartCommand(Plugin plugin)
		{
			an = new Announcement(GetUsage(), "ca_roundstart_whitelist", CustomAnnouncements.RoundStartFilePath);
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Creates custom CASSIE announcements.";
		}

		public string GetUsage()
		{
			return "(RS / ROUNDSTART) (SET / CLEAR / VIEW) (TEXT)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			CustomAnnouncements.ann = UnityEngine.Object.FindObjectOfType<NineTailedFoxAnnouncer>();
			if (!an.CanRunCommand(sender))
				return new string[] { "You are not allowed to run this command." };

			if (args.Length > 0)
			{
				if (string.Equals(args[0], "set", System.StringComparison.OrdinalIgnoreCase))
				{
					if (args.Length > 1)
					{
						return an.SetAnnouncement(CustomAnnouncements.StringArrayToString(args, 1), "Round start announcement set.");
					}
				}
				else if (string.Equals(args[0], "view", System.StringComparison.OrdinalIgnoreCase))
				{
					return an.ViewAnnouncement("Error: announcement has not been set.");
				}
				else if (string.Equals(args[0], "clear", System.StringComparison.OrdinalIgnoreCase))
				{
					return an.ClearAnnouncement("Round start announcement cleared.");
				}
			}
			return new string[] { GetUsage() };
		}
	}
}
