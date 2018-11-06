﻿using System;
using Smod2;
using Smod2.Commands;
using Smod2.API;
using UnityEngine;

namespace CustomAnnouncements
{
	class CountdownCommand : ICommandHandler
	{
		private Plugin plugin;
		private string[] whitelist;

		public CountdownCommand(Plugin plugin)
		{
			this.plugin = plugin;
			whitelist = plugin.GetConfigList("ca_countdown_whitelist");
			for (int i = 0; i < whitelist.Length; i++)
				whitelist[i] = whitelist[i].Replace(" ", "");
		}

		private string[] GetCountdown(int start, int end)
		{
			int count = 0;
			if (start > end)
			{
				string[] num = new string[((start - end + 1) * 2) - 1];
				for (int i = start; i >= end; i--)
				{
					num[count] = (i > 20) ? CustomAnnouncements.ann.ConvertNumber(i).ToString() : i.ToString();
					if (i < 100)
						if (i != end)
							num[count + 1] = ".";
					count += 2;
				}
				return num;
			}
			else
			{
				/*string[] num = new string[((end - start + 1) * 2) - 1];
				for (int i = start; i <= end; i++)
				{
					num[count] = ann.ConvertNumber(i).ToString();
					if (i < 100)
						if (i != start)
							num[count + 1] = ".";
					count += 2;
				}
				return num;*/
				return null;
			}
		}

		public string GetCommandDescription()
		{
			return "Creates custom CASSIE announcements.";
		}

		public string GetUsage()
		{
			return "(CD / COUNTDOWN) (START) (END) (TEXT)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			CustomAnnouncements.ann = UnityEngine.Object.FindObjectOfType<NineTailedFoxAnnouncer>();
			if (sender is Player)
			{
				Player player = (Player)sender;
				if (!CustomAnnouncements.IsPlayerWhitelisted(player, whitelist))
				{
					return new string[] { "You are not allowed to run this command." };
				}
			}

			if (args.Length > 1)
			{
				int start, end = 0;
				if (Int32.TryParse(args[0], out int a))
				{
					start = a;
				}
				else
				{
					return new string[] { "Not a valid number!" };
				}

				if (Int32.TryParse(args[1], out int b))
				{
					end = b;
				}
				else
				{
					return new string[] { "Not a valid number!" };
				}

				string[] statement = GetCountdown(start, end);
				if (statement != null)
				{
					if (args.Length > 2)
					{
						string endString = "";
						for (int i = 2; i < args.Length; i++)
						{
							if (!CustomAnnouncements.IsVoiceLine(args[i]))
							{
								return new string[] { "Error: phrase \"" + args[i] + "\" is not in text to speech." };
							}
							endString += args[i] + " ";
						}
						plugin.pluginManager.Server.Map.AnnounceCustomMessage(string.Join(" ", statement) + " . . " + endString);
					}
					else
					{
						plugin.pluginManager.Server.Map.AnnounceCustomMessage(string.Join(" ", statement));
					}
					return new string[] { "Countdown has been started." };
				}
				else
				{
					return new string[] { "Error: starting value is less than ending value." }; 
				}

			}
			else
			{
				return new string[] { GetUsage() };
			}
		}
	}
}