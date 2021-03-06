﻿using Smod2.Commands;

namespace CustomAnnouncements
{
	class CommandsOutput : ICommandHandler
	{
		public CommandsOutput() {}

		public string GetCommandDescription()
		{
			return "Creates custom CASSIE announcements.";
		}

		public string GetUsage()
		{
			return "(CA / CUSTOMANNOUNCEMENTS)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			return new string[] { "CUSTOM ANNOUNCEMENTS COMMANDS:",
				"(CA / CUSTOMANNOUNCEMENTS)",
				"(CD / COUNTDOWN) (START) (END) (TEXT)",
				"(TA / TEXTANNOUNCEMENT) (TEXT)",
				"(MTFA / MTFANNOUNCEMENT) (SCPS LEFT) (MTF NUMBER) (MTF LETTER)",
				"(SCPA / SCPANNOUNCEMENT) (SCP NUMBER)",
				"(PR / PRESET) (SAVE / LOAD / REMOVE / LIST) (NAME) (TEXT)",
				"(TI / TIMER) (SET / REMOVE / LIST) (TIME) (TEXT)",
				"(PA / PLAYERANNOUNCEMENT) (SAVE / REMOVE / LIST) (PLAYER NAME / STEAMID) (TEXT)",
				"(CHS / CHAOSSPAWN) (SET / CLEAR / VIEW) (TEXT)",
				"(RS / ROUNDSTART) (SET / CLEAR / VIEW) (TEXT)",
				"(RE / ROUNDEND) (SET / CLEAR / VIEW) (TEXT)",
				"(WP / WAITINGFORPLAYERS) (SET / CLEAR / VIEW) (TEXT)",
				"(PE / PLAYERESCAPE) (SET / CLEAR / VIEW) (TEXT)",
				"(AW / AUTOWARHEAD) (SET / CLEAR / VIEW) (TEXT)"
			};
		}
	}
}
