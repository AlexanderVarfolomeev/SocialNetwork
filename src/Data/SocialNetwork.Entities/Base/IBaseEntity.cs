using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Entities.Base;

public interface IBaseEntity
{
    public Guid Id { get; set; }
    
    
    public DateTimeOffset CreationDateTime { get; set; }
    
    public DateTimeOffset ModificationDateTime { get; set; }
}