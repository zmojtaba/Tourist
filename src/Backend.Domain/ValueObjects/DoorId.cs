namespace Backend.Domain.ValueObjects
{
    public class DoorId
    {
        public Guid Value { get; }
        private DoorId(Guid value) => Value = value;
        public static DoorId Of(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(
                    "Door id cannot be empty.",
                    nameof(value));
            return new DoorId(value);
        }
    }
}
