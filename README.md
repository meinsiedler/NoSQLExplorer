# NoSQL Explorer

A tool for comparing NoSQL databases, in particular [Crate.io](https://crate.io/) and [MongoDB](https://www.mongodb.com/).

We host your NoSQL databases on Azure using Docker images.

## Setup

### Twitter

For accessing Twitter feeds, you need to provide custom values for the Twitter *Consumer Key* and *Consumer Secret*. Therefore, change the keys in the `NoSqlExplorer.WpfClient` project in the file `Secrets.config`:

```xml
<?xml version="1.0"?>
<appSettings>
  <add key="twitter:consumer-key" value="YOUR-TWITTER-CONSUMER-KEY"/>
  <add key="twitter:consumer-secret" value="YOUR-TWITTER-CONSUMER-SECRET"/>
</appSettings>
```

To get your *Consumer Key* and *Consumer secret*, you need to create a new Twitter app under <https://apps.twitter.com/>.