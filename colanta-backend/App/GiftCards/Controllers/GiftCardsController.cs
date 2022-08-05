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
        private GiftCardsRepository localRepository;
        private GiftCardsSiesaRepository siesaRepository;
        public GiftCardsController(GiftCardsRepository localRepository, GiftCardsSiesaRepository siesaRepository)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
        }

        [HttpPost]
        [Route("giftcards")]
        public async Task<CreateGiftCardResponse> createGiftCard(CreateGiftCardRequest request)
        {
            CreateGiftcard useCase = new CreateGiftcard(this.localRepository);
            GiftCard giftCard = await useCase.Invoke(request);
            CreateGiftCardResponse response = new CreateGiftCardResponse();
            response.setFromGiftCard(giftCard);
            return response;
        }

        [HttpPost]
        [Route("giftcards/_search")] // obtener giftcards
        public async Task<GiftCardProviderDto[]> getGiftCardsByDocumentAndBusiness(ListAllGiftCardsRequestDto vtexInfo)
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

        [HttpGet("giftcards/{giftCardId}")] // obtener giftcard
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
        [Route("giftcards/{giftCardId}/transactions")] //crear transaccion
        public async Task<TransactionSummaryDto> createGiftCardTransaction(string giftCardId, CreateGiftCardTransactionDto request)
        {
            CreateGiftCardTransaction useCase = new CreateGiftCardTransaction(this.localRepository);
            return await useCase.Invoke(giftCardId, request);
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions")] // obtener transacciones
        public async Task<TransactionSummaryDto[]> getGiftCardTransactions(string giftCardId)
        {
            GetGiftCardTransactions useCase = new GetGiftCardTransactions(this.localRepository);
            Transaction[] transactions = await useCase.Invoke(giftCardId);
            List<TransactionSummaryDto> transactionsDto = new List<TransactionSummaryDto>();
            foreach(Transaction transaction in transactions)
            {
                TransactionSummaryDto transactionDto = new TransactionSummaryDto(transaction.card.siesa_id, transaction.id);
                transactionsDto.Add(transactionDto);
            }
            return transactionsDto.ToArray();
        } 

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}")] //obtener transaccion
        public async Task<GetTransactionByIdResponseDto> getGiftCardTransactionById (string giftCardId, string transactionId)
        {
            GetTransactionById useCase = new GetTransactionById(this.localRepository);
            Transaction transaction = await useCase.Invoke(transactionId);
            var response = GetTransactionByIdResponseMapper.getDto(transaction);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/authorization")] //obtener autorización
        public async Task<AuthorizationInfo> getGiftCardTransactionAuthorization(string giftCardId, string transactionId)
        {
            GetAuthorization useCase = new GetAuthorization(this.localRepository);
            TransactionAuthorization transactionAuthorization = await useCase.Invoke(transactionId);
            AuthorizationInfo response = new AuthorizationInfo();
            response.setFromTransactionAuthorization(transactionAuthorization);
            return response;
        }

        [HttpPost]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/settlements")] //generar liquidacion
        public async Task<SettlementInfoDto> generateGiftCardTransactionSettlement(string giftCardId, string transactionId, SettlementTransactionRequest body)
        {
            SettlementTransaction useCase = new SettlementTransaction(this.localRepository);
            TransactionSettlement settlement = await useCase.Invoke(transactionId, body.value);
            SettlementInfoDto response = new SettlementInfoDto();
            response.setFromTransactionSettlement(settlement);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/settlements")]
        public async Task<SettlementInfoDto[]> getGiftCardTransactionSettlements(string giftCardId, string transactionId)
        {
            GetTransactionSettlements useCase = new GetTransactionSettlements(this.localRepository);
            TransactionSettlement[] settlements = await useCase.Invoke(transactionId);
            List<SettlementInfoDto> settlementsDto = new List<SettlementInfoDto>();
            foreach(TransactionSettlement settlement in settlements)
            {
                SettlementInfoDto settlementDto = new SettlementInfoDto();
                settlementDto.setFromTransactionSettlement(settlement);
                settlementsDto.Add(settlementDto);
            }
            return settlementsDto.ToArray();
        } 

        [HttpPost]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/cancellations")] // cancelar
        public async Task<CancelInfoDto> cancelGiftcardTransaction(string giftCardId, string transactionId, CancelTransactionRequest body)
        {
            CancelTransaction useCase = new CancelTransaction(this.localRepository);
            TransactionCancellation transactionCancellation = await useCase.Invoke(transactionId, body.value);
            CancelInfoDto response = new CancelInfoDto();
            response.setFromTransactionCancellation(transactionCancellation);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/cancellations")]
        public async Task<CancelInfoDto[]> getGiftCardTransactionCancellations(string giftCardId, string transactionId)
        {
            GetTransactionCancellations useCase = new GetTransactionCancellations(this.localRepository);
            TransactionCancellation[] cancellations = await useCase.Invoke(transactionId);
            List<CancelInfoDto> cancellationsDto = new List<CancelInfoDto>();
            foreach (TransactionCancellation cancellation in cancellations)
            {
                CancelInfoDto cancellationDto = new CancelInfoDto();
                cancellationDto.setFromTransactionCancellation(cancellation);
                cancellationsDto.Add(cancellationDto);
            }
            return cancellationsDto.ToArray();
        }
    }
}
