
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; } = null!;
        public DbSet<UserRefreshtokens> UserRefreshtokens { get; set; } = null!;
        public DbSet<Products> Products {get; set; } = null!;
        public DbSet<ProductsCategory> ProductsCategory {get;set;} = null!;
        public DbSet<Transactions> Transactions {get;set;} = null!;

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
             Balance = 1000.0m
         });

         var user = new IdentityUser{ 
             Id = "46b2c7ae-290f-4b18-ae66-39db50de0379",
             Email = "myemail@somedomain.com", 
             NormalizedEmail = "myemail@somedomain.com".ToUpper(),
             UserName = "myemail@somedomain.com", 
             NormalizedUserName ="myemail@somedomain.com".ToUpper(),  
             EmailConfirmed = true, 
             PhoneNumberConfirmed = true };

             user.PasswordHash = _passwordHasher.HashPassword( user, "Pa$$w0rd." );

         modelBuilder.Entity<IdentityUser>().HasData( user );

         modelBuilder.Entity<IdentityUserRole<string>>().HasData( new IdentityUserRole<string>{
             UserId = "46b2c7ae-290f-4b18-ae66-39db50de0379",
             RoleId = "06579486-4460-413c-bc04-ea719960dff7"
         });

         modelBuilder.Entity<UserDetails>().HasData( new UserDetails{
             Id = 99,
             Firstname = "John",
             Lastname = "Doe",
             AspNetUserId = "46b2c7ae-290f-4b18-ae66-39db50de0379",
             Balance = 4.30m
         });

         modelBuilder.Entity<ProductsCategory>().HasData( new ProductsCategory{
             Id = 1,
             Guid = "e2cfd51c-47cc-4b86-b0de-216ede287fe5",
             CategoryName = "Miscellaneous",
         } );

         modelBuilder.Entity<ProductsCategory>().HasData( new ProductsCategory{
             Id = 2,
             Guid = "9005481f-5f2a-470f-9357-3115f37b9f81",
             CategoryName = "Hats",
         } );


         modelBuilder.Entity<Products>().HasData( new Products{
             Id = 1,
             Guid = "b7b2522b-df88-4286-a608-c9a7587d1b7a",
             Name = "Adidas Running Cap",
             Description = "slightly used running cap, still works great",
             Qty = 1,
             Price = 29.50m,
             CategoryId = 2,
             UserId = 99
         });

        }
    }
