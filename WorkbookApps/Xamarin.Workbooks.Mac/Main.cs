﻿//
// Main.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2016 Microsoft. All rights reserved.

using AppKit;
using ObjCRuntime;

namespace Xamarin.Workbooks.Mac
{
	static class MainClass
	{
		static void Main ()
		{
			NSApplication.Init ();

			var menuBar = new NSMenu ();
			var appMenuItem = new NSMenuItem ();
			menuBar.AddItem (appMenuItem);

			var appMenu = new NSMenu ();
			appMenu.AddItem (new NSMenuItem ("Quit", new Selector ("terminate:"), "q"));
			appMenuItem.Submenu = appMenu;

			var app = NSApplication.SharedApplication;
			app.Delegate = new AppDelegate ();
			app.MainMenu = menuBar;
			app.ApplicationIconImage = NSImage.ImageNamed ("AppIcon");
			app.ActivationPolicy = NSApplicationActivationPolicy.Regular;
			app.Run ();
		}
	}
}