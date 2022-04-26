#nullable disable

namespace helloAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TransactionsController(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

///<summary>buy a product</summary>
///<remarks>
/// this endpoint accepts a product id and a user id
///
/// Returns a stripe publishable key and a payment intent client secret for implentation
/// with Stripe API
///
/// for this demo purpose, it also returns a link to a working implementation
/// using the key and client secret returned written in VanillaJS
///</remarks>
        [HttpPost,Route("buy"),Authorize]
        public async Task<IActionResult> Buy( BuyDTO buyDTO ){

            StripeConfiguration.ApiKey = _configuration["Stripe:Secret_key"];

            var productPrice =  Convert.ToInt64( await _context.Products.Where( p => p.Guid == buyDTO.Product.Id ).Select(p => p.Price).SingleAsync() * 100 );


            var options = new PaymentIntentCreateOptions{
                Amount = productPrice,                
                Currency = "cad",
                PaymentMethodTypes = new List<string>{
                    "card",
                },
                Metadata = new Dictionary<string, string>{
                    {"BuyerId",buyDTO.Buyer.Id},
                    {"ProductId",buyDTO.Product.Id},
                }
            };

            var service = new PaymentIntentService();

            var intent = service.Create(options);

            return Ok( new { 
                stripe_publishableKey = _configuration["Stripe:Publishable_key"],
                intent_ClientSecret = intent.ClientSecret,
            sampleHtmlJsImplementation = $"{HttpContext.Request.Scheme }://" + HttpContext.Request.Host.ToUriComponent() + "/testPaymentPage?pk=" + _configuration["Stripe:Publishable_key"] + "&s=" + intent.ClientSecret } );


        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost,Route("/webhooks")]
        public async Task<IActionResult> webhook( ){
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    HttpContext.Request.Headers["Stripe-Signature"],
                    "whsec_54b48a5a814b99e8227eda43e83de5f3c2ea951daf5b2e5f55b54a20afbbca3d"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                
                //foreach( var m in paymentIntent.Metadata){
                    //Console.WriteLine( m.Key + " = " + m.Value );
                //}
                Transactions transaction = new Transactions(){
                    Guid = Guid.NewGuid().ToString(),
                    ProductId = await _context.Products.Where( p => p.Guid == paymentIntent.Metadata["ProductId"] ).Select(p => p.Id).SingleAsync()  ,
                    BuyerId = await _context.UserDetails.Where( u => u.AspNetUserId == paymentIntent.Metadata["BuyerId"] ).Select( u => u.Id ).SingleAsync() ,
                    Date = DateTime.Now,
                    PaymentId = paymentIntent.Id,
                    Status = TransactionStatus.Success
                };

                _context.Transactions.Add( transaction );

                var result = await _context.SaveChangesAsync();

                if( result > 0  ){

                    //update product inventory / status
                    var productBought = await _context.Products.FindAsync( transaction.ProductId );
                    var seller = await _context.UserDetails.FindAsync( productBought.UserId );
                    seller.Balance += paymentIntent.Amount;

                    productBought.Qty -= 1;

                    if( productBought.Qty == 0 )
                        productBought.Status = ProductStatus.Sold;

                    _context.Entry(productBought).State = EntityState.Modified;
                    _context.Entry(seller).State = EntityState.Modified;

                    await _context.SaveChangesAsync();


                    return Ok( new { Transaction = new { ID = transaction.Guid, striperef = paymentIntent.Id } } );
                }
                else
                    return BadRequest();
                
            }

            
            
            

            //Console.WriteLine( $"BuyerId {paymentIntent.Metadata["BuyerId"]}");

            return Ok();
        }

///<remarks>
///accepts the transaction ID. transaction ID is returned from another endpoint
///that lists all transactions of a user, still a *TODO*
///</remarks>
        [HttpPost,Route("refund"),Authorize(Roles="Super Administrator,Administrator")]
        public async Task<IActionResult> Refund( RefundDTO refundDTO ){

            var transactionToRefund = await _context.Transactions.Where( t => t.Guid == refundDTO.Transaction.Id ).FirstOrDefaultAsync();

            StripeConfiguration.ApiKey = _configuration["Stripe:Secret_key"];
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(transactionToRefund.PaymentId);

            var chargeId = paymentIntent.Charges.Data[0].Id;

            var options = new RefundCreateOptions{ Charge = chargeId };
            var refundService = new RefundService();
            await refundService.CreateAsync(options);

            transactionToRefund.Status = TransactionStatus.Refunded;

            _context.Entry(transactionToRefund).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync();

            if( result > 0 ){

                var refundedProduct = await _context.Products.FindAsync( transactionToRefund.ProductId );
                refundedProduct.Refund( 1 );
                _context.Entry(refundedProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok( new {message = "Successfully refunded" });

            }
            else
                return BadRequest();

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet,Route("/testPaymentPage")]
        public ContentResult testPaymentPage( string pk, string s ){
            string html = "<script src=\"https://js.stripe.com/v3/\"></script>"+ "\n";
                  html += @"<form id='payment-form'>
        <div id='payment-element'>
          <!-- Elements will create form elements here -->
        </div>
        <button id='submit'>Pay now</button>
        <div id='error-message'>
          <!-- Display error message to your customers here -->
        </div>
      </form>";
            html += "<script>" + "\n";
            html += "var stripe = Stripe('" +pk+ "');"+ "\n";
            html += "var elements = stripe.elements({clientSecret: '"+ s +"',});"+ "\n";
            //html += "var paymentElement = elements.create('payment');"+ "\n";
            html += @"// Customize which fields are collected by the Payment Element
var paymentElement = elements.create('payment', {
  fields: {
    billingDetails: {
      name: 'never',
      email: 'never',
    }
  }
});

paymentElement.mount(""#payment-element"");

// If you disable collecting fields in the Payment Element, you
// must pass equivalent data when calling `stripe.confirmPayment`.
document.getElementById(""payment-form"").addEventListener('submit', async (event) => {
      event.preventDefault();
      document.getElementById('submit').disabled = true;
  stripe.confirmPayment({
      redirect: 'if_required',
    elements,
    confirmParams: {
      return_url: 'https://example.com',
      payment_method_data: {
        billing_details: {
          name: 'Jenny Rosen',
          email: 'jenny.rosen@example.com',
        }
      },
    },
  })
  .then(function(result) {
    if (result.error) {
      // Inform the customer that there was an error.
    }
    else{
        document.getElementById('payment-form').style.display = 'none';
        alert('Payment has succeeded, close this window!');
    }
  });
});" + "\n";
            html += "</script>"+ "\n";
            return base.Content(html,"text/html");
        }
    }
}
