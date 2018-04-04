/*
  * Trinet.Core.IO.Ntfs - Utilities for working with alternate data streams on NTFS file systems.
  * Copyright (C) 2002-2016 Richard Deeming
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
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Trinet.Core.IO.Ntfs
{
	/// <summary>
	/// A <see cref="SafeHandle"/> for a global memory allocation.
	/// </summary>
	internal sealed class SafeHGlobalHandle : SafeHandle
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.
		/// </summary>
		/// <param name="toManage">
		/// The initial handle value.
		/// </param>
		/// <param name="size">
		/// The size of this memory block, in bytes.
		/// </param>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private SafeHGlobalHandle(IntPtr toManage, int size) : base(IntPtr.Zero, true)
		{
			Size = size;
			SetHandle(toManage);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.
		/// </summary>
		private SafeHGlobalHandle() : base(IntPtr.Zero, true)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the handle value is invalid.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if the handle value is invalid;
		/// otherwise, <see langword="false"/>.
		/// </value>
		public override bool IsInvalid => IntPtr.Zero == handle;

		/// <summary>
		/// Returns the size of this memory block.
		/// </summary>
		/// <value>
		/// The size of this memory block, in bytes.
		/// </value>
		public int Size { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Allocates memory from the unmanaged memory of the process using GlobalAlloc.
		/// </summary>
		/// <param name="bytes">
		/// The number of bytes in memory required.
		/// </param>
		/// <returns>
		/// A <see cref="SafeHGlobalHandle"/> representing the memory.
		/// </returns>
		/// <exception cref="OutOfMemoryException">
		/// There is insufficient memory to satisfy the request.
		/// </exception>
		public static SafeHGlobalHandle Allocate(int bytes)
		{
			return new SafeHGlobalHandle(Marshal.AllocHGlobal(bytes), bytes);
		}

		/// <summary>
		/// Returns an invalid handle.
		/// </summary>
		/// <returns>
		/// An invalid <see cref="SafeHGlobalHandle"/>.
		/// </returns>
		public static SafeHGlobalHandle Invalid()
		{
			return new SafeHGlobalHandle();
		}

		/// <summary>
		/// Executes the code required to free the handle.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the handle is released successfully;
		/// otherwise, in the event of a catastrophic failure, <see langword="false"/>.
		/// In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			Marshal.FreeHGlobal(handle);
			return true;
		}

		#endregion
	}
}
