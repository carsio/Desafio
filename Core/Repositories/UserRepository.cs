using Core.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using User = Core.Models.User;

namespace Core.Repositories;

public class UserRepository: IUserRepository {
    private readonly CosmosClient _cosmosClient;

    public UserRepository(CosmosClient cosmosClient) {
        _cosmosClient = cosmosClient;
    }
    
    public async Task<User?> GetUserByEmail(string email) {
        var container = _cosmosClient.GetContainer("core", "users");
        var query = new QueryDefinition("SELECT * FROM c WHERE c.email = @email")
            .WithParameter("@email", email);
        var iterator = container.GetItemQueryIterator<User>(query);
        var user = await iterator.ReadNextAsync();
        return user.FirstOrDefault();
    }

    public async Task<User> Create(User user) {
        var container = _cosmosClient.GetContainer("core", "users");
        var response = await container.CreateItemAsync(user);
        return response.Resource;
    }

    public async Task<User> Update(User user) {
        var container = _cosmosClient.GetContainer("core", "users");
        var response = await container.ReplaceItemAsync(user, user.Id);
        return response.Resource;
    }
}