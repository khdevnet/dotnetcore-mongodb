# dotnetcore-mongodb
```
docker-compose up --force-recreate
docker-compose rm -fv mongo-one # remove docker compose cache 

docker exec -it dotnetcore-mongodb_mongo-one_1 mongo # config cluster

config = {
      "_id" : "rs0",
      "members" : [
          {
              "_id" : 0,
              "host" : "dotnetcore-mongodb_mongo-one_1:27017"
          },
          {
              "_id" : 1,
              "host" : "dotnetcore-mongodb_mongo-two_1:27017"
          }
      ]
  }

rs.initiate(config)

docker run --network dotnetcore-mongodb_default -e ME_CONFIG_MONGODB_SERVER=dotnetcore-mongodb_mongo-one_1,dotnetcore-mongodb_mongo-two_1 -p 8081:8081 mongo-express

# configure windows hosts to enable replicaset from mongo client
127.0.0.1  dotnetcore-mongodb_mongo-one_1
127.0.0.1  dotnetcore-mongodb_mongo-two_1

```

## Transactional ntfs (deprecated)
Transactional NTFS (TxF) allows file operations on an NTFS file system volume to be performed in a transaction. TxF transactions increase application reliability by protecting data integrity across failures and simplify application development by greatly reducing the amount of error handling code.

#### Transactional ntfs alternatives
* Extensible Storage Engine (ESE) is an advanced indexed and sequential access method (ISAM) storage technology. ESE enables applications to store and retrieve data from tables using indexed or sequential cursor navigation. It supports denormalized schemas including wide tables with numerous sparse columns, multi-valued columns, and sparse and rich indexes. It enables applications to enjoy a consistent data state using transacted data update and retrieval.

* MSSQL FILESTREAM storage is implemented as a varbinary(max) column in which the data is stored as BLOBs in the file system. The sizes of the BLOBs are limited only by the volume size of the file system. The standard varbinary(max) limitation of 2-GB file sizes does not apply to BLOBs that are stored in the file system.

### MongoDb notes 
* **Padding document** 
When MongoDB has to move a document, it bumps the collection’s padding factor,
which is the amount of extra space MongoDB leaves around new documents to give
them room to grow. You can see the padding factor by running db.coll.stats(). Be‐
fore doing the update above, the "paddingFactor" field will be 1: allocate exactly the
size of the document for each new document, as shown in Figure 3-1. If you run it again
after making one of the documents larger (as shown in Figure 3-2), you’ll see that it has
grown to around 1.5: each new document will be given half of its size in free space to
grow. If subsequent updates cause more moves, the padding factor will continue to grow
(although not as dramatically as it did on the first move). If there aren’t more moves,
the padding factor will slowly go down, as shown in Figure 3-3.

If your document has one field that grows, try to keep is as the last field in the document
(but before "garbage"). It is slightly more efficient for MongoDB not to have to rewrite
fields after "tags" if it grows.


* **Cardinality** is how many references a collection has to another collection. Common
relationships are one-to-one, one-to-many, or many-to-many. For example, suppose we
had a blog application. Each post has a title, so that’s a one-to-one relationship. Each
author has many posts, so that’s a one-to-many relationship. And posts have many tags
and tags refer to many posts, so that’s a many-to-many relationship.
When using MongoDB, it can be conceptually useful to split “many” into subcategories:
“many” and “few.” 

For example, you might have a one-to-few cardinality between
authors and posts: each author only writes a few posts. You might have many-to-few
relation between blog posts and tags: your probably have many more blog posts than
you have tags. However, you’d have a one-to-many relationship between blog posts and
comments: each post has many comments.

* **Removing old data**

emoving Old Data
Some data is only important for a brief time: after a few weeks or months it is just wasting
storage space. There are three popular options for removing old data: capped collections,
TTL collections, and dropping collections per time period.
The easiest option is to use a capped collection: set it to a large size and let old data “fall
off” the end. However, capped collections pose certain limitations on the operations
you can do and are vulnerable to spikes in traffic, temporarily lowering the length of
time that they can hold. See “Capped Collections” on page 109 for more information.
The second option is TTL collections: this gives you a finer-grain control over when
documents are removed. However, it may not be fast enough for very high-write-volume
collections: it removes documents by traversing the TTL index the same way a user-
requested remove would. If TTL collections can keep up, though, they are probably the
easiest solution. See “Time-To-Live Indexes” on page 114 for more information about
TTL indexes.
The final option is to use multiple collections: for example, one collection per month.
Every time the month changes, your application starts using this month’s (empty) col‐
lection and searching for data in both the current and previous months’ collections.
Once a collection is older than, say, six months, you can drop it. This can keep up with
nearly any volume of traffic, but it is more complex to build an application around, since
it has to use dynamic collection (or database) names and possibly query multiple
databases

* **Planing databases and collections**
For databases, the big issues to consider are locking (you get a read/write lock per da‐
tabase) and storage. Each database resides in its own files and often its own directory
on disk, which means that you could mount different databases to different volumes.
Thus, you may want all items within a database to be of similar “quality,” similar access
pattern, or similar traffic levels.

For example, suppose we have an application with several components: a logging com‐
ponent that creates a huge amount of not-very-valuable data, a user collection, and a
couple of collections for user-generated data. The user collections are high-value: it is
important that user data is safe. There is also a high-traffic collection for social activities,
which is of lower importance but not quite as unimportant as the logs. This collection
is mainly used for user notifications, so it is almost an append-only collection.
Splitting these up by importance, we might end up with three databases: logs, activi
ties, and users. The nice thing about this strategy is that you may find that your highest-
value data is also your smallest (for instance, users probably don’t generate as much data
as your logging does). You might not be able to afford an SSD for your entire data set,
but you might be able to get one for your users. Or use RAID10 for users and RAID0
for logs and activities.


# Resources
* [transactional-ntfs](https://docs.microsoft.com/en-us/windows/win32/fileio/transactional-ntfs-portal)
