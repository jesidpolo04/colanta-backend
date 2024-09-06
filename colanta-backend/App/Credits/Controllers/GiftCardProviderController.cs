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
    using MicrosoftLogging = Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using System.Linq;
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.Configuration;

    [Route("api/cupo-lacteo")]
    [ApiController]
    public class GiftCardsProviderController : ControllerBase
    {
        private GiftCardsRepository giftcardLocalRepository;
        private SkusRepository skusRepository;
        private MicrosoftLogging.ILogger fileLogger;
        private readonly IConfiguration configuration;

        public GiftCardsProviderController(
            GiftCardsRepository giftcardsLocalRepository,
            SkusRepository skusRepository,
            MicrosoftLogging.ILogger<GiftCardsProviderController> fileLogger,
            IConfiguration configuration
        )
        {
            this.giftcardLocalRepository = giftcardsLocalRepository;
            this.skusRepository = skusRepository;
            this.fileLogger = fileLogger;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("giftcards")]
        public async Task<ActionResult<CreateGiftCardResponse>> createGiftCard(CreateGiftCardRequest request)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
            CreateGiftcard useCase = new CreateGiftcard(this.giftcardLocalRepository);
            GiftCard giftCard = await useCase.Invoke(request);
            CreateGiftCardResponse response = new CreateGiftCardResponse();
            response.setFromGiftCard(giftCard);
            return response;
        }

        [HttpPost]
        [Route("giftcards/_search")] // obtener giftcards
        public async Task<ActionResult<GiftCardProviderDto[]>> getGiftCardsByDocumentAndBusiness(ListAllGiftCardsRequestDto vtexInfo)
        {
            try
            {
                if(!ValidateAuthHeaders()){
                    return Unauthorized();
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                this.fileLogger.LogDebug($"Buscando giftcards de: {vtexInfo.client.document} : {JsonSerializer.Serialize(vtexInfo)}");

                if (vtexInfo.client.document == null || vtexInfo.client.document == "") return new GiftCardProviderDto[0] { };
                if (vtexInfo.client.email == null || vtexInfo.client.email == "") return new GiftCardProviderDto[0] { };
                if (vtexInfo.cart.redemptionCode == null || vtexInfo.cart.redemptionCode == "") return new GiftCardProviderDto[0] { };

                SearchGiftcard useCase = new SearchGiftcard(this.giftcardLocalRepository);
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
                var codes = giftCards.ToList().ConvertAll(giftCard =>
                    {
                        return giftCard.code;
                    });
                stopwatch.Stop();
                this.fileLogger.LogDebug($"Retornando giftcards {string.Join(",", codes)}", giftCardProviderDtos);
                this.fileLogger.LogDebug("Tiempo de retorno: {Time}", stopwatch.Elapsed);
                return giftCardProviderDtos.ToArray();
            }
            catch (Exception exception)
            {
                this.fileLogger.LogDebug($"Excepcion: {exception.Message} al buscar giftcards de: {vtexInfo.client.document}");
                return NotFound();
            }
        }

        [HttpGet("giftcards/{giftCardId}")] // obtener giftcard
        public async Task<ActionResult<GiftCardDetailProviderResponseDto>> getGiftCardBySiesaId(string giftCardId)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
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
        public async Task<ActionResult<TransactionSummaryDto>> createGiftCardTransaction(string giftCardId, CreateGiftCardTransactionDto request)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
            CreateGiftCardTransaction useCase = new CreateGiftCardTransaction(this.giftcardLocalRepository);
            var transaction = await useCase.Invoke(giftCardId, request);
            return new TransactionSummaryDto(giftCardId, transaction.id);
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions")] // obtener transacciones
        public async Task<ActionResult<TransactionSummaryDto[]>> getGiftCardTransactions(string giftCardId)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
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
        public async Task<ActionResult<GetTransactionByIdResponseDto>> getGiftCardTransactionById(string giftCardId, string transactionId)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
            GetTransactionById useCase = new GetTransactionById(this.giftcardLocalRepository);
            Transaction transaction = await useCase.Invoke(transactionId);
            var response = GetTransactionByIdResponseMapper.getDto(transaction);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/authorization")] //obtener autorización
        public async Task<ActionResult<AuthorizationInfo>> getGiftCardTransactionAuthorization(string giftCardId, string transactionId)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
            GetAuthorization useCase = new GetAuthorization(this.giftcardLocalRepository);
            TransactionAuthorization transactionAuthorization = await useCase.Invoke(transactionId);
            AuthorizationInfo response = new AuthorizationInfo();
            response.setFromTransactionAuthorization(transactionAuthorization);
            return response;
        }

        [HttpPost]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/settlements")] //generar liquidacion
        public async Task<ActionResult<SettlementInfoDto>> generateGiftCardTransactionSettlement(string giftCardId, string transactionId, SettlementTransactionRequest body)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
            SettlementTransaction useCase = new SettlementTransaction(this.giftcardLocalRepository);
            TransactionSettlement settlement = await useCase.Invoke(transactionId, body.value);
            SettlementInfoDto response = new SettlementInfoDto();
            response.setFromTransactionSettlement(settlement);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/settlements")]
        public async Task<ActionResult<SettlementInfoDto[]>> getGiftCardTransactionSettlements(string giftCardId, string transactionId)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
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
        public async Task<ActionResult<CancelInfoDto>> cancelGiftcardTransaction(string giftCardId, string transactionId, CancelTransactionRequest body)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
            CancelTransaction useCase = new CancelTransaction(this.giftcardLocalRepository);
            TransactionCancellation transactionCancellation = await useCase.Invoke(transactionId, body.value);
            CancelInfoDto response = new CancelInfoDto();
            response.setFromTransactionCancellation(transactionCancellation);
            return response;
        }

        [HttpGet]
        [Route("giftcards/{giftCardId}/transactions/{transactionId}/cancellations")]
        public async Task<ActionResult<CancelInfoDto[]>> getGiftCardTransactionCancellations(string giftCardId, string transactionId)
        {
            if(!ValidateAuthHeaders()){
                return Unauthorized();
            }
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

        public bool ValidateAuthHeaders()
        {
            // Obtener los valores esperados de la configuración
            string expectedAppKey = configuration["ProviderCupoAppKey"];
            string expectedAppToken = configuration["ProviderCupoAppToken"]; // Se cambió para usar el token correcto

            // Variables para almacenar los valores obtenidos de los headers
            string appKey;
            string appToken;

            // Verificar si los headers existen y obtener sus valores
            if (!Request.Headers.TryGetValue("AppKey", out var headerAppKey) ||
                !Request.Headers.TryGetValue("AppToken", out var headerAppToken))
            {
                // Si falta algún header, retornar false
                return false;
            }

            // Comparar los valores de los headers con los esperados
            appKey = headerAppKey.ToString();
            appToken = headerAppToken.ToString();

            return appKey == expectedAppKey && appToken == expectedAppToken;
        }
    }
}
