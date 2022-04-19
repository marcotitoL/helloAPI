namespace helloAPI.Models;

    public record UserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; internal set; }

        public string Firstname {get;set;} = null!;
        public string Lastname {get;set;} = null!;

        public string? ProfileImage {get;set;}

        [DataType(DataType.Date)]
        public DateTime Birthdate{get;set;}

        [Precision(10,2)]
        public Decimal Balance{get;set;}
        
        [ForeignKey("identityUser")]
        public string? AspNetUserId { get; set; }
        public IdentityUser identityUser {get;set;} = null!;
        
    }
