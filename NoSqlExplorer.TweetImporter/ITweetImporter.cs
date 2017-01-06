﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.TweetImporter
{
  public interface ITweetImporter
  {
    string ContainerName { get; }
    string Host { get; }

    Task EnsureTableExistsAsync();
    Task BulkInsertAsync(IList<Tweet> tweets);
  }
}