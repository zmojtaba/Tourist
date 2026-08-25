//using Microsoft.EntityFrameworkCore;
//using NetTopologySuite.Geometries;
//using NetTopologySuite.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Backend.Domain.Roles;

//namespace Backend.Application
//{

//    //double radiusKm = 5;
//    //double radiusDegrees = radiusKm / 111.0;

//    //double lat = 52.3702;
//    //double lng = 4.8952;

//    //var drivers = await context.DriverRoles
//    //    .FromSqlInterpolated($@"
//    //    SELECT *
//    //    FROM ""DriverRoles""
//    //    WHERE ""CurrentLocation"" IS NOT NULL
//    //    AND ""CurrentLocation"" <-> point({lng}, {lat}) <= {radiusDegrees}
//    //")
//    //    .ToListAsync();

//    //public async Task<List<DriverRole>> GetNearbyDrivers(
//    //double lat, double lng, double radiusKm)
//    //{
//    //    return await _context.DriverRoles
//    //        .FromSqlInterpolated($@"
//    //        SELECT *
//    //        FROM ""DriverRoles""
//    //        WHERE ""Latitude"" IS NOT NULL
//    //        AND (
//    //            6371 * acos(
//    //                cos(radians({lat})) *
//    //                cos(radians(""Latitude"")) *
//    //                cos(radians(""Longitude"") - radians({lng})) +
//    //                sin(radians({lat})) *
//    //                sin(radians(""Latitude""))
//    //            )
//    //        ) <= {radiusKm}
//    //    ")
//    //        .ToListAsync();
//    //}



//    //var drivers = await context.DriverRoles
//    //.Where(d => d.CurrentLocation != null)
//    //.AsEnumerable() // client-side
//    //.Where(d =>
//    //{
//    //    var dx = d.CurrentLocation.Value.X - lng;
//    //    var dy = d.CurrentLocation.Value.Y - lat;
//    //    var distance = Math.Sqrt(dx * dx + dy * dy) * 111;
//    //    return distance <= radiusKm;
//    //})
//    //.ToList();

//    //public async Task<List<DriverRole>> GetNearbyDrivers(
//    //double lat, double lng, double radiusKm)
//    //{
//    //    var drivers = await _context.DriverRoles
//    //        .Where(d => d.CurrentLocation != null)
//    //        .ToListAsync();

//    //    return drivers.Where(d =>
//    //    {
//    //        var dLat = DegreesToRadians(d.CurrentLocation.Latitude - lat);
//    //        var dLon = DegreesToRadians(d.CurrentLocation.Longitude - lng);

//    //        var a =
//    //            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
//    //            Math.Cos(DegreesToRadians(lat)) *
//    //            Math.Cos(DegreesToRadians(d.CurrentLocation.Latitude)) *
//    //            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

//    //        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

//    //        var distance = 6371 * c;

//    //        return distance <= radiusKm;
//    //    }).ToList();
//    //}

//    //private double DegreesToRadians(double deg)
//    //{
//    //    return deg * (Math.PI / 180);
//    //}




//    //---------------------------------------------------------------------------------------
//    //--------------------------------- postgis ---------------------------------------------
//    //---------------------------------------------------------------------------------------
//    //var drivers = await context.Set<DriverRole>()
//    //.Where(d => d.CurrentLocation != null &&
//    //    EF.Functions.Distance(
//    //        d.CurrentLocation.Value,
//    //        userLocation) < 5000)
//    //.ToListAsync();

//    //var drivers = await context.Set<DriverRole>()
//    //.Where(d => d.CurrentLocation != null &&
//    //    EF.Functions.Distance(
//    //        d.CurrentLocation.Value,
//    //        userLocation) < 5000)
//    //.ToListAsync();

//    //var drivers = await context.Set<DriverRole>()
//    //.Where(d => d.CurrentLocation != null &&
//    //    EF.Functions.Distance(
//    //        d.CurrentLocation.Value,
//    //        userLocation) < 5000)   // filter first
//    //.OrderBy(d => d.CurrentLocation.Value.Distance(userLocation)) // then sort
//    //.Take(10)
//    //.ToListAsync();

//    //var nearestDriver = await context.Set<DriverRole>()
//    //.Where(d => d.CurrentLocation != null)
//    //.OrderBy(d => d.CurrentLocation.Value.Distance(userLocation))
//    //.FirstOrDefaultAsync();

//    //var nearestDriver = await context.Set<DriverRole>()
//    //.Where(d => d.CurrentLocation != null &&
//    //    EF.Functions.Distance(
//    //        d.CurrentLocation.Value,
//    //        userLocation) < 10000)
//    //.OrderBy(d => d.CurrentLocation.Value.Distance(userLocation))
//    //.FirstOrDefaultAsync();

//    //double radius = 3000; // 3km

//    //var drivers = await context.Set<DriverRole>()
//    //    .Where(d => d.CurrentLocation != null &&
//    //        EF.Functions.Distance(
//    //            d.CurrentLocation.Value,
//    //            userLocation) < radius)
//    //    .ToListAsync();


//    internal class GeoService
//    {

//        public static (double lat, double lng) GenerateNearby(
//            double baseLat,
//            double baseLng,
//            double radiusMeters)
//        {
//            var baseLattt = 52.3676;
//            var baseLngggg = 4.9041;
//            var userLocation = new Point(baseLng, baseLat)
//            {
//                SRID = 4326
//            };
//            var random = new Random();

//            // Convert radius to degrees
//            var radiusInDegrees = radiusMeters / 111_320f;

//            var u = random.NextDouble();
//            var v = random.NextDouble();

//            var w = radiusInDegrees * Math.Sqrt(u);
//            var t = 2 * Math.PI * v;

//            var latOffset = w * Math.Cos(t);
//            var lngOffset = w * Math.Sin(t);

//            var newLat = baseLat + latOffset;
//            var newLng = baseLng + lngOffset;

//            return (newLat, newLng);
//        }
//    }
//}
