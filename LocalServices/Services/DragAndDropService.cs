using Domain;

namespace LocalServices;

public class DragAndDropService :IDragAndDropService
{

    public List<Guid> CurrentUsersBeingDragged { get; set; } = new();
    public EnumUserGroup? LastDragedPannel { get; set; } = null;



    // 0. ON NETRY HAPPENS TWICE
    // 1 . WHEN HOVERING DRAG OVER LEMENT IT FIRE ONENTER AND ONLEAVE One AFTER ANOTHER
    // 2 . On LEAVING ELEMENT ONLEAVE IS TRIGGERED AGAIN
    // 3 . ONDRAGEND, ONLEAVE IS fired again If IN Cart

    // THAT MEANS ONLEAVE is triggered 3 times when not in card
    // AND 2 times when stopped in  cart

    //private int _OnLeaveCounter = 0;

    //public ModelUser? CurrentUserDragedOver { get; set; }
    //public ModelUser? GetCurrentUserDragedOver()
    //{
    //     if( _OnLeaveCounter == -1)
    //    {
    //        // On LEAVE triggered only 2 times
    //        _OnLeaveCounter = 0;
    //        return CurrentUserDragedOver;
    //    }

    //    return null;
    //}

    //public void SetCurrentUserDraged(ModelUser? currentUserDraged, bool IsEntry)
    //{
    //    if(IsEntry)
    //    {
    //        _OnLeaveCounter++;
    //        CurrentUserDragedOver = currentUserDraged;
    //    }
    //    else 
    //    {
    //        _OnLeaveCounter--;
    //    }


    //}
}
