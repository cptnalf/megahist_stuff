﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TFSTree")]
[assembly: AssemblyDescription("TFSTree is a viewer for the distributed version control system monotone. As a viewer it loads and reads monotone's database to create reports. As it is a Windows application with a graphical user interface it can help to understand monotone's data easier and more quickly.")]

#if DEBUG
#if ANYCPU
[assembly: AssemblyConfiguration("Debug|AnyCPU")]
#else
[assembly: AssemblyConfiguration("Debug|??")]
#endif
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("modified (heavily) by SAA and Associates")]
[assembly: AssemblyProduct("TFSTree")]
[assembly: AssemblyCopyright("Copyright © 2007 Boris Schaeling")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("83eb00d4-04e2-4d9c-981c-b00a8cedc83a")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("0.4.0.0")]
//[assembly: AssemblyFileVersion("0.3.0.0")]
