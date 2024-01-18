using Domain;

namespace LocalServices;

public interface IUserService
{
    public Task<Result<List<ModelUser>>> GetUsersBasedOnGroup(EnumUserGroup enumUserGroup);
    public Task<Result<ModelUser>> GetSingleUserById(Guid UserId);


    public Task<Result<bool>> MoveUserToGivenGroup(Guid UserId,EnumUserGroup? enumUserGroup);


    public void ChangeLockOnUser(Guid UserId, bool IsLocked);


    public Result<List<Guid>> GetSelectedUsers();
    public Task<bool> SelectUser(Guid UserId);

    public Task<Result<bool>> ChangeOrder(Guid UserId);

    public Task ClearUserSelection();
    public void EnableSelection();
}
