```
docker run -p 27017:27017 -d mongo
docker run --network transaction-2pc_default -e ME_CONFIG_MONGODB_SERVER=transaction-2pc_mongo_1 -p 8081:8081 mongo-express
```

## Aggregation
### $group
Groups input documents by the specified _id expression and for each distinct grouping, outputs a document. 
```
db.getCollection('products').aggregate([{ "$group": { "_id" : "all", "sum": { "$sum" : 1  } }}])
## group by field
db.getCollection('products').aggregate([{ "$group": { "_id" : "$sku", "sum": { "$sum" : "$qty"  } }}])

## group by embeded document field
Sample: { "product": 1, "item": { "qty": 1 } }
db.getCollection('products').aggregate([{ "$group": { "_id" : "$sku", "sum": { "$sum" : "$item.qty"  } }}])

## group addToSet group items in collection
Sample: { "product": 1, "item": { "qty": 1 } }
db.getCollection('products').aggregate([{ "$group": { "_id" : "$qty", "skus": { "$addToSet" : "$sku"  } }}])

## group push group items in collection
Sample: { "product": 1, "item": { "qty": 1 } }
db.getCollection('products').aggregate([{ "$group": { "_id" : "$qty", "skus": { "$push" : "$sku"  } }}])

## group first/last document in a group 
Sample: { "product": 1, "item": { "qty": 1 } }
db.getCollection('products').aggregate([{ "$group": { "_id" : "$sku", "sample": { "$first/last" : "$item"  } }}])
```
* $unwind: Deconstructs an array field from the input documents to output a document for each element. Each output document is the input document with the value of the array field replaced by the element.
* $project: Passes along the documents with the requested fields to the next stage in the pipeline. The specified fields can be existing fields from the input documents or newly computed fields.
* $limit: Limits the number of documents passed to the next stage in the pipeline.
* $skip: Skips over the specified number of documents that pass into the stage and passes the remaining documents to the next stage in the pipeline.
* $sort: Sorts all input documents and returns them to the pipeline in sorted order.
* $match: Filters the documents to pass only the documents that match the specified condition(s) to the next pipeline stage.

### Resources
* [mongodb .NET expressions](https://mongodb.github.io/mongo-csharp-driver/2.4/reference/driver/expressions/)