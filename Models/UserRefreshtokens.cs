namespace helloAPI.Models;

[Index(nameof(AspNetUserId), IsUnique = true )]
public record UserRefreshtokens{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; internal set;}

    public string? Refreshtoken { get; set; }

    public DateTime Expiry{ get; set; }

    [ForeignKey("identityUser")]
    public string? AspNetUserId { get; set; }
    public IdentityUser identityUser { get; set; } = null!;
    
}