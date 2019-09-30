# mongodb
```
docker-compose up --force-recreate
docker-compose rm -fv mongo # remove docker compose cache 

docker run --network transaction-replicaset_default -e ME_CONFIG_MONGODB_SERVER=transaction-replicaset_mongo_one_1,transaction-replicaset_mongo_two_1 -p 8081:8081 mongo-express
docker run --network transaction-replicaset_default -p 8080:8080 adminer

Add-Migration AddSaga -OutputDir "Sql/Migrations"
```

# Resources
* [transactional-ntfs](https://docs.microsoft.com/en-us/windows/win32/fileio/transactional-ntfs-portal)
