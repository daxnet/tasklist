using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskList.Common;

namespace TaskList.CrudProviders.MongoDB
{
    public class MongoCrudProvider : ICrudProvider
    {
        private readonly IMongoClient client;
        private readonly IMongoDatabase database;

        public MongoCrudProvider(string databaseName, string server = "localhost", int port = 27017)
        {
            this.client = new MongoClient($"mongodb://{server}:{port}/{databaseName}");
            this.database = this.client.GetDatabase(databaseName);
        }

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : IEntity
        {
            var collection = GetCollection<TEntity>();
            var options = new InsertOneOptions { BypassDocumentValidation = true };
            await collection.InsertOneAsync(entity, options);
        }

        public async Task DeleteByIdAsync<TEntity>(Guid id) where TEntity : IEntity
        {
            var filterDefinition = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            await GetCollection<TEntity>().DeleteOneAsync(filterDefinition);
        }

        public void Dispose()
        {

        }

        public async Task<IEnumerable<TEntity>> FindBySpecificationAsync<TEntity>(Expression<Func<TEntity, bool>> expr) where TEntity : IEntity =>
            await (await GetCollection<TEntity>().FindAsync(expr)).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : IEntity =>
            await FindBySpecificationAsync<TEntity>(_ => true);

        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : IEntity =>
            (await FindBySpecificationAsync<TEntity>(x => x.Id.Equals(id))).FirstOrDefault();

        public async Task UpdateByIdAsync<TEntity>(Guid id, TEntity entity) where TEntity : IEntity
        {
            var filterDefinition = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            await GetCollection<TEntity>().ReplaceOneAsync(filterDefinition, entity);
        }

        private IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : IEntity =>
            this.database.GetCollection<TEntity>(typeof(TEntity).Name);
    }
}
