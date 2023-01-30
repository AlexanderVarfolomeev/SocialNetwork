namespace SocialNetwork.Entities.Base;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreationDateTime { get; set; }
    public DateTimeOffset ModificationDateTime { get; set; }
    
    public void Init()
    {
        CreationDateTime = DateTimeOffset.Now;
        ModificationDateTime = CreationDateTime;
    }

    /// <summary>
    /// Change entity timestamps.
    /// </summary>
    public void Touch()
    {
        ModificationDateTime = DateTimeOffset.Now;
    }
}