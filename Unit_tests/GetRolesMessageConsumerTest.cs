using DAPM.Authenticator.Consumers;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using Moq;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.Authenticator.RoleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Unit_tests
{
    public class GetRolesMessageConsumerTest
    {
        GetRolesMessageConsumer consumer;

        GetRolesMessage message = new GetRolesMessage() {
             MessageId = Guid.NewGuid(),
             TicketId = Guid.NewGuid(),
             TimeToLive = TimeSpan.FromMinutes(1),
        };

        List<string> roles = new List<string>() { "Admin", "Standard", "Priviliged" };


        GetRolesResultMessage result;


        [SetUp]
        public void Setup() {

            Mock<IQueueProducer<GetRolesResultMessage>> _mockqueueu = new Mock<IQueueProducer<GetRolesResultMessage>>();
            _mockqueueu.Setup(queue => queue.PublishMessage(It.IsAny<GetRolesResultMessage>()))
                .Callback<GetRolesResultMessage>( v => result = v);


            Mock<IRoleRepository> _mockrolerepo = new Mock<IRoleRepository>();
            _mockrolerepo.Setup(repo => repo.Roles()).Returns(roles.Select(role => new Role() { Name = role}).ToList());

            consumer = new GetRolesMessageConsumer(
                _mockrolerepo.Object,
                _mockqueueu.Object);
        }


        [Test]
        public async Task RetreiveRolesSuccessfully()
        {
            await consumer.ConsumeAsync(message);

            Assert.True(result != null);
            var template = new[] { new { RoleName = string.Empty } };
            var rolesreturned = JsonConvert.DeserializeAnonymousType(result.Message, template);

            Assert.True(rolesreturned.Length == roles.Count);


            //TODO deserialize and inspect message contents

        }

    }
}
