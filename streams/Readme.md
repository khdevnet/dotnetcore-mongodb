# Mongodb Streams
### Steps
* Create https://cloud.mongodb.com and init mongodb sample data
* Add connection to 
```
 new MFlixNoSqlDbContext(
                "mongodb+srv://######:########@freecluster-omk9a.mongodb.net/test?retryWrites=true&w=majority",
                "sample_mflix"
                );
```
* Run application
* using robomongo insert new document in movie collection
* changes will display in UI

```
use cloudmongodb
https://cloud.mongodb.com
```