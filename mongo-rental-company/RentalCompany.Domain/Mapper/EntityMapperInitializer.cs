using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using RentalCompany.Domain.Entity;

namespace RentalCompany.Domain.Mapper
{
    public static class EntityMapperInitializer
    {
        public static void Init()
        {

            BsonClassMap.RegisterClassMap<Rental>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Price)
                .SetSerializer(new DecimalSerializer(BsonType.Double));

                cm.MapIdMember(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            });
        }
    }
}
