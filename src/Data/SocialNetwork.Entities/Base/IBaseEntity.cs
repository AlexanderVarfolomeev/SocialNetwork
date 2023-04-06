namespace SocialNetwork.Entities.Base;

public interface IBaseEntity
{
    public Guid Id { get; set; }


    public DateTime CreationDateTime { get; set; }

    public DateTime ModificationDateTime { get; set; }

    void Init();
    /// <summary>
    /// Change entity timestamps.
    /// </summary>
    void Touch();
}