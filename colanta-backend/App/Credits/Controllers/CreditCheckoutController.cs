using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Credits.Controllers
{
    using Credits.Domain;
    using Credits.Application;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GiftCards.Domain;
    using Products.Domain;
    using Orders.SiesaOrders.Domain;
    using GiftCards.Application;

    [Route("api/cupo-lacteo")]
    [ApiController]
    public class CreditCheckoutController : ControllerBase
    {
        private CreditsSiesaRepository creditsSiesaRepository;
        private GiftCardsRepository giftcardLocalRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;

        public CreditCheckoutController(CreditsSiesaRepository creditsSiesaRepository, GiftCardsRepository giftcardLocalRepository, SiesaOrdersRepository siesaOrdersLocalRepository)
        {
            this.creditsSiesaRepository = creditsSiesaRepository;
            this.giftcardLocalRepository = giftcardLocalRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
        }

        [HttpPost]
        [Route("generate")]
        public ActionResult Post(GenerateCupoLacteoCodeRequest request)
        {
            if (request.email == "" || request.email == null) return BadRequest();
            if (request.document == "" || request.document == null) return BadRequest();
            GenerateCode useCase = new GenerateCode(this.giftcardLocalRepository, creditsSiesaRepository, siesaOrdersLocalRepository);
            try
            {
                GiftCard[] giftcards = useCase.Invoke(request.document, request.email).Result;
                if(giftcards.Length == 0) return NotFound();
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
