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
    public class GiftCardsProviderController : ControllerBase
    {
        private GiftCardsRepository giftcardLocalRepository;
        private SkusRepository skusRepository;
        public GiftCardsProviderController(GiftCardsRepository giftcardsLocalRepository, SkusRepository skusRepository)
        {
            this.giftcardLocalRepository = giftcardsLocalRepository;
            this.skusRepository = skusRepository;
        }

        [HttpPost]
        [Route("giftcards")]
        public async Task<CreateGiftCardResponse> createGiftCard(CreateGiftCardRequest request)
        {
            CreateGiftcard useCase = new CreateGiftcard(this.giftcardLocalRepository);
            GiftCard giftCard = await useCase.Invoke(request);
            CreateGiftCardResponse response = new CreateGiftCardResponse();
            response.setFromGiftCard(giftCard);
            return response;
        }

        [HttpPost]
        [Route("giftcards/_search")] // obtener giftcards
        public async Task<GiftCardProviderDto[]> getGiftCardsByDocumentAndBusiness(ListAllGiftCardsRequestDto vtexInfo)
        {
            if(vtexInfo.client.document == null || vtexInfo.client.document == "") return new GiftCardProviderDto[0] {};
            if(vtexInfo.client.email == null || vtexInfo.client.email == "") return new GiftCardProviderDto[0] {};
            if(vtexInfo.cart.redemptionCode == null || vtexInfo.cart.redemptionCode == "") return new GiftCardProviderDto[0] {};

            SearchGiftcard useCase = new SearchGiftcard(this.giftcardLocalRepository, this.skusRepository);
            GiftCard[] giftCards = await useCase.Invoke(vtexInfo.client.document, vtexInfo.client.email, vtexInfo.cart.redemptionCode, vtexInfo.cart.items[0].refId);
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
            HttpContext.Response.Headers.Add("REST-Content-Range", "resources " + from + "-" + to + "/" + of);
            return giftCardProviderDtos.ToArray();
        }

        [HttpGet("giftcards/{giftCardId}")] // obtener giftcard
        public async Task<GiftCardDetailProviderResponseDto> getGiftCardBySiesaId(string giftCardId)
        {
            GetCreditByCode useCase = new GetCreditByCode(
                this.giftcardLocalRepository
                );
            GiftCard giftCard = await useCase.Invoke(giftCardId);
            GiftCardDetailProviderResponseDto response = new GiftCardDetailProviderResponseDto();
            response.setDtoFromGiftCard(giftCard);
            return response;
        }

        [HttpPost]
        [Route("giftcards/{giftCardId}/transactions")] //crear transaccion
        public async Task<TransactionSummaryDto> createGiftCardTransaction(string giftCardId, CreateGiftCardTransactionDto request)
        {
            CreateGiftCardTransaction useCase = new CreateGiftCardTransaction(this.giftcardLocalRepository);
            var transaction = await useCase.Invoke(giftCardId, request);
            return new TransactionSummaryDto(giftCardId, transaction.id);
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions")] // obtener transacciones
        public async Task<TransactionSummaryDto[]> getGiftCardTransactions(string giftCardId)
        {
            GetGiftCardTransactions useCase = new GetGiftCardTransactions(this.giftcardLocalRepository);
            Transaction[] transactions = await useCase.Invoke(giftCardId);
            List<TransactionSummaryDto> transactionsDto = new List<TransactionSummaryDto>();
            foreach (Transaction transaction in transactions)
            {
                TransactionSummaryDto transactionDto = new TransactionSummaryDto(transaction.card.siesa_id, transaction.id);
                transactionsDto.Add(transactionDto);
            }
            return transactionsDto.ToArray();
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}")] //obtener transaccion
        public async Task<GetTransactionByIdResponseDto> getGiftCardTransactionById(string giftCardId, string transactionId)
        {
            GetTransactionById useCase = new GetTransactionById(this.giftcardLocalRepository);
            Transaction transaction = await useCase.Invoke(transactionId);
            var response = GetTransactionByIdResponseMapper.getDto(transaction);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/authorization")] //obtener autorización
        public async Task<AuthorizationInfo> getGiftCardTransactionAuthorization(string giftCardId, string transactionId)
        {
            GetAuthorization useCase = new GetAuthorization(this.giftcardLocalRepository);
            TransactionAuthorization transactionAuthorization = await useCase.Invoke(transactionId);
            AuthorizationInfo response = new AuthorizationInfo();
            response.setFromTransactionAuthorization(transactionAuthorization);
            return response;
        }

        [HttpPost]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/settlements")] //generar liquidacion
        public async Task<SettlementInfoDto> generateGiftCardTransactionSettlement(string giftCardId, string transactionId, SettlementTransactionRequest body)
        {
            SettlementTransaction useCase = new SettlementTransaction(this.giftcardLocalRepository);
            TransactionSettlement settlement = await useCase.Invoke(transactionId, body.value);
            SettlementInfoDto response = new SettlementInfoDto();
            response.setFromTransactionSettlement(settlement);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/settlements")]
        public async Task<SettlementInfoDto[]> getGiftCardTransactionSettlements(string giftCardId, string transactionId)
        {
            GetTransactionSettlements useCase = new GetTransactionSettlements(this.giftcardLocalRepository);
            TransactionSettlement[] settlements = await useCase.Invoke(transactionId);
            List<SettlementInfoDto> settlementsDto = new List<SettlementInfoDto>();
            foreach (TransactionSettlement settlement in settlements)
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
            CancelTransaction useCase = new CancelTransaction(this.giftcardLocalRepository);
            TransactionCancellation transactionCancellation = await useCase.Invoke(transactionId, body.value);
            CancelInfoDto response = new CancelInfoDto();
            response.setFromTransactionCancellation(transactionCancellation);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/cancellations")]
        public async Task<CancelInfoDto[]> getGiftCardTransactionCancellations(string giftCardId, string transactionId)
        {
            GetTransactionCancellations useCase = new GetTransactionCancellations(this.giftcardLocalRepository);
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
