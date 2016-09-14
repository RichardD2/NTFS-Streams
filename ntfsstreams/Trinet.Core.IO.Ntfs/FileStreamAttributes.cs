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
	/// Represents the attributes of a file stream.
	/// </summary>
	[Flags]
	public enum FileStreamAttributes
	{
		/// <summary>
		/// No attributes.
		/// </summary>
		None = 0,
		/// <summary>
		/// Set if the stream contains data that is modified when read.
		/// </summary>
		ModifiedWhenRead = 1,
		/// <summary>
		/// Set if the stream contains security data.
		/// </summary>
		ContainsSecurity = 2,
		/// <summary>
		/// Set if the stream contains properties.
		/// </summary>
		ContainsProperties = 4,
		/// <summary>
		/// Set if the stream is sparse.
		/// </summary>
		Sparse = 8,
	}
}
