// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Xamarin.Interactive.Client.Mac
{
	[Register ("NewWorkbookPrimaryChoiceItem")]
	partial class NewWorkbookPrimaryChoiceItem
	{
		[Outlet]
		AppKit.NSImageView imageView { get; set; }

		[Outlet]
		AppKit.NSTextField label { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imageView != null) {
				imageView.Dispose ();
				imageView = null;
			}

			if (label != null) {
				label.Dispose ();
				label = null;
			}
		}
	}
}
