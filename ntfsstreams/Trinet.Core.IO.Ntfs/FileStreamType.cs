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

namespace Trinet.Core.IO.Ntfs
{
	/// <summary>
	/// Represents the type of data in a stream.
	/// </summary>
	public enum FileStreamType
	{
		/// <summary>
		/// Unknown stream type.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Standard data.
		/// </summary>
		Data = 1,
		/// <summary>
		/// Extended attribute data.
		/// </summary>
		ExtendedAttributes = 2,
		/// <summary>
		/// Security data.
		/// </summary>
		SecurityData = 3,
		/// <summary>
		/// Alternate data stream.
		/// </summary>
		AlternateDataStream = 4,
		/// <summary>
		/// Hard link information.
		/// </summary>
		Link = 5,
		/// <summary>
		/// Property data.
		/// </summary>
		PropertyData = 6,
		/// <summary>
		/// Object identifiers.
		/// </summary>
		ObjectId = 7,
		/// <summary>
		/// Reparse points.
		/// </summary>
		ReparseData = 8,
		/// <summary>
		/// Sparse file.
		/// </summary>
		SparseBlock = 9,
		/// <summary>
		/// Transactional data.
		/// (Undocumented - BACKUP_TXFS_DATA)
		/// </summary>
		TransactionData = 10,
	}
}
