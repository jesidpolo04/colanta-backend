namespace colanta_backend.App.Categories.Domain
{
    using System.Collections.Generic;
    public class Category
    {
        public int? id { get; set; }
        public string? siesa_id { get; set; }
        public int? vtex_id { get; set; }
        public string name { get; set; }
        public bool isActive { get; set; }
        public string business { get; set; }
        public Category? father { get; set; }
        public List<Category> childs { get; set; }
        public Category(string name, string? siesa_id = null, int? id = null, int? vtex_id = null, bool isActive = false, string business = "mercolanta")
        {
            this.vtex_id = vtex_id;
            this.id = id;
            this.siesa_id = siesa_id;
            this.name = name;
            this.isActive = isActive;
            this.childs = new List<Category>();
            this.father = null;
            this.business = business;
        } 
        public Category setFather(Category father)
        {
            this.father = father;
            return this;
        }

        public Category addChild(Category newChild)
        {
            foreach(Category child in childs)
            {
                if(child.siesa_id == newChild.siesa_id && child.siesa_id != null)
                {
                    //TODO: buscar manera de notificar o loggear la incidencia
                    return this;
                }
            }
            this.childs.Add(newChild);
            return this;
        }

    }
}
