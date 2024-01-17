using Domain;

namespace LocalServices;

public class UserService : IUserService
{

    public List<ModelUser> AllUsers { get; set; } = new();

    private readonly int dummyRealLifeDelay = 50;

    public ICourier _courier { get;  }


    public UserService(ICourier courier)
    {
        _courier = courier;
        AllUsers = Helper.User.GenerateUsers();

    }

    public async Task<Result<List<ModelUser>>> GetUsersBasedOnGroup(EnumUserGroup enumUserGroup)
    {
        await Task.Delay(dummyRealLifeDelay);

        var result =  AllUsers.Where(n => n.UserGroup == enumUserGroup).ToList();

        return result;
    }

    public async Task<Result<bool>> MoveUserToGivenGroup(Guid UserId, EnumUserGroup? enumUserGroup)
    {
        if(enumUserGroup is null)
        {
            return Result<bool>.Error;
        }

        await Task.Delay(dummyRealLifeDelay);

        ModelUser? ChoosenUser =  AllUsers.FirstOrDefault(n => n.Id == UserId);

        if(ChoosenUser == null)
        { 
            return Result<bool>.Error; 
        }

        if(enumUserGroup == ChoosenUser.UserGroup)
        {
            return Result<bool>.Error;
        }


        var PreviousGroup = ChoosenUser.UserGroup;

        ChoosenUser.UserGroup = enumUserGroup.Value;

        NotificationGroupChange notificationGroupChange = new ()
        { 
            Data = ChoosenUser,
            PreviousGroup = PreviousGroup
        };

        await _courier.Publish(notificationGroupChange);

        return true;
    }

    public async Task<Result<ModelUser>> GetSingleUserById(Guid UserId)
    {
        await Task.Delay(dummyRealLifeDelay);

        ModelUser? ChoosenUser = AllUsers.FirstOrDefault(n => n.Id == UserId);

        if (ChoosenUser == null) { return Result<ModelUser>.Error; }

        return ChoosenUser;

    }
}
