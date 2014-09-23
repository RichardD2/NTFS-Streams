using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Trinet.Core.IO.Ntfs;

namespace NTFS
{
	public sealed class FileStreams : Collection<StreamInfo>
	{
		private readonly FileSystemInfo _file;
		
		public FileStreams(FileSystemInfo file)
		{
			if (null == file) throw new ArgumentNullException("file");
			
			_file = file;
			
			foreach (AlternateDataStreamInfo info in FileSystem.ListAlternateDataStreams(file))
			{
				Items.Add(new StreamInfo(info));
			}
		}
		
		public FileStreams(string path) : this(CreateFile(path))
		{
		}
		
		public FileSystemInfo FileInfo
		{
			get { return _file; }
		}
		
		public string FileName
		{
			get { return _file.FullName; }
		}
		
		public long FileSize
		{
			get
			{
				FileInfo file = _file as FileInfo;
				if (null != file) return file.Length;
				return 0L;
			}
		}
		
		public long Size
		{
			get
			{
				long value = this.FileSize;
				if (0 != this.Count)
				{
					foreach (StreamInfo info in this.Items)
					{
						value += info.Size;
					}
				}
				
				return value;
			}
		}
		
		public StreamInfo this[string streamName]
		{
			get
			{
				int index = this.IndexOf(streamName);
				if (-1 != index) return this.Items[index];
				return null;
			}
		}
		
		private static FileSystemInfo CreateFile(string path)
		{
			if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
			
			path = Path.GetFullPath(path);
			if (!File.Exists(path) && Directory.Exists(path)) return new DirectoryInfo(path);
			return new FileInfo(path);
		}
		
		public int IndexOf(string streamName)
		{
			int result = -1;
			if (0 != this.Count)
			{
				int index = 0;
				foreach (StreamInfo item in this.Items)
				{
					if (string.Equals(streamName, item.Name, StringComparison.OrdinalIgnoreCase))
					{
						result = index;
						break;
					}
					
					index++;
				}
			}
			
			return result;
		}
		
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			if (!(_file is FileInfo)) throw new InvalidOperationException();
			return File.Open(_file.FullName, mode, access, share);
		}
		
		public FileStream Open(FileMode mode, FileAccess access)
		{
			return this.Open(mode, access, FileShare.None);
		}
		
		public FileStream Open(FileMode mode)
		{
			return this.Open(mode, FileAccess.ReadWrite, FileShare.None);
		}
		
		public FileStream Open()
		{
			return this.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
		}
		
		public void Delete()
		{
			_file.Delete();
			this.Items.Clear();
		}
		
		public int Add(string streamName)
		{
			int index = this.IndexOf(streamName);
			if (-1 == index)
			{
				StreamInfo item = new StreamInfo(FileSystem.GetAlternateDataStream(_file, streamName, FileMode.OpenOrCreate));
				
				index = this.Items.Count;
				this.InsertItem(index, item);
			}
			
			return index;
		}
		
		public bool Remove(string streamName)
		{
			int index = this.IndexOf(streamName);
			if (-1 == index) return false;
			
			this.RemoveAt(index);
			return true;
		}
		
		protected override void ClearItems()
		{
			if (0 != this.Count)
			{
				foreach (StreamInfo item in this.Items)
				{
					item.Delete();
				}
			}
			
			base.ClearItems();
		}
		
		protected override void RemoveItem(int index)
		{
			this[index].Delete();
			base.RemoveItem(index);
		}
	}
}
