//using NetTopologySuite.Geometries;

namespace Backend.Domain.ValueObjects
{
    //public class GeoLocation
    //{
    //    public Point Value { get; private set; }

    //    private GeoLocation() { } // EF

    //    public GeoLocation(Point point)
    //    {
    //        Value = point;
    //    }

    //    public static GeoLocation Of(double latitude, double longitude)
    //    {
    //        var point = new Point(longitude, latitude) // IMPORTANT: (lng, lat)
    //        {
    //            SRID = 4326
    //        };

    //        return new GeoLocation(point);
    //    }

    //    public double Latitude => Value.Y;
    //    public double Longitude => Value.X;

    //}
    public class GeoLocation
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private GeoLocation() { } // EF

        public GeoLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
