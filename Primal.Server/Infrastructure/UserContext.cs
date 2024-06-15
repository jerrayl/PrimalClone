using System.Threading.Tasks;
using Primal.Entities;
using Primal.Repositories;

namespace Primal.Infrastructure
{
    public class UserContext
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public int? ScenarioId { get; set; }
    }

    public interface IUserContextFactory
    {
        public Task SetContext(string email, string givenName, string familyName, int? scenarioId);
        public UserContext UserContext { get; }
    }

    public class UserContextFactory(IDatabaseRepository<User> users) : IUserContextFactory
    {
        private readonly IDatabaseRepository<User> _users = users;
        private UserContext _userContext;

        public UserContext UserContext => _userContext;

        public async Task SetContext(string email, string givenName, string familyName, int? scenarioId)
        {
            // Todo: cache fetching of user
            var user = _users.ReadOne(x => x.Email.Equals(email));
            if (user is null)
            {
                user = new User { Email = email };
                _users.Add(user);
            }

            _userContext = new UserContext()
            {
                Id = user.Id,
                Email = email,
                GivenName = givenName,
                FamilyName = familyName,
                ScenarioId =  scenarioId
            };
        }
    }
}