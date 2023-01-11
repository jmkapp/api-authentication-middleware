using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AuthenticationApi.Database;
using AuthenticationApi.Model;

namespace AuthenticationApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _tableName = "AuthenticationApi";
        private readonly AmazonDynamoDBClient _databaseClient;

        public UserRepository(IDatabaseClient databaseClient)
        {
            _databaseClient = databaseClient.Client;
        }

        public async Task<User> Get(string userName)
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"UserName", new AttributeValue() { S = userName }}
                }
            };

            GetItemResponse response = await _databaseClient.GetItemAsync(request);
            Dictionary<string, AttributeValue> fields = response.Item;

            if (fields.Any())
            {
                return new User()
                {
                    UserName = fields["UserName"].S,
                    PasswordHash = fields["Password"].S
                };
            }

            return new User();
        }

        public async Task<bool> Add(User newUser)
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "UserName", new AttributeValue { S = newUser.UserName } },
                    { "Password", new AttributeValue { S = newUser.PasswordHash } }
                },
                ConditionExpression = "attribute_not_exists(UserName)"
            };

            try
            {
                await _databaseClient.PutItemAsync(request);
            }
            catch (ConditionalCheckFailedException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(string userName)
        {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "UserName", new AttributeValue { S = userName} }
                },
                ConditionExpression = "attribute_exists(UserName)"
            };

            try
            {
                await _databaseClient.DeleteItemAsync(request);
            }
            catch (ConditionalCheckFailedException)
            {
                return false;
            }

            return true;
        }
    }
}
