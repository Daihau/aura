﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using System;
using System.Collections.Generic;
using System.Linq;

namespace Aura.Shared.Util.Commands
{
	/// <summary>
	/// Console command manager
	/// </summary>
	public class ConsoleCommands : CommandManager<ConsoleCommand, ConsoleCommandFunc>
	{
		public ConsoleCommands()
		{
			_commands = new Dictionary<string, ConsoleCommand>();

			this.Add("help", "Displays this help", HandleHelp);
			this.Add("exit", "Closes application/server", HandleExit);
			this.Add("status", "Displays application status", HandleStatus);
		}

		/// <summary>
		/// Adds new command handler.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="handler"></param>
		protected void Add(string name, string description, ConsoleCommandFunc handler)
		{
			this.Add(name, "", description, handler);
		}

		/// <summary>
		/// Adds new command handler.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="usage"></param>
		/// <param name="description"></param>
		/// <param name="handler"></param>
		protected void Add(string name, string usage, string description, ConsoleCommandFunc handler)
		{
			if (_commands.ContainsKey(name))
				Log.Warning("Console command '{0}' is being overwritten.", name);

			_commands[name] = new ConsoleCommand(name, usage, description, handler);
		}

		/// <summary>
		/// Waits and parses commands till "exit" is entered.
		/// </summary>
		public void Wait()
		{
			Log.Info("Type 'help' for a list of console commands.");

			var line = "";
			while (true)
			{
				line = Console.ReadLine();

				var args = this.ParseLine(line);
				if (args.Length == 0)
					continue;

				var command = this.GetCommand(args[0]);
				if (command == null)
				{
					Log.Info("Unknown command '{0}'", args[0]);
					continue;
				}

				var result = command.Func(line, args);
				if (result == CommandResult.Break)
				{
					break;
				}
				else if (result == CommandResult.Fail)
				{
					Log.Error("Failed to run command '{0}'.", command.Name);
				}
				else if (result == CommandResult.InvalidArgument)
				{
					Log.Info("Usage: {0} {1}", command.Name, command.Usage);
				}
			}
		}

		private CommandResult HandleHelp(string command, string[] args)
		{
			var maxLength = _commands.Values.Max(a => a.Name.Length);

			Log.Info("Available commands");
			foreach (var cmd in _commands.Values)
				Log.Info("  {0,-" + (maxLength + 2) + "}{1}", cmd.Name, cmd.Description);

			return CommandResult.Okay;
		}

		private CommandResult HandleStatus(string command, string[] args)
		{
			Log.Status("Memory Usage: {0} KB", Math.Round(GC.GetTotalMemory(false) / 1024f));

			return CommandResult.Okay;
		}

		protected virtual CommandResult HandleExit(string command, string[] args)
		{
			CliUtil.Exit(0, false);

			return CommandResult.Okay;
		}
	}

	public class ConsoleCommand : Command<ConsoleCommandFunc>
	{
		public ConsoleCommand(string name, string usage, string description, ConsoleCommandFunc func)
			: base(name, usage, description, func)
		{
		}
	}

	public delegate CommandResult ConsoleCommandFunc(string command, string[] args);
}