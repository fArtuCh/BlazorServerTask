using Domain;
using static LocalServices.Helper;

namespace LocalServices;

public class UserService : IUserService
{
    private readonly ICourier _courier;
    private readonly IDragAndDropService _dragDropService;
    public List<ModelUser> AllUsers { get; set; } = new();

    public List<Guid> SelectedUsers { get; set; } = new();
    private readonly int dummyRealLifeDelay = 50; // Some Services functions could take data from DB
    private bool isShiftPressed = false;

    private Dictionary<EnumUserGroup, List<Guid>> orderOfUsers { get; set; } = new();

    public UserService(ICourier courier, IDragAndDropService dragAndDropService)
    {
        _courier = courier;
        _dragDropService = dragAndDropService;
        AllUsers = Helper.User.GenerateUsers();
    }



    private List<ModelUser> SortUsersByGuidOrder(List<ModelUser> users, EnumUserGroup group)
    {
        if (!orderOfUsers.ContainsKey(group))
        {
            var IdsInOrder = users.Select(n => n.Id).ToList();
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


    public void EnableSelection()
    {
        isShiftPressed = true;
    }


    private bool ChangeOrderOfUsers(Guid UserId)
    {
        ModelUser? ChoosenUser = AllUsers.FirstOrDefault(n => n.Id == UserId);

        if (ChoosenUser == null || ChoosenUser.IsPositionLocked)
        {
            return false;
        }

        if (!orderOfUsers.ContainsKey(ChoosenUser.UserGroup))
        {
            var result = AllUsers.Where(n => n.UserGroup == ChoosenUser.UserGroup).ToList();

            //Move user to front 
            var userToMove = result.FirstOrDefault(u => u.Id == UserId);

            // If found, move the user to the front
            if (userToMove != null)
            {
                result.Remove(userToMove);
                result.Insert(0, userToMove);
            }


            var IdsInOrder = result.Select(n => n.Id).ToList();
            orderOfUsers[ChoosenUser.UserGroup] = IdsInOrder;
            return true;
        }
        else
        {
            var order = orderOfUsers[ChoosenUser.UserGroup].ToList();
            if (order.Contains(UserId))
            {
                // Remove the GUID from its current position
                order.Remove(UserId);
                // Insert the GUID at the front of the list
                order.Insert(0, UserId);
                orderOfUsers[ChoosenUser.UserGroup] = order;
                return true;
            }

        }

        return false;
    }

    public async Task<Result<bool>> ChangeOrder(Guid UserId)
    {
        if (!isShiftPressed)
        {
            if (ChangeOrderOfUsers(UserId))
            {
                await _courier.Publish(new NotificationOrderOfUserChanged()); // It is used only for refresh not to worry
            }
            return true;
        }

        return false;
    }


    public  Task<bool> SelectUser(Guid UserId)
    {
        if (!isShiftPressed)
        {
            return Task.FromResult(false);
        }

        if (SelectedUsers.Contains(UserId))
        {
            SelectedUsers.Remove(UserId);
        }
        else
        {
            SelectedUsers.Add(UserId);
        }

        return Task.FromResult(false);
    }


    public Result<List<Guid>> GetSelectedUsers() => SelectedUsers;

    public async Task<Result<List<ModelUser>>> GetUsersBasedOnGroup(EnumUserGroup enumUserGroup)
    {
        await Task.Delay(dummyRealLifeDelay);

        var result = AllUsers.Where(n => n.UserGroup == enumUserGroup).ToList();

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

        if (enumUserGroup != ChoosenUser.UserGroup)
        {
            //MOVE SINGLE USER
            await ChangeGroupForUser(enumUserGroup, ChoosenUser);
        }
       
        // MOVE MULTIPLE USERS
        if (SelectedUsers.Count > 0)
        {
            foreach (var user in SelectedUsers)
            {
                ModelUser? ChoosenUserV2 = AllUsers.FirstOrDefault(n => n.Id == user);
                if (ChoosenUserV2 is not null && enumUserGroup != ChoosenUserV2.UserGroup)
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

        if (ChoosenUser.IsPositionLocked)
        {
            return; // LOCKED USERS ARE NOT ALLOWED TO CHANGE GROUP
        }

        var PreviousGroup = ChoosenUser.UserGroup;
        ChoosenUser.UserGroup = enumUserGroup!.Value;

        NotificationGroupChange notificationGroupChange = new()
        {
            Data = ChoosenUser,
            PreviousGroup = PreviousGroup
        };

        RemoveUserFromOrderInGroupAndAddToOtherOne(ChoosenUser.Id, PreviousGroup, ChoosenUser.UserGroup);

        await _courier.Publish(notificationGroupChange);
    }

    private void RemoveUserFromOrderInGroupAndAddToOtherOne(Guid user, EnumUserGroup StartGroup, EnumUserGroup EndGroup)
    {
        //OLD GROUP
        if(orderOfUsers.ContainsKey(StartGroup))
        {
            orderOfUsers[StartGroup].Remove(user);
        }
        else
        {
            orderOfUsers[StartGroup] = new() {  };
        }

        //NewGroup 
        if (orderOfUsers.ContainsKey(EndGroup))
        {
            orderOfUsers[EndGroup].Add(user);
        }
        else
        {
            orderOfUsers[EndGroup] = new() { user };
        }
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
        if (ChoosenUserV2 != null)
        {
            ChoosenUserV2.IsPositionLocked = IsLocked;
        }
    }
}
