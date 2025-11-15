namespace Lumicore.Domain.role;

public class Role(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;

    public Role(string name) : this(Guid.NewGuid(), name) { }
}