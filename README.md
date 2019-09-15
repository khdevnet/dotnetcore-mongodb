# dotnetcore-mongodb
```
docker-compose up --force-recreate
docker-compose rm -fv mongo # remove docker compose cache 
```

## Transactional ntfs (deprecated)
Transactional NTFS (TxF) allows file operations on an NTFS file system volume to be performed in a transaction. TxF transactions increase application reliability by protecting data integrity across failures and simplify application development by greatly reducing the amount of error handling code.

#### Transactional ntfs alternatives
* Extensible Storage Engine (ESE) is an advanced indexed and sequential access method (ISAM) storage technology. ESE enables applications to store and retrieve data from tables using indexed or sequential cursor navigation. It supports denormalized schemas including wide tables with numerous sparse columns, multi-valued columns, and sparse and rich indexes. It enables applications to enjoy a consistent data state using transacted data update and retrieval.

* MSSQL FILESTREAM storage is implemented as a varbinary(max) column in which the data is stored as BLOBs in the file system. The sizes of the BLOBs are limited only by the volume size of the file system. The standard varbinary(max) limitation of 2-GB file sizes does not apply to BLOBs that are stored in the file system.


# Resources
* [transactional-ntfs](https://docs.microsoft.com/en-us/windows/win32/fileio/transactional-ntfs-portal)
