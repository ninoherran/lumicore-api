namespace Lumicore.Domain.organization;

public class Role
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public required ICollection<Permission> Permissions { get; set; }
}