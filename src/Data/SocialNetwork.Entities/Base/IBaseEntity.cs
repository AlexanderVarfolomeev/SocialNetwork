namespace SocialNetwork.Entities.Base;

public interface IBaseEntity
{
    public Guid Id { get; set; }


    public DateTimeOffset CreationDateTime { get; set; }

    public DateTimeOffset ModificationDateTime { get; set; }

    void Init();
    /// <summary>
    /// Change entity timestamps.
    /// </summary>
    void Touch();
}