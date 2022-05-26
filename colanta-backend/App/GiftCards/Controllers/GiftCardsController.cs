using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace colanta_backend.App.GiftCards.Controllers
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GiftCards.Domain;
    using GiftCards.Application;

    [Route("api")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
        private GiftCardsSiesaRepository siesaRepository;
        public GiftCardsController(GiftCardsSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
        }
        // GET: api/<ValuesController>
        [HttpPost]
        [Route("giftcards/_search")]
        public async Task<GiftCardProviderDto[]> getGiftCardsByDocumentAndBusiness(object vtexInfo)
        {
            ListAllGiftCardByDocumentAndBusiness listAllGiftCardsByDocumentAndBussines = new ListAllGiftCardByDocumentAndBusiness(this.siesaRepository);
            GiftCard[] giftCards = await listAllGiftCardsByDocumentAndBussines.Invoke("1002999476", "mercolanta");
            List<GiftCardProviderDto> giftCardProviderDtos = new List<GiftCardProviderDto>();
            foreach (GiftCard giftCard in giftCards)
            {
                GiftCardProviderDto giftCardProviderDto = new GiftCardProviderDto();
                giftCardProviderDto.setDtoFromGiftCard(giftCard);
                giftCardProviderDtos.Add(giftCardProviderDto);
            }
            return giftCardProviderDtos.ToArray();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
