using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Authenticator.UserManagement;
using DAPM.Authenticator.Interfaces.Repostory_Interfaces;
using DAPM.Authenticator.Models;
using Microsoft.AspNetCore.Identity;
using RabbitMQLibrary.Messages.Authenticator.Base;
using UtilLibrary;
using DAPM.Authenticator.Interfaces;

namespace DAPM.Authenticator.Consumers
{
    public class EditAsUserMessageConsumer : IQueueConsumer<EditAsUserMessage>
    {
        private readonly IRoleManagerWrapper _rolemanager;
        private readonly IUserManagerWrapper _usermanager;
        private readonly IUserRepository _userrepository;
        private readonly IQueueProducer<EditAsUserResultMessage> _editAsUserResultProducer;

        public EditAsUserMessageConsumer(IUserManagerWrapper usermanager, IRoleManagerWrapper rolemanager, IUserRepository userrepository, IQueueProducer<EditAsUserResultMessage> editAsUserResultProducer)
        {
            _usermanager = usermanager;
            _userrepository = userrepository;
            _rolemanager = rolemanager;
            _editAsUserResultProducer = editAsUserResultProducer;
        }

        private async Task<(bool, string)> EditUser(
            EditAsUserMessage editDto,
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

                
                // Users should not be able to add new roles to themselves
                /*
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
                }*/



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

        public async Task ConsumeAsync(EditAsUserMessage message)
        {
            var editAsUserResultMessage = new EditAsUserResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = message.TicketId,
                Succeeded = false,
                Message = "Attempt to edit user not in database"
            };

            var user = await _usermanager.FindByIdAsync(message.Id.ToString());

            if (user == null)
            {
                _editAsUserResultProducer.PublishMessage(editAsUserResultMessage);
                return;
            }

            (bool, string) result = await EditUser(message, user, _usermanager, _rolemanager, _userrepository);

            if (result.Item1)
            {
                editAsUserResultMessage.Succeeded = true;
            }

            editAsUserResultMessage.Message = result.Item2;

            _editAsUserResultProducer.PublishMessage(editAsUserResultMessage);
        }
    }
}
