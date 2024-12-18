using AutoMapper;
using DAPM.Authenticator.Consumers;
using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using Moq;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilLibrary;

namespace Unit_tests
{
    public class GetUsersMessageConsumerTest
    {

        GetUsersMessageConsumer consumer;
        List<(User, List<Role>)> usersAndRoles;
        GetUsersMessage message = new GetUsersMessage()
        {
            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromSeconds(1)
        };

        GetUsersResultMessage result = null;

        [SetUp]
        public void Setup() {

            usersAndRoles = new List<(User, List<Role>)> {
             (
                new User {
                FullName = "Jimbob",
                PasswordHash = "",
                Id = 5,
                UserName = "johnny" },
                new List<Role>(){new Role{Name= "Standard"} }
                )};

            Mock<IQueueProducer<GetUsersResultMessage>> _mockqueueu = new Mock<IQueueProducer<GetUsersResultMessage>>();
            _mockqueueu.Setup(q => q.PublishMessage(It.IsAny<GetUsersResultMessage>()))
                 .Callback<GetUsersResultMessage>(v => result = v);

            Mock<IUserManagerWrapper> _mockusermanager = new Mock<IUserManagerWrapper>();
            _mockusermanager.Setup(usermanager => usermanager.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(
                       (User input) => {
                           (User, List<Role>) user = usersAndRoles.FirstOrDefault(x => x.Item1.Id == input.Id);
                           return user.Item1 != null ? user.Item2.Select(x => x.Name).ToList() : new List<string>();
                       }
                   );

            Mock<IUserRepository> _mockuserrepo = new Mock<IUserRepository>();
            _mockuserrepo.Setup(repo => repo.Users()).Returns(usersAndRoles.Select( x => x.Item1).ToList());


            Mock<IMapper> _mockmapper = new Mock<IMapper>();
            _mockmapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns((User user) => {
                return new UserDto() { 
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    OrganizationId = user.OrganizationId,
                    OrganizationName = user.OrganizationName,
                }; 
            });


            consumer = new GetUsersMessageConsumer(
                _mockusermanager.Object,
                _mockuserrepo.Object,
                _mockmapper.Object,
                _mockqueueu.Object
            );

        }

        [Test]
        public void TestUserRetrieval() {
            consumer.ConsumeAsync(message);
            Assert.True( result != null );
            List<UserDto> users = JsonConvert.DeserializeObject<List<UserDto>>(result.Message);
            Assert.True(users[0].UserName == usersAndRoles[0].Item1.UserName);
            Assert.True(users.Count == usersAndRoles.Count);
        }

    }
}
