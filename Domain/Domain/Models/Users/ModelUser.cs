namespace Domain;

public sealed class ModelUser
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Surname{ get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string ProfilePictureLocation { get; set; } = string.Empty;

    public bool IsPositionLocked { get; set; }
    public EnumUserGroup UserGroup { get; set; } =  EnumUserGroup.None;
    
}
