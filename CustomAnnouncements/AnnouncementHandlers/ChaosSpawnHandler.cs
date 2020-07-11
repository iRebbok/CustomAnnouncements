using Smod2;
using System.Threading;

namespace CustomAnnouncements
{
	class ChaosSpawnHandler
	{
		public ChaosSpawnHandler(Plugin plugin, string[] message)
		{
			Thread.Sleep(50);
			string text = CustomAnnouncements.ReplaceVariables(CustomAnnouncements.SpacePeriods(CustomAnnouncements.StringArrayToString(message, 0)));
			PluginManager.Manager.Server.Map.AnnounceCustomMessage(text);
		}
	}
}
