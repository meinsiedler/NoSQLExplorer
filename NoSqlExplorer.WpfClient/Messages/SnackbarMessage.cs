using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.WpfClient.Messages
{
  public class SnackbarMessage
  {
    public SnackbarMessage(string text)
    {
      Text = text;
    }

    public SnackbarMessage(string text, string action)
    {
      Text = text;
      Action = action;
    }

    public string Text { get; }
    public string Action { get; }
  }
}
