namespace colanta_backend.App.Brands.Infraestructure
{
    public class VtexBrandDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Keywords { get; set; }
        public string SiteTitle { get; set; }
        public bool Active { get; set; }
        public bool? MenuHome { get; set; }
        public string AdWordsRemarketingCode { get; set; }
        public string LomadeeCampaignCode { get; set; }
        public int? Score { get; set; }
        public string LinkId { get; set; }

    }
}
