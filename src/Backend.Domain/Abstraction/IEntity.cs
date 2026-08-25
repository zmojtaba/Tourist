namespace Backend.Domain.Abstraction
{
    public interface IEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified {  get; set; }
    }

    public interface IEntity<T> : IEntity
    {
        public T Id { get; set; }
    }
}
