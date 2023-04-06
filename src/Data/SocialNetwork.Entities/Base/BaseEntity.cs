namespace SocialNetwork.Entities.Base;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModificationDateTime { get; set; }
    
    public void Init()
    {
        CreationDateTime = DateTime.Now;
        ModificationDateTime = CreationDateTime;
    }

    /// <summary>
    /// Change entity timestamps.
    /// </summary>
    public void Touch()
    {
        ModificationDateTime = DateTime.Now;
    }
}