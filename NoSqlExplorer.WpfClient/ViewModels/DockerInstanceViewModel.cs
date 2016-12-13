using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NoSqlExplorer.DockerAdapter;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class DockerInstanceViewModel : ViewModelBase
  {
    private readonly DockerInstance _dockerInstance;

    public DockerInstanceViewModel(DockerInstance dockerInstance, int number)
    {
      _dockerInstance = dockerInstance;
      Host = _dockerInstance.Host;
      Port = _dockerInstance.Port;
      Number = number;
    }

    private int _number;
    public int Number
    {
      get { return _number; }
      set { Set(ref _number, value); }
    }

    private string _host;
    public string Host
    {
      get { return _host; }
      set { Set(ref _host, value); }
    }

    private int _port;
    public int Port
    {
      get { return _port; }
      set { Set(ref _port, value); }
    }
  }
}
