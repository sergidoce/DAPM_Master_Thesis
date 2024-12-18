using AutoMapper;
using DAPM.Authenticator.Consumers;
using DAPM.Authenticator.Interfaces;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using DAPM.Authenticator.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authenticator.Base;
using RabbitMQLibrary.Messages.ClientApi;
using UtilLibrary.Interfaces;

namespace Unit_tests
{
    public class RegisterMessageMessageConsumerTest
    {
        RegisterUserMessageConsumer registerconsumer;

        RegisterUserMessage source;
        User dest;

        RegisterUserResultMessage result = null;

        List<User> users = new List<User>();

        Guid ticketId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {

            users = new List<User>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();

            source = new RegisterUserMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                FullName = "Test McTestersson",
                Password = "passworD123",
                UserName = "Test",
                OrganizationId = Guid.Parse("d87bc490-828f-46c8-aa44-ded7729eaa82"),
                OrganizationName = "DTU",
                Roles = new List<string> { "Standard" }
            };

            dest = new User()
            {
                FullName = "Test McTestersson",
                UserName = "Test",
                OrganizationId = Guid.NewGuid(),
                OrganizationName = "Test",
            };


            _mockMapper.Setup(mapper => mapper.Map<User>(source)).Returns(dest);


            Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();
            _mockUserRepo.Setup(repo => repo.UserExists(It.IsAny<string>())).Returns(true);
            _mockUserRepo.Setup(repo => repo.SaveChanges(It.IsAny<User>())).Returns(1);


            Mock<IUserManagerWrapper> _mockusermanager = new Mock<IUserManagerWrapper>();
            //TODO setup for usermanager
            _mockusermanager.Setup(userman => userman.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(
               (User user, string pass) => {

                    users.Add(user);
                    return Task.FromResult(IdentityResult.Success);
                }
              );

            _mockusermanager.Setup(usermanager => usermanager.FindByNameAsync(It.IsAny<string>()))
                .Returns((string input) => Task.FromResult(users.FirstOrDefault(x => x.UserName.Equals(input, StringComparison.OrdinalIgnoreCase))));


            _mockusermanager.Setup(userman => userman.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));


            IConfiguration configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json").Build();

            Mock <IIdentityService> _mockidentityService = new Mock<IIdentityService>();
            _mockidentityService.Setup(iservice => iservice.GetIdentity()).Returns(new UtilLibrary.models.Identity {
                Id = Guid.Parse("d87bc490-828f-46c8-aa44-ded7729eaa82"),
                Name = "DTU",
                Domain = "dapm1.compute.dtu.dk"
            });

            Mock<IQueueProducer<RegisterUserResultMessage>> _mockqueueu = new Mock<IQueueProducer<RegisterUserResultMessage>>();
            _mockqueueu.Setup(queue => queue.PublishMessage(It.IsAny<RegisterUserResultMessage>()))
                    .Callback<RegisterUserResultMessage>(v => result = v);



            registerconsumer = new RegisterUserMessageConsumer(
                     null,
                     _mockMapper.Object,
                     null,
                     _mockusermanager.Object,
                     _mockUserRepo.Object,
                     null,
                     null,
                     _mockidentityService.Object,
                     _mockqueueu.Object
              );
        }

        [Test]
        public async Task RegisterUserTest()
        {
            Assert.True(users.Count == 0);
            await registerconsumer.ConsumeAsync(source);

            Assert.True(result != null);
            Assert.True(result.Message == "Okily dokily");
            Assert.True(users.Count == 1);

        }

        [Test]
        public async Task RegisterSameUserTwice()
        {
            Assert.True(users.Count == 0);
            await registerconsumer.ConsumeAsync(source);

            Assert.True(result != null);
            Assert.True(result.Message == "Okily dokily");
            Assert.True(users.Count == 1);

            await registerconsumer.ConsumeAsync(source);
            Assert.True(result.Message == "Username is already in use");
            Assert.True(users.Count == 1);

        }
    }
}