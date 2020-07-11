using Smod2;
using Smod2.API;
using Smod2.Commands;
using System;

namespace CustomAnnouncements
{
	class PlayerAnnouncementCommand : ICommandHandler
	{
		private readonly Announcement an;
		private readonly Plugin plugin;

		public PlayerAnnouncementCommand(Plugin plugin)
		{
			an = new Announcement(GetUsage(), "ca_player_whitelist", CustomAnnouncements.PlayerJoinFilePath);
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Creates custom CASSIE announcements.";
		}

		public string GetUsage()
		{
			return "(PA / PLAYERANNOUNCEMENT) (SAVE / REMOVE / LIST) (STEAMID) (TEXT)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			CustomAnnouncements.ann = UnityEngine.Object.FindObjectOfType<NineTailedFoxAnnouncer>();
			if (!an.CanRunCommand(sender))
				return new string[] { "You are not allowed to run this command." };

			if (args.Length > 0)
			{
				if (string.Equals(args[0], "save", StringComparison.OrdinalIgnoreCase))
				{
					if (args.Length > 2)
					{
						string name = "", id = "";
						Player cPlayer = CustomAnnouncements.GetPlayer(args[1], out cPlayer);
						if (cPlayer != null)
						{
							name = cPlayer.Name;
							id = cPlayer.UserId;
						}
						else if (ulong.TryParse(args[1], out ulong a))
						{
							name = a.ToString();
							id = a.ToString();
						}
						else
						{
							return new string[] { "Error: invalid player id." };
						}

						return an.SetVariable(id, CustomAnnouncements.StringArrayToString(args, 2), "Error: Player already exists.", "Saved announcement for player \"" + name + "\".");
					}
				}
				else if (string.Equals(args[0], "remove", StringComparison.OrdinalIgnoreCase))
				{
					if (args.Length > 1)
					{
						string name = "", id = "";
						if (!string.Equals(args[1], "all", StringComparison.OrdinalIgnoreCase) && args[1] != "*")
						{
							Player cPlayer = CustomAnnouncements.GetPlayer(args[1], out cPlayer);
							if (cPlayer != null)
							{
								name = cPlayer.Name;
								id = cPlayer.UserId;
							}
							else if (ulong.TryParse(args[1], out ulong a))
							{
								name = a.ToString();
								id = a.ToString();
							}
						}
						else
						{
							id = "all";
						}

						return an.RemoveVariable(id, "Error: there are no player announcements.", "Error: couldn't find player \"" + name + "\".", "Removed all player announcements.", "Removed player \"" + name + "\".");
					}
				}
				else if (string.Equals(args[0], "list", StringComparison.OrdinalIgnoreCase))
				{
					return an.ListVariables("Error: there are no player announcements.");
				}
			}
			return new string[] { GetUsage() };
		}
	}
}