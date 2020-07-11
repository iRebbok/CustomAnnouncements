using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SMRoleType = Smod2.API.RoleType;
// TO DO:

// ¯\_(ツ)_/¯

namespace CustomAnnouncements
{
	[PluginDetails(
	author = "Cyanox",
	name = "CustomAnnouncements",
	description = "Makes custom CASSIE announcements",
	id = "cyan.custom.announcements",
	version = "1.7",
	SmodMajor = 3,
	SmodMinor = 7,
	SmodRevision = 0
	)]
	public class CustomAnnouncements : Plugin
	{
		private static Plugin plugin;
		public static NineTailedFoxAnnouncer ann;
		public static List<Announcement> anList = new List<Announcement>();
		public static List<string> roundVariables = new List<string>()
		{
			"$scp_alive",
			"$scp_start",
			"$scp_dead",
			"$scp_zombies",
			"$classd_alive",
			"$classd_escape",
			"$classd_start",
			"$scientists_alive",
			"$scientists_escape",
			"$scientists_start",
			"$scp_kills",
			"$grenade_kills",
			"$mtf_alive",
			"$ci_alive",
			"$tutorial_alive",
			"$round_duration",
			"$escape_class" // this one is not listed in the ReplaceVariables method due to it only applying to one event
		};
		public static string ConfigFolerFilePath = FileManager.GetAppFolder() + "CustomAnnouncements";
		public static string PresetsFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "Presets.txt";
		public static string TimersFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "Timers.txt";
		public static string ChaosSpawnFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "ChaosSpawn.txt";
		public static string RoundStartFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "RoundStart.txt";
		public static string RoundEndFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "RoundEnd.txt";
		public static string PlayerJoinFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "PlayerJoin.txt";
		public static string WaitingForPlayersFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "WaitingForPlayers.txt";
		public static string PlayerEscapeFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "PlayerEscape.txt";
		public static string WarheadAutoStartFilePath = FileManager.GetAppFolder() + "CustomAnnouncements" + Path.DirectorySeparatorChar + "WarheadAutoStart.txt";
		public static bool roundStarted = false;

		public override void OnDisable() { }

		public override void OnEnable()
		{
			if (!Directory.Exists(ConfigFolerFilePath))
			{
				Directory.CreateDirectory(ConfigFolerFilePath);
			}
			if (!File.Exists(PresetsFilePath))
			{
				using (new StreamWriter(File.Create(PresetsFilePath))) { }
			}
			if (!File.Exists(TimersFilePath))
			{
				using (new StreamWriter(File.Create(TimersFilePath))) { }
			}
			if (!File.Exists(ChaosSpawnFilePath))
			{
				using (new StreamWriter(File.Create(ChaosSpawnFilePath))) { }
			}
			if (!File.Exists(RoundStartFilePath))
			{
				using (new StreamWriter(File.Create(RoundStartFilePath))) { }
			}
			if (!File.Exists(RoundEndFilePath))
			{
				using (new StreamWriter(File.Create(RoundEndFilePath))) { }
			}
			if (!File.Exists(PlayerJoinFilePath))
			{
				using (new StreamWriter(File.Create(PlayerJoinFilePath))) { }
			}
			if (!File.Exists(WaitingForPlayersFilePath))
			{
				using (new StreamWriter(File.Create(WaitingForPlayersFilePath))) { }
			}
			if (!File.Exists(PlayerEscapeFilePath))
			{
				using (new StreamWriter(File.Create(PlayerEscapeFilePath))) { }
			}
			if (!File.Exists(WarheadAutoStartFilePath))
			{
				using (new StreamWriter(File.Create(WarheadAutoStartFilePath))) { }
			}
		}

		public override void Register()
		{
			plugin = this;

			// Event handlers
			AddEventHandlers(new RoundEventHandler(this));

			// Config settings
			var defaultCommandPermissionRoles = new[] { "owner", "admin" };
			AddConfig(new ConfigSetting("ca_all_whitelist", Array.Empty<string>(), true, "Defines what ranks are allowed to use all commands. This will override all other whitelists."));
			AddConfig(new ConfigSetting("ca_countdown_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the countdown command."));
			AddConfig(new ConfigSetting("ca_text_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the text command."));
			AddConfig(new ConfigSetting("ca_mtf_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the mtf command."));
			AddConfig(new ConfigSetting("ca_scp_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the scp command."));
			AddConfig(new ConfigSetting("ca_preset_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the preset command."));
			AddConfig(new ConfigSetting("ca_timer_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the timer command."));
			AddConfig(new ConfigSetting("ca_chaosspawn_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the chaosspawn command."));
			AddConfig(new ConfigSetting("ca_roundstart_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the roundstart command."));
			AddConfig(new ConfigSetting("ca_roundend_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the roundend command."));
			AddConfig(new ConfigSetting("ca_player_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the player command."));
			AddConfig(new ConfigSetting("ca_waitingforplayers_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the waitingforplayers command."));
			AddConfig(new ConfigSetting("ca_playerescape_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the playerescape command."));
			AddConfig(new ConfigSetting("ca_autowarhead_whitelist", defaultCommandPermissionRoles, true, "Defines what ranks are allowed to use the autowarhead command."));

			// Commands
			AddCommands(new[] { "customannouncements", "ca" }, new CommandsOutput());
			AddCommands(new[] { "countdown", "cd" }, new CountdownCommand(this));
			AddCommands(new[] { "textannouncement", "ta" }, new CustomTextCommand(this));
			AddCommands(new[] { "mtfannouncement", "mtfa" }, new MTFAnnouncementCommand(this));
			AddCommands(new[] { "scpannouncement", "scpa" }, new SCPEliminationCommand(this));
			AddCommands(new[] { "preset", "pr" }, new PresetCommand(this));
			AddCommands(new[] { "timer", "ti" }, new TimerCommand(this));
			AddCommands(new[] { "chaosspawn", "chs" }, new ChaosSpawnCommand(this));
			AddCommands(new[] { "roundstart", "rs" }, new RoundStartCommand(this));
			AddCommands(new[] { "roundend", "re" }, new RoundEndCommand(this));
			AddCommands(new[] { "playerannouncement", "pa" }, new PlayerAnnouncementCommand(this));
			AddCommands(new[] { "waitingforplayers", "wp" }, new WaitingForPlayersCommand(this));
			AddCommands(new[] { "playerescape", "pe" }, new PlayerEscapeCommand(this));
			AddCommands(new[] { "autowarhead", "aw" }, new AutoWarheadCommand(this));
		}

		public static int CountRoles(SMRoleType role)
		{
			var baseGameClass = (RoleType)role;
			return ReferenceHub.GetAllHubs().Count(rh => rh.Value.characterClassManager.CurClass == baseGameClass);
		}

		public static bool IsPlayerWhitelisted(Player player, string[] whitelist)
		{
			foreach (string rank in whitelist)
			{
				plugin.Info(rank + "=" + player.GetRankName().ToLower());
				if (string.Equals(player.GetRankName(), rank, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public static string[] SetWhitelist(string whitelistName)
		{
			string[] allWhitelist = plugin.GetConfigList("ca_all_whitelist");
			return allWhitelist.Length > 0 ? allWhitelist : plugin.GetConfigList(whitelistName);
		}

		public static bool IsVoiceLine(string str)
		{
			foreach (NineTailedFoxAnnouncer.VoiceLine vl in ann.voiceLines)
			{
				if (string.Equals(vl.apiName, str, StringComparison.OrdinalIgnoreCase))
					return true;
			}
			return false;
		}

		public static string GetNonValidText(string[] text)
		{
			foreach (string str in text)
			{
				string word = str;
				if (word?.Length == 0) continue;
				if (word.IndexOf(".") != -1)
				{
					word = word.Replace(" .", "");
				}

				if (!IsVoiceLine(word) && !roundVariables.Contains(word.ToLower()))
				{
					return word;
				}
			}
			return null;
		}

		public static string StringArrayToString(string[] array, int startPos)
		{
			string saveText = null;
			if (array.Length > 0)
			{
				for (int i = startPos; i < array.Length; i++)
				{
					saveText += array[i];
					if (i != array.Length - 1)
						saveText += " ";
				}
			}
			return saveText;
		}

		public static bool DoesKeyExistInFile(string filePath, string key)
		{
			string[] currentText = File.ReadAllLines(filePath);

			if (currentText.Length > 0)
			{
				foreach (string str in currentText)
				{
					if (string.Equals(str.Split(':')[0], key, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string GetValueOfKey(string filePath, string key)
		{
			string[] keys = File.ReadAllLines(filePath);
			if (keys.Length > 0)
			{
				foreach (string str in keys)
				{
					if (string.Equals(str.Split(':')[0], key, StringComparison.OrdinalIgnoreCase))
					{
						return str.Substring(str.IndexOf(':') + 2);
					}
				}
			}
			return null;
		}

		public static int AddLineToFile(string filePath, string key, string value)
		{
			if (DoesKeyExistInFile(filePath, key))
			{
				return -1;
			}

			File.AppendAllText(filePath, key + ": " + value + Environment.NewLine);
			return 1;
		}

		public static int RemoveLineFromFile(string[] lines, string removeString, string filePath)
		{
			List<string> newText = new List<string>();
			int val = lines.Length;
			int count = 0;
			foreach (string str in lines)
			{
				if (str.Split(':')[0] != removeString)
				{
					newText.Add(str);
					count++;
				}
			}

			if (val == count)
			{
				return -1;
			}

			File.WriteAllText(filePath, String.Empty);

			foreach (string str in newText.ToArray())
			{
				File.AppendAllText(filePath, str + Environment.NewLine);
			}
			return 1;
		}

		public static string ReplaceVariables(string input)
		{
			RoundStats stats = PluginManager.Manager.Server.Round.Stats;
			int minutes = PluginManager.Manager.Server.Round.Duration / 60, duration = PluginManager.Manager.Server.Round.Duration;

			input = input.Replace("$scp_alive", stats.SCPAlive.ToString());
			input = input.Replace("$scp_start", stats.SCPStart.ToString());
			input = input.Replace("$scp_dead", stats.SCPDead.ToString());
			input = input.Replace("$scp_zombies", stats.Zombies.ToString());
			input = input.Replace("$classd_alive", stats.ClassDAlive.ToString());
			input = input.Replace("$classd_escape", stats.ClassDEscaped.ToString());
			input = input.Replace("$classd_start", stats.ClassDStart.ToString());
			input = input.Replace("$scientists_alive", stats.ScientistsAlive.ToString());
			input = input.Replace("$scientists_escape", stats.ScientistsEscaped.ToString());
			input = input.Replace("$scientists_start", stats.ScientistsStart.ToString());
			input = input.Replace("$scp_kills", stats.SCPKills.ToString());
			input = input.Replace("$grenade_kills", stats.GrenadeKills.ToString());
			input = input.Replace("$mtf_alive", stats.NTFAlive.ToString());
			input = input.Replace("$ci_alive", stats.CiAlive.ToString());
			input = input.Replace("$tutorial_alive", CountRoles(SMRoleType.TUTORIAL).ToString());
			input = input.Replace("$round_duration", (duration < 60) ? duration + ((duration == 1) ? " second" : " seconds") : minutes + ((minutes == 1) ? " minute" : " minutes") + " and " + (duration - (minutes * 60)) + ((duration - (minutes * 60) == 1) ? " second" : " seconds"));

			string[] words = input.Split(' ');

			for (int i = 0; i < words.Length; i++)
			{
				if (Int32.TryParse(words[i], out int a) && a > 20)
				{
					words[i] = ann.ConvertNumber(a);
				}
			}

			return StringArrayToString(words, 0);
		}

		public static string SpacePeriods(string input)
		{
			string[] words = input.Split(' ');
			for (int i = 0; i < words.Length; i++)
			{
				int index = words[i].IndexOf(".");
				if (index != -1)
				{
					if (index == 0)
						words[i] = words[i].Replace(".", ". ");
					else
						words[i] = words[i].Replace(".", " .");
				}
			}
			return StringArrayToString(words, 0);
		}

		public static string HandleNumbers(string input)
		{
			string[] words = input.Split(' ');
			for (int i = 0; i < words.Length; i++)
			{
				if (int.TryParse(words[i], out int a))
				{
					if (a > 20)
					{
						string b = ann.ConvertNumber(a).Replace("  ", " ");
						words[i] = b.Substring(0, b.Length - 1);
					}
				}
			}
			return StringArrayToString(words, 0);
		}

		public static int LevenshteinDistance(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			for (int i = 1; i <= n; i++)
			{
				for (int j = 1; j <= m; j++)
				{
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			return d[n, m];
		}

		public static Player GetPlayer(string args, out Player playerOut)
		{
			int maxNameLength = 31, LastnameDifference = 31;
			Player plyer = null;
			string str1 = args.ToLower();
			foreach (Player pl in PluginManager.Manager.Server.GetPlayers(str1))
			{
				if (pl.Name.IndexOf(args, StringComparison.OrdinalIgnoreCase) < 0) { goto NoPlayer; }
				if (str1.Length < maxNameLength)
				{
					int x = maxNameLength - str1.Length;
					int y = maxNameLength - pl.Name.Length;
					string str2 = pl.Name;
					for (int i = 0; i < x; i++)
					{
						str1 += "z";
					}
					for (int i = 0; i < y; i++)
					{
						str2 += "z";
					}
					int nameDifference = CustomAnnouncements.LevenshteinDistance(str1, str2);
					if (nameDifference < LastnameDifference)
					{
						LastnameDifference = nameDifference;
						plyer = pl;
					}
				}
				NoPlayer:;
			}
			playerOut = plyer;
			return playerOut;
		}

		public static float CalculateDuration(string tts)
		{
			float num = 0f;
			foreach (string text in tts.Split(' '))
			{
				foreach (NineTailedFoxAnnouncer.VoiceLine voiceLine in ann.voiceLines)
				{
					if (string.Equals(text, voiceLine.apiName, StringComparison.OrdinalIgnoreCase))
					{
						num += voiceLine.length;
					}
				}
			}
			return num;
		}
	}
}
