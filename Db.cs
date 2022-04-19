
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<UserRefreshtokens> UserRefreshtokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /*
            Pardon the hardcode, i could probably populate these directly in DB
            */

            base.OnModelCreating(modelBuilder);

         Dictionary<string,string> roles = new Dictionary<string,string>(){
             {"573718cc-2ff6-4980-800d-f73e0605649a", "Super Administrator"},
             {"fb0b8efb-97e4-4302-8b84-a8670d84d0e0", "Administrator"},
             {"06579486-4460-413c-bc04-ea719960dff7", "User"},
         };

         foreach( var role in roles ){
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = role.Key, Name = role.Value, NormalizedName = role.Value.ToUpper() });
         }

         PasswordHasher<IdentityUser> _passwordHasher = new PasswordHasher<IdentityUser>();

         var superAdmin = new IdentityUser{ 
             Id = "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
             Email = "marco.lambo@vogappdevelopers.com", 
             NormalizedEmail = "marco.lambo@vogappdevelopers.com".ToUpper(),
             UserName = "marco.lambo@vogappdevelopers.com", 
             NormalizedUserName ="marco.lambo@vogappdevelopers.com".ToUpper(),  
             EmailConfirmed = true, 
             PhoneNumberConfirmed = true };

             superAdmin.PasswordHash = _passwordHasher.HashPassword( superAdmin, "Pa$$w0rd." );

         modelBuilder.Entity<IdentityUser>().HasData( superAdmin );

         modelBuilder.Entity<IdentityUserRole<string>>().HasData( new IdentityUserRole<string>{
             UserId = "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
             RoleId = "573718cc-2ff6-4980-800d-f73e0605649a"
         });

         modelBuilder.Entity<UserDetails>().HasData( new UserDetails{
             Id = 1,
             Firstname = "Super",
             Lastname = "Admin",
             AspNetUserId = "0e45e701-4bfe-4867-8550-87f6ae9bf6c8",
             Balance = 0.0m
         });

        }
    }
