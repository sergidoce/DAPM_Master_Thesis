using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAPM.Authenticator.Consumers;
using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using Moq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using RabbitMQLibrary.Messages.ClientApi;

namespace Unit_tests
{
    public class SetOrganizationConsumerTest
    {

        SetOrganizationMessageConsumer consumer;


        List<User> users= new List<User> {
             
                new User {
                FullName = "Jimbob",
                PasswordHash = "",
                Id = 5,
                UserName = "johnny" }};

        SetOrganizationMessage message = new SetOrganizationMessage() {

            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromSeconds(1),
            UserName = "johnny",
            OrganizationId = Guid.NewGuid(),
            OrganizationName = "SDU"
        };


        SetOrganizationMessage message2 = new SetOrganizationMessage()
        {

            MessageId = Guid.NewGuid(),
            TicketId = Guid.NewGuid(),
            TimeToLive = TimeSpan.FromSeconds(1),
            UserName = "johnny2",
            OrganizationId = Guid.NewGuid(),
            OrganizationName = "SDU"
        };
        SetOrganizationResultMessage result = null;

        [SetUp]
        public void Setup() {

            Mock<IQueueProducer<SetOrganizationResultMessage>> _mockqueueu = new Mock<IQueueProducer<SetOrganizationResultMessage>>();
            _mockqueueu.Setup(q => q.PublishMessage(It.IsAny<SetOrganizationResultMessage>())).
                Callback<SetOrganizationResultMessage>(v => result = v);



            Mock<IUserManagerWrapper> _mockusermanager = new Mock<IUserManagerWrapper>();
            _mockusermanager.Setup(usermanager => usermanager.FindByNameAsync(It.IsAny<string>()))
                .Returns((string input) =>
                {
                    User user = users.FirstOrDefault(x => x.UserName.Equals(input, StringComparison.OrdinalIgnoreCase));
                    return Task.FromResult(user);
                });

            Mock<IUserRepository> _mockuserrepo = new Mock<IUserRepository>();
            _mockuserrepo.Setup(repo => repo.SaveChanges(It.IsAny<User>()));


            consumer = new SetOrganizationMessageConsumer(
                    _mockusermanager.Object,
                    _mockuserrepo.Object,
                    _mockqueueu.Object );
        }

        [Test]
        public async Task SetOrgTest() {

            await consumer.ConsumeAsync(message);
            Assert.True(result != null);
            Assert.True(result.Message == $"successfully assigned {message.UserName} a new organisation");
        
        }

        [Test]
        public async Task SetOrgOfUserNotInSystemTest()
        {

            await consumer.ConsumeAsync(message2);
            Assert.True(result != null);
            Assert.True(result.Message == $"This user does not exist in our system, therefore we cannot set their org");

        }
    }
}
