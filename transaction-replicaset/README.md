# mongodb
```
docker-compose up --force-recreate
docker-compose rm -fv mongo_one # remove docker compose cache 
docker-compose rm -fv mongo_two

docker exec -it transaction-replicaset_mongo_one_1 mongo # config cluster

config = {
      "_id" : "rs0",
      "members" : [
          {
              "_id" : 0,
              "host" : "transaction-replicaset_mongo_one_1:27017"
          },
          {
              "_id" : 1,
              "host" : "transaction-replicaset_mongo_two_1:27017"
          }
      ]
  }

rs.initiate(config)

docker run --network transaction-replicaset_default -e ME_CONFIG_MONGODB_SERVER=transaction-replicaset_mongo_one_1,transaction-replicaset_mongo_two_1 -p 8081:8081 mongo-express
docker run --network transaction-replicaset_default -p 8080:8080 adminer

# configure windows hosts to enable replicaset from mongo client
127.0.0.1  transaction-replicaset_mongo_one_1
127.0.0.1  transaction-replicaset_mongo_two_1

```

## Transactional ntfs (deprecated)
Transactional NTFS (TxF) allows file operations on an NTFS file system volume to be performed in a transaction. TxF transactions increase application reliability by protecting data integrity across failures and simplify application development by greatly reducing the amount of error handling code.

#### Transactional ntfs alternatives
* Extensible Storage Engine (ESE) is an advanced indexed and sequential access method (ISAM) storage technology. ESE enables applications to store and retrieve data from tables using indexed or sequential cursor navigation. It supports denormalized schemas including wide tables with numerous sparse columns, multi-valued columns, and sparse and rich indexes. It enables applications to enjoy a consistent data state using transacted data update and retrieval.

* MSSQL FILESTREAM storage is implemented as a varbinary(max) column in which the data is stored as BLOBs in the file system. The sizes of the BLOBs are limited only by the volume size of the file system. The standard varbinary(max) limitation of 2-GB file sizes does not apply to BLOBs that are stored in the file system.

# Resources
* [transactional-ntfs](https://docs.microsoft.com/en-us/windows/win32/fileio/transactional-ntfs-portal)
