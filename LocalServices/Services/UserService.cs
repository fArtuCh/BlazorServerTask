using Domain;
using static LocalServices.Helper;

namespace LocalServices;

public class UserService : IUserService
{
    private readonly ICourier _courier;

    public List<ModelUser> AllUsers { get; set; } = new();

    public List<Guid> SelectedUsers { get; set; } = new();
    private readonly int dummyRealLifeDelay = 50; // Some Services functions could take data from DB
    private  bool isShiftPressed = false;

    private Dictionary<EnumUserGroup,List<Guid>> orderOfUsers { get; set; } = new();

    public UserService(ICourier courier)
    {
        _courier = courier;
        AllUsers = Helper.User.GenerateUsers();
    }



    private List<ModelUser> SortUsersByGuidOrder(List<ModelUser> users, EnumUserGroup group )
    {
        if (!orderOfUsers.ContainsKey(group))
        {
            var IdsInOrder = users.Select(n=>n.Id).ToList();
            orderOfUsers[group] = IdsInOrder;
            return users;
        }

        List<Guid> orderedGuids = orderOfUsers[group];
        // Creating a dictionary for faster lookups
        var guidIndexDict = orderedGuids.Select((id, index) => new { id, index })
                                        .ToDictionary(g => g.id, g => g.index);

        return users.OrderBy(u =>
        {
            if (guidIndexDict.TryGetValue(u.Id, out int index))
            {
                return index;
            }
            return int.MaxValue; // Users not in the GUID list are placed at the end
        }).ToList();
    }





    public async Task ClearUserSelection()
    {
        SelectedUsers.Clear();
        isShiftPressed = false;
        await _courier.Publish(new NotificationShiftKeyRealeased());
    }


    public void  EnableSelection()
    {
        isShiftPressed = true;
    }


    public Result<bool> SelectUser(Guid UserId)
    {
        if(!isShiftPressed)
        {
            return false;
        }

        if (SelectedUsers.Contains(UserId))
        {
            SelectedUsers.Remove(UserId);
        }
        else
        {
            SelectedUsers.Add(UserId);
        }

        return true;
    }



    public  Result<List<Guid>> GetSelectedUsers()
    {    
        return SelectedUsers;
    }



    public async Task<Result<List<ModelUser>>> GetUsersBasedOnGroup(EnumUserGroup enumUserGroup)
    {
        await Task.Delay(dummyRealLifeDelay);

        var result =  AllUsers.Where(n => n.UserGroup == enumUserGroup).ToList();

        return SortUsersByGuidOrder(result, enumUserGroup);
    }



    public async Task<Result<bool>> MoveUserToGivenGroup(Guid UserId, EnumUserGroup? enumUserGroup)
    {
        if (enumUserGroup is null)
        {
            return Result<bool>.Error;
        }

        await Task.Delay(dummyRealLifeDelay);

        ModelUser? ChoosenUser = AllUsers.FirstOrDefault(n => n.Id == UserId);

        if (ChoosenUser == null)
        {
            return Result<bool>.Error;
        }

        if (enumUserGroup == ChoosenUser.UserGroup)
        {
            return Result<bool>.Error;
        }


        //SelectedUsers

        await ChangeGroupForUser(enumUserGroup, ChoosenUser);

        if(SelectedUsers.Count > 0)
        {
            foreach (var user in SelectedUsers)
            {
                ModelUser? ChoosenUserV2 = AllUsers.FirstOrDefault(n => n.Id == user);
                if (ChoosenUserV2 != null && enumUserGroup != ChoosenUserV2.UserGroup)
                {
                    await ChangeGroupForUser(enumUserGroup, ChoosenUserV2);
                }
            }
            await ClearUserSelection();
        }

        return true;
    }



    private async Task ChangeGroupForUser(EnumUserGroup? enumUserGroup, ModelUser ChoosenUser)
    {

        if(ChoosenUser.IsPositionLocked)
        {
            return;
        }

        var PreviousGroup = ChoosenUser.UserGroup;
        ChoosenUser.UserGroup = enumUserGroup!.Value;

        NotificationGroupChange notificationGroupChange = new()
        {
            Data = ChoosenUser,
            PreviousGroup = PreviousGroup
        };

        await _courier.Publish(notificationGroupChange);
    }

    public async Task<Result<ModelUser>> GetSingleUserById(Guid UserId)
    {
        await Task.Delay(dummyRealLifeDelay);

        ModelUser? ChoosenUser = AllUsers.FirstOrDefault(n => n.Id == UserId);

        if (ChoosenUser == null) { return Result<ModelUser>.Error; }

        return ChoosenUser;

    }

    public void ChangeLockOnUser(Guid UserId, bool IsLocked)
    {
        ModelUser? ChoosenUserV2 = AllUsers.FirstOrDefault(n => n.Id == UserId);
        if (ChoosenUserV2 != null )
        {
            ChoosenUserV2.IsPositionLocked = IsLocked;
        }
    }
}
