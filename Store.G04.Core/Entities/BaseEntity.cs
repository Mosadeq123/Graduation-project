namespace Store.G04.Core.Entities;
public class BaseEntity<TKey>
{
    public int Id { get; set; }
    public DateTime CreateAt {  get; set; } = DateTime.UtcNow;
}
