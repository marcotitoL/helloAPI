namespace helloAPI.DTO;

public class TransactionsDTO{

    public int Guid {get;internal set;}

    public string? ProductId {get;set;}

    public string? BuyerId {get;set;}

    [DataType(DataType.Date)]
    public DateTime Date {get;set;} 

    public string? PaymentId {get;set;}

}