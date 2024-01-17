using Domain;

namespace LocalServices;

public class DragAndDropService :IDragAndDropService
{
    public List<Guid> CurrentUsersBeingDragged { get; set; } = new();

    public EnumUserGroup? LastDragedPannel { get; set; } = null;
}
