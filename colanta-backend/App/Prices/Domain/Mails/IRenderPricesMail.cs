namespace colanta_backend.App.Prices.Domain
{
    using Prices.Domain;
    using System.Collections.Generic;
    public interface IRenderPricesMail
    {
        void sendMail(List<Price> loadedPrices, List<Price> updatedPrices, List<Price> failedPrices);

    }
}
