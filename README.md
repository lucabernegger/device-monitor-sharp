# device-monitor-sharp
Simple .NET 5 console Application to get some basic system information like CPU load, memory,... 
This project is in an very early stage of development, so some things are a bit hacky currently.

## Todo
- Implment more data
- Website to display json data?

## Settings

Default settings:
```json
{
  "Url": "http://localhost:8000/",
  "EncryptionKey": null,
  "EncryptionEnabled": false,
  "StoreDatabaseInterval": 10000
}
```
To enable encryption you can just set ``` EncryptionEnabled=true ``` in the settings.json file. This will generate a custom key for you on the next restart.

<b>If you activate encryption later, all data until activation will be saved unencrypted in the database, and could so be possibly get sent by the webserver unencypted!</b>
## Data-Access
You can acces the data via a simple http-request to the url specified in your settings.
#### Possible query parameters
- ``` from ```: UNIX-Timestamp from which date until now you want the data (sorted from newest to oldest)
- ``` limit ```: You can specify a limit on how much entries you want to get returned (sorted from newest to oldest)

## Opensource Projects used
- LiteDB
- Newtonsoft.JSON
