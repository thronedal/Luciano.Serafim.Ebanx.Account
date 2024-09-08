using System;
using Luciano.Serafim.Ebanx.Account.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Luciano.Serafim.Ebanx.Account.Infrastructure.MongoDb;

public static class Mapping
{

    /// <summary>
    /// Map MongoDb entities
    /// </summary>
    public static void MapEntities()
    {
        var pack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        };

        ConventionRegistry.Register("EnumStringConvention", pack, t => true);

        BsonClassMap.RegisterClassMap<Core.Models.Account>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<AccountConsolidatedBalance>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
            classMap.MapIdProperty(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            classMap.IdMemberMap.SetSerializer(new StringSerializer().WithRepresentation(BsonType.ObjectId));
        });
 
        BsonClassMap.RegisterClassMap<Event>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
            classMap.MapIdProperty(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            classMap.IdMemberMap.SetSerializer(new StringSerializer().WithRepresentation(BsonType.ObjectId));
        });      
    }
}
