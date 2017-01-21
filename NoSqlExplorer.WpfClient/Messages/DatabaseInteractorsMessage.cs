using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction;

namespace NoSqlExplorer.WpfClient.Messages
{
  public class DatabaseInteractorsMessage
  {
    public IList<IDatabaseInteractor> DatabaseInteractors { get; }

    public DatabaseInteractorsMessage(IList<IDatabaseInteractor> databaseInteractors)
    {
      DatabaseInteractors = databaseInteractors;
    }
  }
}
