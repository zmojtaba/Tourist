using Backend.Domain.Models;

namespace Backend.Domain.ValueObjects
{
    public record Address
    {
        public string City { get; }
        public string Region { get; }
        public string PostalCode { get; }
        public string Country { get; }
        private Address() { }
        private Address (string city, string region, string postalCode, string country)
        {
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
        }
        public static Address Of(string city, string region, string postalCode, string country)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(city, nameof(city));
            ArgumentException.ThrowIfNullOrWhiteSpace(region, nameof(region));
            ArgumentException.ThrowIfNullOrWhiteSpace(postalCode, nameof(postalCode));
            ArgumentException.ThrowIfNullOrWhiteSpace(country, nameof(country));
            return new Address (city, region, postalCode, country);

        }

    }
}
