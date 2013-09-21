fb2-thumbnails-provider
=======================

Windows 7 x86/x64 compatible shell extension for previewing of thumbnails of FB2 books

The code is based on this article: http://www.codemonkeycodes.com/2010/01/11/ithumbnailprovider-re-visited/
Visual Studio 2012 Express can be used to create a binary file. By default it's requires .NET framework 4.0.
After creating a library file you need to register it(see https://github.com/C4Grey/fb2-thumbnails-provider/blob/master/dll_reg.cmd) and add a registry entries(see https://github.com/C4Grey/fb2-thumbnails-provider/blob/master/FileProvider.reg and https://github.com/C4Grey/fb2-thumbnails-provider/blob/master/FileExtension.reg)
