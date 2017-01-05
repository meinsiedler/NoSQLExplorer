using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using NoSqlExplorer.WpfClient.Messages;

namespace NoSqlExplorer.WpfClient
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      Current.DispatcherUnhandledException += (sender, args) =>
      {
        string text;
        if (args.Exception is HttpRequestException)
        {
          text = "A request to a resource failed, because it was most probably not ready yet. Please try your last action again.";
        }
        else
        {
          text = "An unknown error occured. Please try your last action again.";
        }
        Messenger.Default.Send(new SnackbarMessage(text, "DISMISS"));

        args.Handled = true;
      };
    }
  }
}
