﻿using System;
using Smod2;
using Smod2.API;
using Smod2.Attributes;
using System.IO;

// TO DO:

// ADD SUPPORT FOR PERIODS
// INVESTIGATE SOME WORDS NOT WORKING

namespace CustomAnnouncements
{
    [PluginDetails(
    author = "Cyanox",
    name = "CustomAnnouncements",
    description = "Makes custom CASSIE announcements",
    id = "cyan.custom.announcements",
    version = "0.7",
    SmodMajor = 3,
    SmodMinor = 0,
    SmodRevision = 0
    )]
    public class CustomAnnouncements : Plugin
    {
		public static NineTailedFoxAnnouncer ann;
		public static string configFolerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/CustomAnnouncements";
		public static string presetFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/CustomAnnouncements/presets.txt";
		public static string timerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/CustomAnnouncements/timers.txt";
		public static string chaosFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/CustomAnnouncements/chaos.txt";
		public static string roundendFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/SCP Secret Laboratory/CustomAnnouncements/roundend.txt";
		public static bool roundStarted = false;

		public override void OnDisable() {}

		public override void OnEnable()
		{
			if (IsLinux)
			{
				configFolerFilePath = "/home/" + Environment.UserName + "/.config/SCP Secret Laboratory/CustomAnnouncements";
				presetFilePath = "/home/" + Environment.UserName + "/.config/SCP Secret Laboratory/CustomAnnouncements/presets.txt";
				timerFilePath = "/home/" + Environment.UserName + "/.config/SCP Secret Laboratory/CustomAnnouncements/timers.txt";
				chaosFilePath = "/home/" + Environment.UserName + "/.config/SCP Secret Laboratory/CustomAnnouncements/chaos.txt";
				roundendFilePath = "/home/" + Environment.UserName + "/.config/SCP Secret Laboratory/CustomAnnouncements/roundend.txt";
			}

			if (!Directory.Exists(configFolerFilePath))
			{
				Directory.CreateDirectory(configFolerFilePath);
			}
			if (!File.Exists(presetFilePath))
			{
				using (new StreamWriter(File.Create(presetFilePath))) { }
			}
			if (!File.Exists(timerFilePath))
			{
				using (new StreamWriter(File.Create(timerFilePath))) { }
			}
			if (!File.Exists(chaosFilePath))
			{
				using (new StreamWriter(File.Create(chaosFilePath))) { }
			}
			if (!File.Exists(roundendFilePath))
			{
				using (new StreamWriter(File.Create(roundendFilePath))) { }
			}
		}

        public override void Register()
        {
			// Event handlers
			this.AddEventHandlers(new RoundEventHandler(this));

			// Config settings
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_countdown_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the countdown command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_text_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the text command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_mtf_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the mtf command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_scp_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the scp command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_preset_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the preset command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_timer_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the timer command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_chaosspawn_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the chaosspawn command."));
			this.AddConfig(new Smod2.Config.ConfigSetting("ca_roundend_whitelist", new string[] { "owner", "admin" }, Smod2.Config.SettingType.LIST, true, "Defines what ranks are allowed to use the roundend command."));

			// Commands
			this.AddCommands(new string[] { "customannouncements", "ca" }, new CommandsOutput());
			this.AddCommands(new string[] { "countdown", "cd" }, new CountdownCommand(this));
			this.AddCommands(new string[] { "textannouncement", "ta" }, new CustomTextCommand(this));
			this.AddCommands(new string[] { "mtfannouncement", "mtfa" }, new MTFAnnouncementCommand(this));
			this.AddCommands(new string[] { "scpannouncement", "scpa" }, new SCPEliminationCommand(this));
			this.AddCommands(new string[] { "preset", "pr" }, new PresetCommand(this));
			this.AddCommands(new string[] { "timer", "ti" }, new TimerCommand(this));
			this.AddCommands(new string[] { "chaosspawn", "cs" }, new ChaosSpawnCommand(this));
			this.AddCommands(new string[] { "roundend", "re" }, new RoundEndCommand(this));
		}

		public static bool IsPlayerWhitelisted(Player player, string[] whitelist)
		{
			foreach (string rank in whitelist)
			{
				if (player.GetRankName().ToLower() == rank.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsVoiceLine(string str)
		{
			foreach (NineTailedFoxAnnouncer.VoiceLine vl in ann.voiceLines)
			{
				if (vl.apiName.ToUpper() == str.ToUpper())
					return true;
			}
			return false;
		}

		public static bool IsLinux
		{
			get
			{
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
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
	}
}
