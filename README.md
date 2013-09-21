fb2-thumbnails-provider
=======================

Windows 7 x86/x64 compatible shell extension for previewing of thumbnails of FB2 books

The code is based on this article: http://www.codemonkeycodes.com/2010/01/11/ithumbnailprovider-re-visited/
Visual Studio 2012 Express can be used to create a binary file. By default it's requires .NET framework 4.0.
After creating a library file you need to register it:

x86: %windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm /codebase bin\Release\FBThumbs.dll

x64: %windir%\Microsoft.NET\Framework64\v4.0.30319\RegAsm /codebase bin\Release\FBThumbs.dll

After that you need to add registry entries:
[HKEY_CLASSES_ROOT\.fb2\shellex\{e357fccd-a995-4576-b01f-234630154e96}]
@="{YOUR-GUID}"

[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved]
"{YOUR-GUID}"="FB2 Thumbnail Provider"

