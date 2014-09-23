/**
  * Trinet.Core.IO.Ntfs - Utilities for working with alternate data streams on NTFS file systems.
  * Copyright (C) 2002-2010 Richard Deeming
  * 
  * This code is free software: you can redistribute it and/or modify it under the terms of either
  * - the Code Project Open License (CPOL) version 1 or later; or
  * - the GNU General Public License as published by the Free Software Foundation, version 3 or later; or
  * - the BSD 2-Clause License;
  * 
  * This code is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
  * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
  * See the license files for details.
  * 
  * You should have received a copy of the licenses along with this code. 
  * If not, see <http://www.codeproject.com/info/cpol10.aspx>, <http://www.gnu.org/licenses/> 
  * and <http://opensource.org/licenses/bsd-license.php>.
*/

using System;
using System.Runtime.InteropServices;

namespace Trinet.Core.IO.Ntfs
{
	internal sealed class StreamName : IDisposable
	{
		#region Private Data

		private static readonly SafeHGlobalHandle _invalidBlock = SafeHGlobalHandle.Invalid();
		private SafeHGlobalHandle _memoryBlock = _invalidBlock;

		#endregion

		#region Properties

		/// <summary>
		/// Returns the handle to the block of memory.
		/// </summary>
		/// <value>
		/// The <see cref="SafeHGlobalHandle"/> representing the block of memory.
		/// </value>
		public SafeHGlobalHandle MemoryBlock
		{
			get { return _memoryBlock; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Performs application-defined tasks associated with freeing, 
		/// releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (!_memoryBlock.IsInvalid)
			{
				_memoryBlock.Dispose();
				_memoryBlock = _invalidBlock;
			}
		}

		/// <summary>
		/// Ensures that there is sufficient memory allocated.
		/// </summary>
		/// <param name="capacity">
		/// The required capacity of the block, in bytes.
		/// </param>
		/// <exception cref="OutOfMemoryException">
		/// There is insufficient memory to satisfy the request.
		/// </exception>
		public void EnsureCapacity(int capacity)
		{
			int currentSize = _memoryBlock.IsInvalid ? 0 : _memoryBlock.Size;
			if (capacity > currentSize)
			{
				if (0 != currentSize) currentSize <<= 1;
				if (capacity > currentSize) currentSize = capacity;

				if (!_memoryBlock.IsInvalid) _memoryBlock.Dispose();
				_memoryBlock = SafeHGlobalHandle.Allocate(currentSize);
			}
		}

		/// <summary>
		/// Reads the Unicode string from the memory block.
		/// </summary>
		/// <param name="length">
		/// The length of the string to read, in characters.
		/// </param>
		/// <returns>
		/// The string read from the memory block.
		/// </returns>
		public string ReadString(int length)
		{
			if (0 >= length || _memoryBlock.IsInvalid) return null;
			if (length > _memoryBlock.Size) length = _memoryBlock.Size;
			return Marshal.PtrToStringUni(_memoryBlock.DangerousGetHandle(), length);
		}

		/// <summary>
		/// Reads the string, and extracts the stream name.
		/// </summary>
		/// <param name="length">
		/// The length of the string to read, in characters.
		/// </param>
		/// <returns>
		/// The stream name.
		/// </returns>
		public string ReadStreamName(int length)
		{
			string name = ReadString(length);
			if (!string.IsNullOrEmpty(name))
			{
				// Name is of the format ":NAME:$DATA\0"
				int separatorIndex = name.IndexOf(SafeNativeMethods.StreamSeparator, 1);
				if (-1 != separatorIndex)
				{
					name = name.Substring(1, separatorIndex - 1);
				}
				else
				{
					// Should never happen!
					separatorIndex = name.IndexOf('\0');
					if (1 < separatorIndex)
					{
						name = name.Substring(1, separatorIndex - 1);
					}
					else
					{
						name = null;
					}
				}
			}

			return name;
		}

		#endregion
	}
}
