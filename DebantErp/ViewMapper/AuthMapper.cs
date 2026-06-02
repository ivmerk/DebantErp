using DebantErp.ViewModels;
using DebantErp.DAL.Models;

namespace DebantErp.ViewMapper;
public class AuthMapper
{
    public static UserModel MapRegistrationViewModelToUserModel(RegisterViewModel model)
    {
        return new UserModel()
        {
            Email = model.Email!,
            Password = model.Password!
        };
    }
}
