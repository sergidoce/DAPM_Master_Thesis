using DAPM.Authenticator.Consumers;
using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.Authenticator.RoleManagement;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_tests
{

    

    public class DeleteUserMessagerConsumerTest
    {
        DeleteUserMessageConsumer consumer;
        List<User> users;


        DeleteUserMessage message = new DeleteUserMessage()
        {
            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromMinutes(1),
            UserName = "johnny"
        };

        DeleteUserMessage message2 = new DeleteUserMessage()
        {
            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromMinutes(1),
            UserName = "johnny2"
        };

        DeleteUserResultMessage result;

        [SetUp]
        public void Setup() {

            result = null;

            users = new List<User>() { new User {
            FullName = "Jimbob",
            PasswordHash = "",
            Id = 5,
            UserName = "johnny" } };

            Mock<IQueueProducer<DeleteUserResultMessage>> _mockqueueu = new Mock<IQueueProducer<DeleteUserResultMessage>>();
            _mockqueueu.Setup(queue => queue.PublishMessage(It.IsAny<DeleteUserResultMessage>()))
                .Callback<DeleteUserResultMessage>(r => result = r);
                


            Mock<IUserManagerWrapper> _mockusermanager = new Mock<IUserManagerWrapper>();
            //TODO setup for usermanager
            _mockusermanager.Setup(usermanager => usermanager.DeleteAsync(It.IsAny<User>()))
                            .ReturnsAsync((User input) => {
                                users = users.Where(x => x.Id != input.Id).ToList();
                                return IdentityResult.Success;
                            });

            _mockusermanager.Setup(usermanager => usermanager.FindByNameAsync(It.IsAny<string>()))
                            .Returns( (string input) => Task.FromResult(users.FirstOrDefault(x => x.UserName.Equals(input, StringComparison.OrdinalIgnoreCase))));

            consumer = new DeleteUserMessageConsumer(
                    _mockusermanager.Object,
                    _mockqueueu.Object
                );
        
        }

        [Test]
        public async Task DeleteUserTest() {

            int amountOfUsers = users.Count;
            await consumer.ConsumeAsync(message);
            Assert.True(users.Count == amountOfUsers - 1);
            Assert.True(result != null);
            Assert.True(result.Message == "User succesfully deleted");
        }

        [Test]
        public async Task DeleteUserThatDoesntExist()
        {
            int amountOfUsers = users.Count;
            await consumer.ConsumeAsync(message2);
            Assert.True(users.Count == amountOfUsers);
            Assert.True(result != null);
            Assert.True(result.Message == "This user does not exist in our system, therefore cannot be deleted");
        }


    }
}
