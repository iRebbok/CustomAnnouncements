using Smod2;
using Smod2.Commands;
using System;

namespace CustomAnnouncements
{
	class RoundEndCommand : ICommandHandler
	{
		private readonly Plugin plugin;
		private readonly Announcement an;

		public RoundEndCommand(Plugin plugin)
		{
			an = new Announcement(GetUsage(), "ca_roundend_whitelist", CustomAnnouncements.RoundEndFilePath);
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Creates custom CASSIE announcements.";
		}

		public string GetUsage()
		{
			return "(RE / ROUNDEND) (SET / CLEAR / VIEW) (TEXT)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			CustomAnnouncements.ann = UnityEngine.Object.FindObjectOfType<NineTailedFoxAnnouncer>();
			if (!an.CanRunCommand(sender))
				return new string[] { "You are not allowed to run this command." };

			if (args.Length > 0)
			{
				if (string.Equals(args[0], "set", StringComparison.OrdinalIgnoreCase))
				{
					if (args.Length > 1)
					{
						return an.SetAnnouncement(CustomAnnouncements.StringArrayToString(args, 1), "Round end announcement set.");
					}
				}
				else if (string.Equals(args[0], "view", StringComparison.OrdinalIgnoreCase))
				{
					return an.ViewAnnouncement("Error: announcement has not been set.");
				}
				else if (string.Equals(args[0], "clear", StringComparison.OrdinalIgnoreCase))
				{
					return an.ClearAnnouncement("Round end announcement cleared.");
				}
			}
			return new string[] { GetUsage() };
		}
	}
}
