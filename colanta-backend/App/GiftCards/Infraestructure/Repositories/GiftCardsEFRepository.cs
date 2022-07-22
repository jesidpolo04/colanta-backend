namespace colanta_backend.App.GiftCards.Infraestructure
{
    using App.GiftCards.Domain;
    using App.Shared.Infraestructure;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    public class GiftCardsEFRepository : Domain.GiftCardsRepository
    {
        private ColantaContext dbContext;

        public GiftCardsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<GiftCard[]> getAllGiftCards()
        {
            EFGiftCard[] efGiftCards = this.dbContext.GiftCards.ToArray();
            List<GiftCard> giftCards = new List<GiftCard>();
            foreach (EFGiftCard efGiftCard in efGiftCards)
            {
                giftCards.Add(efGiftCard.getGiftCardFromEfGiftCard());
            }
            return giftCards.ToArray();
        }

        public async Task<GiftCard> getGiftCardBySiesaId(string siesaId)
        {
            var efGiftCards = this.dbContext.GiftCards.Where(giftcard => giftcard.siesa_id == siesaId);
            if(efGiftCards.ToArray().Length > 0)
            {
                return efGiftCards.First().getGiftCardFromEfGiftCard();
            }
            return null;
        }

        public async Task<GiftCard[]> getGiftCardsByDocumentAndBusiness(string ownerDocument, string business)
        {
            EFGiftCard[] efGiftCards = this.dbContext.GiftCards.Where(giftcard => giftcard.owner == ownerDocument && giftcard.business == business).ToArray();
            List<GiftCard> giftCards = new List<GiftCard>();
            foreach(EFGiftCard efGiftCard in efGiftCards)
            {
                giftCards.Add(efGiftCard.getGiftCardFromEfGiftCard());
            }
            return giftCards.ToArray();
        }

        public async Task<Transaction[]> getGiftCardTransactions(int giftCardId)
        {
            EFGiftCardTransaction[] efTransactions = this.dbContext.GiftCardsTransactions
                .Include(transaction => transaction.card)
                .Include(transaction => transaction.transaction_authorization)
                .Where(transaction => transaction.card_id == giftCardId).ToArray();
            List<Transaction> transactions = new List<Transaction>();
            foreach(EFGiftCardTransaction efTransaction in efTransactions)
            {
                transactions.Add(efTransaction.getTransaction());
            }
            return transactions.ToArray();
        }

        public async Task<Transaction> getTransaction(string transactionId)
        {
            EFGiftCardTransaction efTransaction = this.dbContext.GiftCardsTransactions
                .Include(transaction => transaction.transaction_authorization)
                .Include(transaction => transaction.card)
                .Where(transaction => transaction.id == transactionId).First();
               
            return efTransaction.getTransaction();
        }

        public async Task<TransactionAuthorization> getTransactionAuthorization(string transactionId)
        {
            EFGiftCardTransactionAuthorization efTransactionAuthorization = this.dbContext.GiftCardsTransactionsAuthorizations
                .Include(authorization => authorization.transaction)
                .Where(authorization => authorization.transaction.id == transactionId).First();
            return efTransactionAuthorization.getTransactionAuthorization();
        }

        public async Task<TransactionCancellation[]> getTransactionCancellations(string transactionId)
        {
            EFGiftCardTransactionCancellation[] efTransactionCancellations = this.dbContext.GiftCardsTransactionsCancellations
                .Include(authorization => authorization.transaction)
                .Where(cancellations => cancellations.transaction.id == transactionId).ToArray();
            List<TransactionCancellation> transactionCancellations = new List<TransactionCancellation>();
            foreach(EFGiftCardTransactionCancellation efCancellation in efTransactionCancellations)
            {
                transactionCancellations.Add(efCancellation.getTransactionCancellation());
            }
            return transactionCancellations.ToArray();
        }

        public async Task<TransactionSettlement[]> GetTransactionSettlements(string transactionId)
        {
            EFGiftCardTransactionSettlement[] efTransactionSettlements = this.dbContext.GiftCardsTransactionsSettlements
                .Include(authorization => authorization.transaction)
                .Where(settlements => settlements.transaction.id == transactionId).ToArray();
            List<TransactionSettlement> transactionSettlements = new List<TransactionSettlement>();
            foreach(EFGiftCardTransactionSettlement efSettlement in efTransactionSettlements)
            {
                transactionSettlements.Add(efSettlement.getTransactionSettlement());
            }
            return transactionSettlements.ToArray();
        }

        public async Task<GiftCard> saveGiftCard(GiftCard giftCard)
        {
            EFGiftCard efGiftCard = new EFGiftCard();
            efGiftCard.setEfGiftCardFromGiftCard(giftCard);
            this.dbContext.Add(efGiftCard);
            this.dbContext.SaveChanges();
            return await this.getGiftCardBySiesaId(giftCard.siesa_id);
        }

        public async Task<Transaction> saveGiftCardTransaction(Transaction transaction)
        {
            EFGiftCardTransaction efTransaction = new EFGiftCardTransaction();
            efTransaction.setEfTransaction(transaction);
            this.dbContext.Add(efTransaction);
            this.dbContext.SaveChanges();
            return transaction;
        }

        public async Task<TransactionAuthorization> saveTransactionAuthorization(TransactionAuthorization transactionAuthorization)
        {
            EFGiftCardTransactionAuthorization efAuthorization = new EFGiftCardTransactionAuthorization();
            efAuthorization.setEfTransactionAuthorization(transactionAuthorization);
            this.dbContext.Add(efAuthorization);
            this.dbContext.SaveChanges();
            return transactionAuthorization;
        }

        public async Task<TransactionCancellation> saveTransactionCancellation(TransactionCancellation transactionCancellation)
        {
            EFGiftCardTransactionCancellation efCancellation = new EFGiftCardTransactionCancellation();
            efCancellation.setEfTransactionCancellation(transactionCancellation);
            this.dbContext.Add(efCancellation);
            this.dbContext.SaveChanges();
            return transactionCancellation;
        }

        public async Task<TransactionSettlement> saveTransactionSettlement(TransactionSettlement transactionSettlement)
        {
            EFGiftCardTransactionSettlement efSettlement = new EFGiftCardTransactionSettlement();
            efSettlement.setEfTransactionSettlement(transactionSettlement);
            this.dbContext.Add(efSettlement);
            this.dbContext.SaveChanges();
            return transactionSettlement;
        }

        public async Task<GiftCard> updateGiftCard(GiftCard giftCard)
        {
            EFGiftCard efGiftCard = this.dbContext.GiftCards.Where(giftcard => giftcard.siesa_id == giftCard.siesa_id).First();
            efGiftCard.name = giftCard.name;
            efGiftCard.code = giftCard.code;
            efGiftCard.token = giftCard.token;
            efGiftCard.business = giftCard.business;
            efGiftCard.emision_date = giftCard.emision_date;
            efGiftCard.expire_date = giftCard.expire_date;
            efGiftCard.balance = giftCard.balance;
            efGiftCard.siesa_id = giftCard.siesa_id;

            this.dbContext.SaveChanges();
            return giftCard;
        }

        public async Task<GiftCard[]> updateGiftCards(GiftCard[] giftCards)
        {
            foreach(GiftCard giftCard in giftCards)
            {
                EFGiftCard efGiftCard = this.dbContext.GiftCards.Where(giftcard => giftcard.siesa_id == giftCard.siesa_id).First();
                efGiftCard.name = giftCard.name;
                efGiftCard.code = giftCard.code;
                efGiftCard.token = giftCard.token;
                efGiftCard.business = giftCard.business;
                efGiftCard.emision_date = giftCard.emision_date;
                efGiftCard.expire_date = giftCard.expire_date;
                efGiftCard.balance = giftCard.balance;
                efGiftCard.siesa_id = giftCard.siesa_id;
            }
            this.dbContext.SaveChanges();
            return giftCards;
        }
    }
}
