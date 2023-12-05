using FirebaseAdmin;
using FirebaseAdmin.Auth;
using GraphQLDemo.API.Schema.Queries;
using System.Collections.Immutable;

namespace GraphQLDemo.API.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<string, UserType>
    {
        public readonly FirebaseAuth _firebaseAuth;

        public UserDataLoader(
            FirebaseApp firebaseApp,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null) : base(batchScheduler, options)
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var users = await _firebaseAuth.GetUsersAsync(
                keys.Select(u => new UidIdentifier(u)).ToList()
            );
            return users.Users.Select(u => new UserType
            {
                Id = u.Uid,
                PhotoUrl = u.PhotoUrl,
                Username = u.DisplayName
            }).ToDictionary(u => u.Id);
        }
    }
}
