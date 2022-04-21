namespace helloAPI.DTO;

public class ProductsDTO{
    public string? Id {get;set;}

    public string? Name{get;set;}

    public string? Description {get;set;}

    public int Qty {get;set;}

    public decimal Price {get;set;}

    public idDTO Category {get;set;} = null!;

    public idDTO Seller {get;set;} = null!;
}