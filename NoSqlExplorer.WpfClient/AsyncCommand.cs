using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace NoSqlExplorer.WpfClient
{
  public class AsyncCommand : AsyncCommand<object>
  {
    public AsyncCommand(Func<Task> executedHandler, Func<bool> canExecuteHandler = null)
      : base(o => executedHandler(), canExecuteHandler)
    {
    }
  }

  public class AsyncCommand<T> : RelayCommand
  {
    private readonly Func<T, Task> _executeHandler;
    private bool _isExecuting;

    public AsyncCommand(Func<T, Task> executedHandler, Func<bool> canExecuteHandler = null)
      : base(() => { }, canExecuteHandler)
    {
      if (executedHandler == null)
      {
        throw new ArgumentNullException("executedHandler");
      }

      this._executeHandler = executedHandler;
    }

    public new bool CanExecute(object parameter)
    {
      return base.CanExecute(parameter) || !_isExecuting;
    }

    public override async void Execute(object parameter)
    {
      await ExecuteAsync((T)parameter);
    }

    private async Task ExecuteAsync(T parameter)
    {
      try
      {
        _isExecuting = true;
        RaiseCanExecuteChanged();
        await _executeHandler(parameter);
      }
      finally
      {
        _isExecuting = false;
        RaiseCanExecuteChanged();
      }
    }
  }
}
