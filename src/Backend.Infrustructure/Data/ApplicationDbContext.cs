namespace Backend.Infrustructure.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account>               Accounts            => Set<Account>();
        public DbSet<Device>                Devices             => Set<Device>();
        public DbSet<Facility>              Facilities          => Set<Facility>();
        public DbSet<Door>                  Doors               => Set<Door>();
        public DbSet<Camera>                Cameras             => Set<Camera>();
        public DbSet<CameraDoorBinding>     CameraDoorBindings  => Set<CameraDoorBinding>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  

        }
    }

}
