# device-monitor-sharp
Simple .NET 5 console Application to get some basic system information like CPU load, memory,... 
This project is in an very early stage of development, so some things are a bit hacky currently.

## Todo
- Implement more data

## Settings

Default settings (Client):
```json
{
  "Url": "http://localhost:8000/",
  "EncryptionKey": null,
  "EncryptionEnabled": false,
  "StoreDatabaseInterval": 10000
}
```
Default settings (Server):
```json
{
  "Url": "http://localhost:8000/",
  "EncryptionKey": null
  "EncryptionEnabled": false,
  "StoreDatabaseInterval": 10000,
  "ConnectionString": null
}
```
<b>You need to specify a connection string to your mysql/mariadb database </b>

To enable encryption you can just set ``` EncryptionEnabled=true ``` in the settings.json file. This will generate a custom key for you on the next restart.

## Data-Access
You can acces the data via a simple http-request to the url specified in your settings.

## Opensource Projects used
- EntityFrameworkCore
- Mysql.EntityFrameworkCore
- Newtonsoft.JSON
