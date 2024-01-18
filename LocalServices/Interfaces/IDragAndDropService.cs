using Domain;

namespace LocalServices;

public interface IDragAndDropService
{

    List<Guid> CurrentUsersBeingDragged { get; set; }

    EnumUserGroup? LastDragedPannel { get; set; }


    // FAILED ATTEMPT:
    //ModelUser? GetCurrentUserDragedOver();

    //void SetCurrentUserDraged(ModelUser? currentUserDraged, bool IsEntry);


}
