//
// Author:
//   Sandy Armstrong <sandy@xamarin.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Interactive.Logging;
using Xamarin.Interactive.MTouch;

// Define these for LogEntry since we do not actually need Newtonsoft.Json in simchecker.
namespace Newtonsoft.Json
{
    sealed class JsonObjectAttribute : Attribute
    {
    }

    sealed class JsonConstructorAttribute : Attribute
    {
    }
}

namespace Xamarin.Interactive.Mac.SimChecker
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            if (args.Length == 1 && args [0] == "--version") {
                Console.WriteLine (BuildInfo.VersionString);
                Environment.Exit (0);
                return;
            }

            Log.Initialize (new LogProvider (LogLevel.Debug));

            string sdkRoot;
            try {
                sdkRoot = await MTouchSdkTool.GetXcodeSdkRootAsync ();
            } catch (Exception e) {
                Console.Error.WriteLine (e.Message);
                Environment.Exit (100); // Xcode not configured in XS or not installed at /Applications/Xcode.app
                return;
            }

            var xcodeVersion = MTouchSdkTool.GetXcodeVersion (sdkRoot);
            if (xcodeVersion < MTouchSdkTool.RequiredMinimumXcodeVersion) {
                Environment.Exit (105); // Xcode too old
                return;
            }

            MTouchListSimXml mtouchList;
            try {
                mtouchList = await MTouchSdkTool.MtouchListSimAsync (sdkRoot);
            } catch (Exception e) {
                e = (e as AggregateException)?.InnerExceptions?.FirstOrDefault () ?? e;
                Console.Error.WriteLine (e.Message);
                if (e is FileNotFoundException)
                    Environment.Exit (101); // mlaunch (Xamarin Studio) not installed
                else
                    Environment.Exit (102); // Error running mlaunch
                return;
            }

            IEnumerable<MTouchListSimXml.SimDeviceElement> compatibleDevices;
            try {
                compatibleDevices = MTouchSdkTool.GetCompatibleDevices (mtouchList);
            } catch (Exception e) {
                Console.Error.WriteLine (e.Message);
                Environment.Exit (103); // Invalid mlaunch output
                return;
            }

            var firstCompatibleDevice = compatibleDevices?.FirstOrDefault ();
            if (firstCompatibleDevice == null) {
                Console.Error.WriteLine ("No compatible simulator devices installed");
                Environment.Exit (104); // No compatible sim listed by mlaunch
            }

            Console.WriteLine ($"UDID: {firstCompatibleDevice.UDID}");
        }
    }
}
