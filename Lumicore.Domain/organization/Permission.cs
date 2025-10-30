namespace Lumicore.Domain.organization;

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Folder { get; set; }
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanDelete { get; set; }
    public bool CanManageAccess { get; set; }
}