using DAPM.Authenticator.Consumers;
using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using RabbitMQLibrary.Messages.ClientApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_tests
{
    public class SetRolesMessageConsumerTest
    {
        SetRolesMessageConsumer consumer;
        SetRolesMessage message = new SetRolesMessage()
        {

            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromSeconds(1),
            UserName = "johnny",
            Roles = new List<string>() { "Admin"}
        };

        SetRolesMessage message2 = new SetRolesMessage()
        {

            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromSeconds(1),
            UserName = "johnny",
            Roles = new List<string>() { "Bandit" }
        };

        List<(User, List<Role>)> usersandRoles;

        List<string> roles = new List<string>() { "Admin", "Standard", "Priviliged" };
        SetRolesResultMessage result = null;


        [SetUp]
        public void Setup() {
            usersandRoles = new List<(User, List<Role>)> {
             (
                new User {
                FullName = "Jimbob",
                PasswordHash = "",
                Id = 5,
                UserName = "johnny" },
                new List<Role>(){new Role{Name= "Standard"} })};


            Mock<IQueueProducer<SetRolesResultMessage>> _mockqueueu = new Mock<IQueueProducer<SetRolesResultMessage>>();
            _mockqueueu.Setup(q => q.PublishMessage(It.IsAny<SetRolesResultMessage>())).
                Callback<SetRolesResultMessage>(v => result = v);

            Mock<IUserManagerWrapper> _mockusermanager = new Mock<IUserManagerWrapper>();
            _mockusermanager.Setup(usermanager => usermanager.FindByNameAsync(It.IsAny<string>()))
                .Returns((string input) => Task.FromResult(usersandRoles.FirstOrDefault(x => x.Item1.UserName.Equals(input, StringComparison.OrdinalIgnoreCase)).Item1));
            _mockusermanager.Setup(usermanager => usermanager.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(
                  (User userinput, string roleinput) => {
                      (User, List<Role>) user = usersandRoles.FirstOrDefault(x => x.Item1.Id == userinput.Id);
                      if (!(user.Item2.Any(x => x.Name == roleinput)))
                          user.Item2.Add(new Role { Name = roleinput });
                      return Task.FromResult(IdentityResult.Success);
                  }
             );



            Mock<IRoleManagerWrapper> _mockrolemanager = new Mock<IRoleManagerWrapper>();
            _mockrolemanager.Setup(repo => repo.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync((string role)
                => 
            roles.Contains(role));

            _mockrolemanager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync((string name) => {
                                string role = roles.FirstOrDefault(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
                                if (role != null) {
                                    return new Role { Name = role };
                                }
                                else {
                                    return null;
                                }
                                
                                });

            Mock<IUserRepository> _mockuserrepo = new Mock<IUserRepository>();
            _mockuserrepo.Setup(repo => repo.SaveChanges(It.IsAny<User>()));

            consumer = new SetRolesMessageConsumer(
                _mockusermanager.Object,
                _mockrolemanager.Object,
                _mockuserrepo.Object,
                _mockqueueu.Object
                );
        }

        [Test]
        public async Task SetRolesTest() { 
        
            await consumer.ConsumeAsync(message);
            Assert.True(result != null);
            Assert.True(result.Message == "succesfully added specified roles");
        }

        [Test]
        public async Task SetNonLegalRolesTest()
        {

            await consumer.ConsumeAsync(message2);
            Assert.True(result != null);
            Assert.True(result.Message == "Attempting to add non existent role, all roles in list must exist");
        }

    }
}
