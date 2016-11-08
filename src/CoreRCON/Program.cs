﻿using CoreRCON.PacketFormats;
using System;
using System.Threading.Tasks;

namespace CoreRCON
{
	internal class ExampleProgram
	{
		/// <summary>
		/// Example program for CoreRCON.
		/// </summary>
		internal static void Main(string[] args)
		{
			var task = Task.Run(async () =>
			{
				var rcon = new RCON();
				await rcon.ConnectAsync("192.168.1.8", 27015, "rcon");
				await rcon.StartLogging("192.168.1.8");

				// Listen for chat messages
				rcon.Listen<Parsers.Standard.ChatMessage>(chat =>
				{
					Console.WriteLine($"Chat message: {chat.Player.Name} said {chat.Message} on channel {chat.Channel}");
				});

				// Listen for kills
				rcon.Listen<Parsers.Standard.KillFeed>(kill =>
				{
					Console.WriteLine($"Player {kill.Killer.Name} ({kill.Killer.Team}) killed {kill.Killed.Name} ({kill.Killed.Team}) with {kill.Weapon}");
				});

				// Listen to all raw responses as strings
				rcon.Listen(raw =>
				{
					Console.WriteLine($"Received a raw string: {raw.Truncate(100).Replace("\n", "")}");
				});

				// Listen to all raw responses, but get their full packets
				rcon.Listen((LogAddressPacket packet) =>
				{
					Console.WriteLine($"Received a LogAddressPacket: Time - {packet.Timestamp} Body - {packet.Body}");
				});

				// Reconnect if the connection is ever lost
				await rcon.KeepAliveAsync();
			});

			// .Wait() puts exceptions into an AggregateException, while .GetResult() doesn't
			task.GetAwaiter().GetResult();
		}
	}
}