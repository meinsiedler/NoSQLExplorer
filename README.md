# NoSQL Explorer

A tool for comparing NoSQL databases, in particular [Crate.io](https://crate.io/) and [MongoDB](https://www.mongodb.com/).

We host your NoSQL databases on Azure using Docker container.

## Setup

### Twitter

For accessing Twitter feeds, you need to provide custom values for the Twitter *Consumer Key* and *Consumer Secret*. Therefore, change the keys in the `NoSqlExplorer.WpfClient` project in the file `Config\Twitter.config`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<twitterConfig>
  <twitterSettings
	feedUrl="https://stream.twitter.com/1.1/statuses/sample.json"
    consumerKey="YOUR-TWITTER-CONSUMER-KEY"
    consumerSecret="YOUR-TWITTER-CONSUMER-SECRET" />
</twitterConfig>
```

To get your *Consumer Key* and *Consumer secret*, you need to create a new Twitter app under <https://apps.twitter.com/>.

### Docker Instances

You need to configure the *host* and *port* as well as *username* and *password* for your Docker instances. This configuration can be found in the `Config\Docker.config` file:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<dockerConfig>
  <dockerInstances>
    <dockerInstance host="YOUR-HOST-1" port="1111" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
    <dockerInstance host="YOUR-HOST-2" port="2222" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
    <dockerInstance host="YOUR-HOST-3" port="3333" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
    <dockerInstance host="YOUR-HOST-4" port="4444" username="YOUR-USERNAME" password="YOUR-PASSWORD" />
  </dockerInstances>
</dockerConfig>
```

### Azure Access

You need to configure the access to your Azure account by providing your subscription ID, the name of the resource group of your Docker containers and the Base64 formatted Azure App Certificate. This configuration can be found in the `Config\Azure.config` file.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<azureConfig>
  <azureSubscription
    subscriptionId="YOUR-AZURE-SUBCRIPTION-ID"
    resourceGroup="NAME-OF-THE-RESOURCEGROUP-CONTAINING-YOUR-DOCKER-INSTANCES"
    base64encodedCertificate="BASE-64-ENCODED-AZURE-APP-CERTIFICATE" />
</azureConfig>
```