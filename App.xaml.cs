﻿using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
using System.Windows;

namespace DesktopNote
{
    public partial class App : Application
    {
        //Private Declare Unicode Function PathIsNetworkPath Lib "shlwapi" Alias "" (ByVal pszPath As String) As Boolean
        [System.Runtime.InteropServices.DllImport("shlwapi.dll")]
        private static extern bool PathIsNetworkPathW(string pszPath);

        private void RunCheck(object sender1, StartupEventArgs e1)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs e) =>
              {
                  var desiredAssembly = new System.Reflection.AssemblyName(e.Name).Name;
                  if (desiredAssembly == "Xceed.Wpf.Toolkit")
                  {
                      var ressourceName = "DesktopNote.Resources." + desiredAssembly + ".dll";
                      using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(ressourceName))
                      {
                          byte[] assemblyData = new byte[stream.Length];
                          stream.Read(assemblyData, 0, assemblyData.Length);
                          return System.Reflection.Assembly.Load(assemblyData);
                      }
                  }
                  else
                      return null;
              };

            if (PathIsNetworkPathW(System.AppDomain.CurrentDomain.BaseDirectory))
            {
                MessageBox.Show((string)Resources["msgbox_run_from_network"], "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Current.Shutdown();
                return;
            }

            //if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            //{
            //    MessageBox.Show("Only one instance of DesktopNote can be running.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    Current.Shutdown();
            //    return;
            //}

            //localization
            var dict = new ResourceDictionary();
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2))
            {
                case "zh":
                    dict.Source = new Uri(@"Resources\StringResources.zh.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri(@"Resources\StringResources.xaml", UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(dict);

            var mainwin = new MainWindow();
            mainwin.Show();
        }
    }
}
