namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using Shared.Domain;
    using System.Threading.Tasks;
    using Shared.Domain;
    using Shared.Application;
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Collections.Generic;
    public class RenderPromotions
    {
        private string processName = "Renderizado de promociones";
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;
        private PromotionsSiesaRepository siesaRepository;
        private IProcess process;
        private ILogger logger;

        private List<Promotion> loadPromotions = new List<Promotion>();
        private List<Promotion> inactivePromotions = new List<Promotion>();
        private List<Promotion> inactivatedPromotions = new List<Promotion>();
        private List<Promotion> failedPromotions = new List<Promotion>();
        private List<Promotion> notProccecedPromotions = new List<Promotion>();
        private List<Promotion> obtainedPromotions = new List<Promotion>();

        private List<Detail> details = new List<Detail>();

        private RenderPromotionsMail renderPromotionsMail;
        private CustomConsole console = new CustomConsole();
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions();

        public RenderPromotions(
                PromotionsRepository localRepository,
                PromotionsVtexRepository vtexRepository,
                PromotionsSiesaRepository siesaRepository,
                IProcess process,
                ILogger logger,
                EmailSender emailSender
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.process = process;
            this.logger = logger;
            this.renderPromotionsMail = new RenderPromotionsMail(emailSender);
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            this.console.processStartsAt(processName, DateTime.Now);
            try
            {
                Promotion[] allSiesaPromotions = await this.siesaRepository.getAllPromotions();
                this.details.Add(new Detail(
                            origin: "siesa",
                            action: "obtener todas las promociones",
                            content: JsonSerializer.Serialize(allSiesaPromotions, this.jsonOptions),
                            description: "petición para obtener las promociones, completada con éxito",
                            success: true));

                Promotion[] deltaPromotions = await this.localRepository.getDeltaPromotions(allSiesaPromotions);
                foreach (Promotion deltaPromotion in deltaPromotions)
                {
                    try
                    {
                        deltaPromotion.is_active = false;
                        await vtexRepository.updatePromotion(deltaPromotion);
                        await localRepository.updatePromotion(deltaPromotion);
                        this.inactivatedPromotions.Add(deltaPromotion);
                        this.details.Add(new Detail(
                                origin: "vtex",
                                action: "actualizar promoción",
                                content: JsonSerializer.Serialize(deltaPromotion, this.jsonOptions),
                                description: "petición para actualizar la promoción, completada con éxito",
                                success: true));
                    }
                    catch(VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.details.Add(new Detail("vtex", vtexException.requestUrl, vtexException.responseBody, vtexException.Message, false));
                        this.logger.writelog(vtexException);
                    }
                }
                foreach (Promotion siesaPromotion in allSiesaPromotions)
                {
                    try
                    {
                        this.obtainedPromotions.Add(siesaPromotion);

                        Promotion? localPromotion = await this.localRepository.getPromotionBySiesaId(siesaPromotion.siesa_id);

                        if (localPromotion != null)
                        {
                            if (localPromotion.is_active == true)
                            {
                                this.notProccecedPromotions.Add(localPromotion);
                            }
                            if (localPromotion.is_active == false && localPromotion.vtex_id !=  null)
                            {
                                this.inactivePromotions.Add(localPromotion);
                                //localPromotion.is_active = true;
                                //Promotion vtexPromotion = await this.vtexRepository.updatePromotion(localPromotion);
                                //localPromotion.is_active = vtexPromotion.is_active;
                                //await this.localRepository.updatePromotion(localPromotion);

                                //this.loadPromotions.Add(localPromotion);
                                //this.details.Add(new Detail(
                                //    origin: "vtex",
                                //    action: "actualizar promoción",
                                //    content: JsonSerializer.Serialize(localPromotion, this.jsonOptions),
                                //    description: "petición para actualizar promoción, completada con éxito",
                                //    success: true));
                            }
                        }

                        if (localPromotion == null)
                        {
                            localPromotion = await this.localRepository.savePromotion(siesaPromotion);
                            Promotion vtexPromotion = await vtexRepository.savePromotion(localPromotion);
                            localPromotion.vtex_id = vtexPromotion.vtex_id;
                            await this.localRepository.updatePromotion(localPromotion);

                            this.loadPromotions.Add(localPromotion);
                            this.details.Add(new Detail(
                                origin: "vtex",
                                action: "crear o actualizar promoción",
                                content: JsonSerializer.Serialize(localPromotion, this.jsonOptions),
                                description: "petición para crear o actualizar promoción, completada con éxito",
                                success: true));
                        }
                    }
                    catch(VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.failedPromotions.Add(siesaPromotion);
                        this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: vtexException.requestUrl,
                                    content: vtexException.responseBody,
                                    description: vtexException.Message,
                                    success: false));
                        this.logger.writelog(vtexException);
                    }
                }
            }
            catch(SiesaException siesaException)
            {
                this.console.throwException(siesaException.Message);
                this.details.Add(new Detail(
                            origin: "siesa",
                            action: siesaException.requestUrl,
                            content: siesaException.requestBody,
                            description: siesaException.Message,
                            success: false));
                this.logger.writelog(siesaException);
            }
            catch(Exception genericException)
            {
                this.console.throwException(genericException.Message);
                this.logger.writelog(genericException);
            }

            this.renderPromotionsMail.sendMail(this.inactivePromotions.ToArray(), this.failedPromotions.ToArray());
            this.process.Log(
                        name: processName,
                        total_loads: this.loadPromotions.Count,
                        total_errors: this.failedPromotions.Count,
                        total_not_procecced: this.notProccecedPromotions.Count,
                        total_obtained: this.obtainedPromotions.Count,
                        json_details: JsonSerializer.Serialize(this.details, this.jsonOptions));
            this.console.processEndstAt(processName, DateTime.Now);
        }
    }
}
