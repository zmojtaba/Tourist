using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.ValueObjects
{
    public class VehicleId
    {
        public Guid Value { get; }

        private VehicleId(Guid value) => Value = value;
        public static VehicleId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty) throw new DomainException("Value can not be empty");
            return new VehicleId(value);

        }
    }
}
