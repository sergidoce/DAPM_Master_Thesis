using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using Microsoft.AspNetCore.Identity;
using UtilLibrary;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using RabbitMQLibrary.Messages.Authenticator.Base;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class EditAsAdminMessageConsumer : IQueueConsumer<EditAsAdminMessage>
    {
        private readonly IRoleManagerWrapper _rolemanager;
        private readonly IUserManagerWrapper _usermanager;
        private readonly IUserRepository _userrepository;
        private readonly IQueueProducer<EditAsAdminResultMessage> _editAsAdminResultProducer;

        public EditAsAdminMessageConsumer(IUserManagerWrapper usermanager, IRoleManagerWrapper rolemanager, IUserRepository userrepository, IQueueProducer<EditAsAdminResultMessage> editAsAdminResultProducer)
        {
            _usermanager = usermanager;
            _userrepository = userrepository;
            _rolemanager = rolemanager;
            _editAsAdminResultProducer = editAsAdminResultProducer;
        }

        private async Task<(bool, string)> EditUser(
            EditAsAdminMessage editDto,
            User user,
            IUserManagerWrapper userManager,
            IRoleManagerWrapper rolemanager,
            IUserRepository repository)
        {

            try
            {
                user.FullName = editDto.FullName;
                user.UserName = editDto.UserName;

                repository.SaveChanges(user);


                //remove all roles
                List<string> currentRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
                foreach (string removerole in currentRoles)
                {
                    await userManager.RemoveFromRoleAsync(user, removerole);
                }

                //add all new roles, roles that dont exist will be ignored
                foreach (var role in editDto.Roles)
                {
                    if (await rolemanager.RoleExistsAsync(role))
                    {
                        IdentityResult resultrole = await userManager.AddToRoleAsync(user, role);
                        if (resultrole != IdentityResult.Success)
                        {
                            return (false, $"Error occurred when adding role: {role}");
                        }
                    }
                }
                if (editDto.NewPassword != "")
                {
                    IdentityResult resultremove = await userManager.RemovePasswordAsync(user);
                    IdentityResult resultadd = await userManager.AddPasswordAsync(user, editDto.NewPassword);
                    if (resultadd != IdentityResult.Success || resultadd != IdentityResult.Success)
                    {
                        return (false, "Error occurred changing password");
                    }
                }

                return (true, "Edit operation succeeded");
            }
            catch
            {
                return (false, "Edit operation encountered an exception");
            }


        }

        public async Task ConsumeAsync(EditAsAdminMessage message)
        {
            var editAsAdminResultMessage = new EditAsAdminResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = false,
                Message = "Attempt to edit user not in database"
            };
            
            var user = await _usermanager.FindByIdAsync(message.Id.ToString());

            if (user == null)
            {
                _editAsAdminResultProducer.PublishMessage(editAsAdminResultMessage);
                return;
            }

            (bool, string) result = await EditUser(message, user, _usermanager, _rolemanager, _userrepository);

            if (result.Item1)
            {
                editAsAdminResultMessage.Succeeded = true;
            }

            editAsAdminResultMessage.Message = result.Item2;
            _editAsAdminResultProducer.PublishMessage(editAsAdminResultMessage);
        }
    }
}
