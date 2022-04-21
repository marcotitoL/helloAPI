namespace helloAPI.Models;

public record Transactions{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Guid { get; set;}

    [ForeignKey("products")]
    public int productId { get; set; }
    public Products products {get; set;} = null!;

    [ForeignKey("userDetails")]
    public int buyerId { get; set; }
    public UserDetails userDetails {get; set;} = null!;

    public DateTime date {get;set;}

    public int paymentId{ get;set;}


}