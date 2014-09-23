using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trinet.Core.IO.Ntfs;

static class Program
{
	static void Main(string[] args)
	{
		try
		{
			bool recurse = false;
			bool whatIf = false;
			List<string> directories = new List<string>();
			List<string> files = new List<string>();
			char[] invalidChars = Path.GetInvalidPathChars();
			
			foreach (string value in args)
			{
				if (string.Equals(value, "/recurse", StringComparison.OrdinalIgnoreCase))
				{
					recurse = true;
				}
				else if (string.Equals(value, "/whatif", StringComparison.OrdinalIgnoreCase))
				{
					whatIf = true;
				}
				else if (-1 == value.IndexOfAny(invalidChars))
				{
					if (File.Exists(value))
					{
						files.Add(value);
					}
					else if (Directory.Exists(value))
					{
						directories.Add(value);
					}
					else
					{
						Console.WriteLine("File not found: " + value);
					}
				}
				else
				{
					Console.WriteLine("Unknown argument: " + value);
				}
			}
			
			if (0 != directories.Count)
			{
				SearchOption option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
				files.AddRange(directories.SelectMany(d => Directory.GetFiles(d, "*", option)));
			}
			
			if (0 != files.Count)
			{
				var process = whatIf ? new Func<string, bool>(ProcessFile_WhatIf) : new Func<string, bool>(ProcessFile_Real);
				int count = files.Count(process);
				Console.WriteLine("Processed {0:D} files.", count);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}
	
	static bool ProcessFile_Real(string path)
	{
		bool result = FileSystem.AlternateDataStreamExists(path, ZoneName);
		if (result)
		{
			// Clear the read-only attribute, if set:
			FileAttributes attributes = File.GetAttributes(path);
			if (FileAttributes.ReadOnly == (FileAttributes.ReadOnly & attributes))
			{
				attributes &= ~FileAttributes.ReadOnly;
				File.SetAttributes(path, attributes);
			}
			
			result = FileSystem.DeleteAlternateDataStream(path, ZoneName);
			if (result) Console.WriteLine("Process {0}", path);
		}
		
		return result;
	}
	
	static bool ProcessFile_WhatIf(string path)
	{
		bool result = FileSystem.AlternateDataStreamExists(path, ZoneName);
		if (result) Console.WriteLine("Process {0}", path);
		return result;
	}
	
	const string ZoneName = "Zone.Identifier";
}