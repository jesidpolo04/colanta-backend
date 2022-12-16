using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Credits.Controllers
{
    using Credits.Domain;
    using Credits.Application;
    using GiftCards.Domain;
    using Orders.SiesaOrders.Domain;
    using Microsoft.AspNetCore.Cors;

    [Route("api/cupo-lacteo")]
    [ApiController]
    public class CreditCheckoutController : ControllerBase
    {
        private CupoLacteoSiesaRepository creditsSiesaRepository;
        private GiftCardsRepository giftcardLocalRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;

        public CreditCheckoutController(CupoLacteoSiesaRepository creditsSiesaRepository, GiftCardsRepository giftcardLocalRepository, SiesaOrdersRepository siesaOrdersLocalRepository)
        {
            this.creditsSiesaRepository = creditsSiesaRepository;
            this.giftcardLocalRepository = giftcardLocalRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
        }

        [HttpPost]
        [EnableCors("Ecommerce")]
        [Route("generate")]
        public ActionResult Post(GenerateCupoLacteoCodeRequest request)
        {
            if (request.email == "" || request.email == null) return BadRequest();
            if (request.document == "" || request.document == null) return BadRequest();
            if(request.business == "" || request.business == null) return BadRequest();
            GenerateCupoLacteoGiftcard useCase = new GenerateCupoLacteoGiftcard(this.giftcardLocalRepository, creditsSiesaRepository, siesaOrdersLocalRepository);
            try
            {
                GiftCard giftcards = useCase.Invoke(request.document, request.email, request.business).Result;
                if(giftcards == null) return NotFound();
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
