namespace helloAPI.Models;

public record Transactions{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Guid { get; set;}

    [ForeignKey("products")]
    public int ProductId { get; set; }
    public Products products {get; set;} = null!;

    [ForeignKey("userDetails")]
    public int BuyerId { get; set; }
    public UserDetails userDetails {get; set;} = null!;

    public DateTime Date {get;set;}

    public string? PaymentId{ get;set;}

    public TransactionStatus Status { get;set; }


}

public enum TransactionStatus{
    Success,
    Failed,
    Refunded
}