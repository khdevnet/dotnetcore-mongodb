```
docker run -p 27017:27017 -d mongo
docker run --network transaction-2pc_default -e ME_CONFIG_MONGODB_SERVER=transaction-2pc_mongo_1 -p 8081:8081 mongo-express
```