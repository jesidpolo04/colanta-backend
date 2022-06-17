using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace colanta_backend.App.GiftCards.Controllers
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GiftCards.Domain;
    using GiftCards.Application;

    [Route("")]
    [ApiController]
    public class GiftCardsController : ControllerBase
    {
        private GiftCardsRepository localRepository;
        private GiftCardsSiesaRepository siesaRepository;
        public GiftCardsController(GiftCardsRepository localRepository, GiftCardsSiesaRepository siesaRepository)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
        }
        // GET: api/<ValuesController>
        [HttpPost]
        [Route("api/giftcards/_search")]
        public async Task<GiftCardProviderDto[]> getGiftCardsByDocumentAndBusiness(object vtexInfo)
        {
            ListAllGiftCardByDocumentAndBusiness listAllGiftCardsByDocumentAndBussines = new ListAllGiftCardByDocumentAndBusiness(this.localRepository, this.siesaRepository);
            GiftCard[] giftCards = await listAllGiftCardsByDocumentAndBussines.Invoke("1002999476", "mercolanta");
            List<GiftCardProviderDto> giftCardProviderDtos = new List<GiftCardProviderDto>();
            foreach (GiftCard giftCard in giftCards)
            {
                GiftCardProviderDto giftCardProviderDto = new GiftCardProviderDto();
                giftCardProviderDto.setDtoFromGiftCard(giftCard);
                giftCardProviderDtos.Add(giftCardProviderDto);
            }
            int from = 0;
            int to = giftCards.Length;
            int of = giftCards.Length;
            HttpContext.Response.Headers.Add("REST-Content-Range", "resources " + from+"-"+to+"/"+of);
            return giftCardProviderDtos.ToArray();
        }

        // GET api/<ValuesController>/5
        [HttpGet("api/giftcards/{giftCardId}")]
        public async Task<GiftCardDetailProviderResponseDto> getGiftCardBySiesaId(string giftCardId)
        {
            GetAndUpdateGiftCardBySiesaId getAndUpdateGiftCardBySiesaId = new GetAndUpdateGiftCardBySiesaId(
                this.localRepository,
                this.siesaRepository
                );
            GiftCard giftCard = await getAndUpdateGiftCardBySiesaId.Invoke(giftCardId);
            GiftCardDetailProviderResponseDto response = new GiftCardDetailProviderResponseDto();
            response.setDtoFromGiftCard(giftCard);
            return response;
        }

        [HttpPost]
        [Route("/giftcards/{giftCardId}/transactions/{transactionId}/cancellations")]
        public async Task<TransactionSummaryDto> giftCardTransaction(string giftCardId, string transactionId, object body)
        {
            TransactionSummaryDto transactionSummaryDto = new TransactionSummaryDto();
            transactionSummaryDto.setCardId(giftCardId);
            return transactionSummaryDto;
        }

    }
}
