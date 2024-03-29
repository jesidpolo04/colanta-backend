﻿namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using Products.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Linq;
    using System.Collections.Generic;
    using MicrosoftLogging = Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging;

    public class SearchGiftcards
    {
        private GiftCardsSiesaRepository siesaRepository;
        private GiftCardsRepository localRepository;
        private SkusRepository skusLocalRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;

        private MicrosoftLogging.ILogger fileLogger;

        public SearchGiftcards(
            GiftCardsRepository localRepository,
            GiftCardsSiesaRepository siesaRepository,
            SkusRepository skusLocalRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            MicrosoftLogging.ILogger fileLogger
            )
        {
            this.siesaRepository = siesaRepository;
            this.localRepository = localRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.fileLogger = fileLogger;
        }

        public async Task<GiftCard[]> Invoke(string document, string skuRefId, string redemptionCode)
        {
            if (redemptionCode == "" || redemptionCode == null) return new GiftCard[0] { };
            string business = "mercolanta";
            GiftCard[] siesaGiftCards = await this.siesaRepository.getGiftCardsByDocumentAndBusiness(document, business);
            foreach (GiftCard siesaGiftCard in siesaGiftCards)
            {
                GiftCard localGiftCard = await localRepository.getGiftCardBySiesaId(siesaGiftCard.siesa_id);

                if (localGiftCard == null) await localRepository.saveGiftCard(siesaGiftCard);
                if (localGiftCard != null && !(this.userHasPendingOrders(document))) this.updateGiftcardBalance(localGiftCard);
            }

            GiftCard[] localGiftcards = this.getAvailableGiftcards(document, business);
            return localGiftcards.Where(giftcard => giftcard.code == redemptionCode).ToArray();
        }

        private GiftCard[] getAvailableGiftcards(string document, string business)
        {
            this.fileLogger.LogDebug($"Obteniendo bonos disponibles");
            List<GiftCard> availableGiftcards = new List<GiftCard>();
            GiftCard[] giftcards = this.localRepository.getGiftCardsByDocumentAndBusiness(document, business).Result;
            foreach (GiftCard giftcard in giftcards)
            {
                if (
                    !giftcard.used &&
                    !giftcard.isExpired() &&
                    giftcard.provider == Providers.GIFTCARDS
                    )
                {
                    availableGiftcards.Add(giftcard);
                }
            }
            return availableGiftcards.ToArray();
        }

        private bool userHasPendingOrders(string document)
        {
            this.fileLogger.LogDebug($"Obteniendo ordenes pendientes de usuario");
            SiesaOrder[] userOrders = this.siesaOrdersLocalRepository.getSiesaOrdersByDocument(document).Result;
            SiesaOrder[] unfinishedUserOrder = userOrders.Where(siesaOrder => siesaOrder.finalizado == false).ToArray();
            return unfinishedUserOrder.Length > 0 ? true : false;
        }

        private void updateGiftcardBalance(GiftCard localGiftcard)
        {
            this.fileLogger.LogDebug($"Actualizando balance de la giftcard");
            decimal newCardBalance = this.siesaRepository.getGiftCardBalanceBySiesaId(localGiftcard.siesa_id).Result;
            localGiftcard.updateBalance(newCardBalance);
            this.localRepository.updateGiftCard(localGiftcard).Wait();
        }
    }
}
