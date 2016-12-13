# NoSQL Explorer

A tool for comparing NoSQL databases, in particular [Crate.io](https://crate.io/) and [MongoDB](https://www.mongodb.com/).

We host your NoSQL databases on Azure using Docker container.

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

### Docker Instances

You need to configure the *host* and *port* as well as *username* and *password* for your Docker instances. This configuration can be found in the `App.config` file in the section `<dockerConfig>`:

```xml
<dockerConfig>
  <dockerInstances>
    <dockerInstance host="YOUR-HOST-1" port="1111" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
    <dockerInstance host="YOUR-HOST-2" port="2222" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
    <dockerInstance host="YOUR-HOST-3" port="3333" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
    <dockerInstance host="YOUR-HOST-4" port="4444" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
  </dockerInstances>
</dockerConfig>
```