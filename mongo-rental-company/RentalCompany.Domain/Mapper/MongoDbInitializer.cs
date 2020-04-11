using MongoDB.Bson.Serialization.Conventions;

namespace RentalCompany.Domain.Mapper
{
    public static class MongoDbInitializer
    {
        public static void Init()
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention()
            };

            ConventionRegistry.Register(
                "CamelCaseElementNameConvention",
                pack,
                t => true);

            EntityMapperInitializer.Init();
        }
    }
}
