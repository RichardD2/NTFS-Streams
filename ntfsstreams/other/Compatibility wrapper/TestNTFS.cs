using System;
using System.Drawing;

/// <summary>
/// A console app to test the NTFS class.
/// </summary>
namespace NTFS.Test
{
	class TestNTFS
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			TestStream();
			Console.ReadLine();
		}

		static void TestStream() 
		{
			const string IMAGE_FILE = @"test.jpg";
			const string THUMB_STREAM = "thumb";

			//List all of the alternative streams for the file:
			NTFS.FileStreams FS = new NTFS.FileStreams(IMAGE_FILE);
			Console.WriteLine(FS.FileName);
			foreach(NTFS.StreamInfo s in FS) Console.WriteLine("\t" + s.Name);
			
			
			//Using an alternative stream to store a thumbnail image:
			System.Drawing.Image thm;
			System.IO.FileStream FileStream;
			
			int i = FS.IndexOf(THUMB_STREAM);
			if (i==-1) 
			{
				//Thumbnail stream not found - create and store the thumbnail:
				Console.WriteLine("Creating thumbnail:");
				System.Drawing.Image img = System.Drawing.Image.FromFile(IMAGE_FILE);
				int Width = 100; int Height = 100;
				
				//Maintain aspect ratio:
				int lW = (int)(img.Width * Height / img.Height);
				int lH = (int)(img.Height * Width / img.Width);
				if (lW>Width) Height = lH;
				else if (lH>Height) Width = lW;
				thm = img.GetThumbnailImage(Width, Height, null, IntPtr.Zero);
				
				//Save the thumbnail to the stream
				FS.Add(THUMB_STREAM);
				FileStream = FS[THUMB_STREAM].Open(System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
				thm.Save(FileStream, System.Drawing.Imaging.ImageFormat.Jpeg);
				FileStream.Close();
				Console.WriteLine("Created thumbnail: Size {0}x{1}", thm.Width, thm.Height);
				
			}
			else 
			{
				//Thumbnail stream exists - read the thumbnail back
				Console.WriteLine("Thumbnail already exists!");
				FileStream = FS[THUMB_STREAM].Open(System.IO.FileMode.Open, System.IO.FileAccess.Read);
				thm = System.Drawing.Image.FromStream(FileStream);
				FileStream.Close();
				Console.WriteLine("Read thumbnail: Size {0}x{1}", thm.Width, thm.Height);
				
				//Remove the thumbnail stream, for demo purposes only!
				FS.Remove(THUMB_STREAM);
			}
		}
	}
}