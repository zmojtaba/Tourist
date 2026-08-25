using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrustructure.Data.Converter
{
    public class StronglyTypedIdConverter<T> : ValueConverter<T, Guid>
        where T : class
    {
        public StronglyTypedIdConverter(Func<Guid, T> factory)
            : base(
                id => GetValue(id),
                value => factory(value))
        {
        }

        private static Guid GetValue(T id)
        {
            if (id == null)
                return Guid.Empty;

            var prop = id.GetType().GetProperty("Value");
            if (prop == null)
                throw new InvalidOperationException($"No Value property on {typeof(T)}");

            return (Guid)prop.GetValue(id)!;
        }
    }
}
