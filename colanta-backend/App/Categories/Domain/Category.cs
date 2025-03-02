namespace colanta_backend.App.Categories.Domain
{
    using System.Collections.Generic;
    public class Category
    {
        public int? Id { get; set; }
        public string? SiesaId { get; set; }
        public int? VtexId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Business { get; set; }
        public Category? Father { get; set; }
        public List<Category> Childs { get; set; }

        public Category(string name, string? siesa_id = null, int? id = null, int? vtex_id = null, bool isActive = false, string business = "mercolanta")
        {
            VtexId = vtex_id;
            Id = id;
            SiesaId = siesa_id;
            Name = name;
            IsActive = isActive;
            Childs = new List<Category>();
            Father = null;
            Business = business;
        }
        public Category SetFather(Category father)
        {
            Father = father;
            return this;
        }

        public Category AddChild(Category newChild)
        {
            foreach(Category child in Childs)
            {
                if(child.SiesaId == newChild.SiesaId && child.SiesaId is not null)
                {
                    //Omite la inserción de un hijo
                    return this;
                }
            }
            Childs.Add(newChild);
            return this;
        }

    }
}
