# NTFS Streams

A library for working with alternate data streams on NTFS file systems from .NET applications.

Originally published on CodeProject: http://www.codeproject.com/KB/cs/ntfsstreams.aspx

Now available as a NuGet package: https://www.nuget.org/packages/Trinet.Core.IO.Ntfs/

    Install-Package Trinet.Core.IO.Ntfs


## Introduction

Since NT 3.1, the NTFS file system has supported multiple data-streams for files. There has never been built-in support for viewing or manipulating these additional streams, but the Windows API functions include support for them with a special file syntax: `Filename.ext:StreamName`. Even Win9x machines can access the alternative data streams of files on any NTFS volume they have access to, e.g., through a mapped drive. Because the `Scripting.FileSystemObject` and many other libraries call the `CreateFile` API behind the scenes, even scripts have been able to access alternative streams quite easily (although enumerating the existing streams has always been tricky).

In .NET, however, it seems someone decided to add some checking to the format of filenames. If you attempt to open a `FileStream` on an alternative stream, you will get a "Path Format not supported" exception. I have been unable to find any class in the CLR that provides support for alternative data streams, so I decided to roll my own.


## Using the Classes

The `AlternateDataStreamInfo` class represents the details of an individual stream, and provides methods to create, open, or delete the stream.

The static `FileSystem` class provides methods to retrieve the list of streams for a file, retrieve a specific stream from a file, determine whether a stream exists, and delete a specific stream.

All methods on the `FileSystem` class offer overloads which accept either a path or a `FileSystemInfo` object. The overloads which accept a `FileSystemInfo` object can also be invoked as extension methods.


### Example:

    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Trinet.Core.IO.Ntfs;
    
    ...
    
    FileInfo file = new FileInfo(path);
    
    // List the additional streams for a file:
    foreach (AlternateDataStreamInfo s in file.ListAlternateDataStreams())
    {
        Console.WriteLine("{0} - {1} bytes", s.Name, s.Size);
    }
    
    // Read the "Zone.Identifier" stream, if it exists:
    if (file.AlternateDataStreamExists("Zone.Identifier"))
    {
        Console.WriteLine("Found zone identifier stream:");
        
        AlternateDataStreamInfo s = file.GetAlternateDataStream("Zone.Identifier", FileMode.Open);
        using (TextReader reader = s.OpenText())
        {
            Console.WriteLine(reader.ReadToEnd());
        }
        
        // Delete the stream:
        s.Delete();
    }
    else
    {
        Console.WriteLine("No zone identifier stream found.");
    }
    
    // Alternative method to delete the stream:
    file.DeleteAlternateDataStream("Zone.Identifier");


## Files Included

* The *Trinet.Core.IO.Ntfs* folder contains the source code.
* The *doc* folder contains the documentation and FxCop project.
* The *binaries* folder contains the compiled assembly.
* The *other* folder contains a compatibility wrapper for the original version of this code, and a sample to recursively delete the "Zone.Identifier" stream from all files in a given path.


## References

This code was inspired by Dino Esposito's MSDN article from March 2000: 
http://msdn.microsoft.com/en-us/library/ms810604.aspx


## License

Copyright (c) 2002-2010 Richard Deeming
All rights reserved.

This code is free software: you can redistribute it and/or modify it under the terms of either

* the Code Project Open License (CPOL) version 1 or later; or
* the GNU General Public License as published by the Free Software Foundation, version 3 or later; or
* the BSD 2-Clause License;

This code is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
