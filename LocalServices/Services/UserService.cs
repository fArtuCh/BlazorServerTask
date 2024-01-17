using Domain;

namespace LocalServices;

public class UserService : IUserService
{

    public List<ModelUser> AllUsers { get; set; } = new();

    private readonly int dummyRealLifeDelay = 100;


    public UserService()
    {

        AllUsers = Helper.User.GenerateUsers();

    }

    public async Task<Result<List<ModelUser>>> GetUsersBasedOnGroup(EnumUserGroup enumUserGroup)
    {
        await Task.Delay(dummyRealLifeDelay);

        var result =  AllUsers.Where(n => n.UserGroup == enumUserGroup).ToList();

        return result;
    }

    public async Task<Result<bool>> MoveUserToGivenGroup(Guid UserId, EnumUserGroup enumUserGroup)
    {
        await Task.Delay(dummyRealLifeDelay);

        ModelUser? ChoosenUser =  AllUsers.FirstOrDefault(n => n.Id == UserId);

        if(ChoosenUser == null) { return Result<bool>.Error; }

        ChoosenUser.UserGroup = enumUserGroup;

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
