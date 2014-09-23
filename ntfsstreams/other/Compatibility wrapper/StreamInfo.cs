using System;
using System.IO;
using Trinet.Core.IO.Ntfs;

namespace NTFS
{
	public sealed class StreamInfo
	{
		private readonly AlternateDataStreamInfo _stream;
		
		internal StreamInfo(AlternateDataStreamInfo stream)
		{
			_stream = stream;
		}
		
		public string Name
		{
			get { return _stream.Name; }
		}
		
		public long Size
		{
			get { return _stream.Size; }
		}
		
		public override int GetHashCode()
		{
			return _stream.GetHashCode();
		}
		
		public override string ToString()
		{
			return _stream.ToString();
		}
		
		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj)) return false;
			if (object.ReferenceEquals(this, obj)) return true;
			
			StreamInfo info = obj as StreamInfo;
			if (null != info) return _stream.Equals(info._stream);
			
			AlternateDataStreamInfo other = obj as AlternateDataStreamInfo;
			if (null != other) return _stream.Equals(other);
			
			string path = obj as string;
			if (null != path) return string.Equals(path, _stream.FullPath, StringComparison.OrdinalIgnoreCase);
			
			return false;
		}
		
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return _stream.Open(mode, access, share);
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
		
		public bool Delete()
		{
			return _stream.Delete();
		}
	}
}
