namespace LocalServices;

public class DragAndDropService :IDragAndDropService
{
    public List<Guid> CurrentUsersBeingDragged { get; set; } = new();


}
