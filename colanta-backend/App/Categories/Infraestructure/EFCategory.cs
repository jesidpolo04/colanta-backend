namespace colanta_backend.App.Categories.Infraestructure
{
    using System.Collections.Generic;
    using App.Categories.Domain;
    public class EFCategory
    {
        public int? id { get; set; }
        public string? siesa_id { get; set; }
        public int? vtex_id { get; set; }
        public string name { get; set; }
        public string business { get; set; }
        public bool isActive { get; set; }
        public EFCategory father { get; set; }
        public List<EFCategory> childs { get; set; }

        public EFCategory()
        {
            this.childs = new List<EFCategory>();
        }

        public void setEfCategoryFromCategory(Category category)
        {
            this.id = category.id;
            this.name = category.name;
            this.business = category.business;
            this.isActive = category.isActive;
            this.siesa_id = category.siesa_id;
            this.vtex_id = category.vtex_id;
            
            if(category.father != null)
            {
                EFCategory efFather = new EFCategory();
                efFather.id = category.father.id;
                efFather.name = category.father.name;
                efFather.business = category.father.business;
                efFather.isActive = category.father.isActive;
                efFather.siesa_id = category.father.siesa_id;
                efFather.vtex_id = category.father.vtex_id;

                this.father = efFather;
            }

            if(category.childs.Count > 0)
            {
                foreach(Category child in category.childs)
                {
                    EFCategory efChild = new EFCategory();
                    efChild.id = child.id;
                    efChild.name = child.name;
                    efChild.business = child.business;
                    efChild.isActive = child.isActive;
                    efChild.siesa_id = child.siesa_id;
                    efChild.vtex_id = child.vtex_id;
                    efChild.father = this;
                    this.childs.Add(efChild);
                }
            }
        }

        public Category getCategoryFromEFCategory()
        {
            Category category = new Category(
                    id: this.id,
                    siesa_id: this.siesa_id,
                    vtex_id: this.vtex_id,
                    name: this.name,
                    business: this.business,
                    isActive: this.isActive
                );

            if(this.father != null)
            {
                Category father = new Category(
                        id: this.father.id,
                        siesa_id: this.father.siesa_id,
                        vtex_id: this.father.vtex_id,
                        name: this.father.name,
                        business: this.father.business,
                        isActive: this.father.isActive
                    );
                category.setFather(father);
            }
            if(this.childs.Count > 0)
            {
                foreach(EFCategory efChild in this.childs)
                {
                    Category child = new Category(
                            id: efChild.id,
                            siesa_id: efChild.siesa_id,
                            vtex_id: efChild.vtex_id,
                            name: efChild.name,
                            business: efChild.business,
                            isActive: efChild.isActive
                        );
                    child.setFather(category);
                    category.addChild(child);
                }
            }
            return category;
        }

    }
}
