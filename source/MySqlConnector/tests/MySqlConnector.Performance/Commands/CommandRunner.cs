﻿using System;

namespace MySqlConnector.Performance.Commands
{
	public static class CommandRunner
	{
		public static void Help()
		{
			Console.Error.WriteLine(@"dotnet run
concurrency [iterations] [concurrency] [operations]
-h, --help		 show this message
");
		}

		public static int Run(string[] args)
		{
			var cmd = args[0];

			try
			{
				switch (cmd)
				{
					case "concurrency":
						if (args.Length != 4)
							goto default;
						ConcurrencyCommand.Run(int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]));
						break;
					case "-h":
					case "--help":
						Help();
						break;
					default:
						Help();
						return 1;
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				return 1;
			}
			return 0;
		}
	}
}
