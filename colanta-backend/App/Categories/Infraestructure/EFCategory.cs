namespace colanta_backend.App.Categories.Infraestructure
{
    using System.Collections.Generic;
    using App.Categories.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EFCategory
    {
        public int? Id { get; set; }
        public string? SiesaId { get; set; }
        public int? VtexId { get; set; }
        public string Name { get; set; }
        public string Business { get; set; }
        public bool IsActive { get; set; }
        public EFCategory Father { get; set; }
        public List<EFCategory> Childs { get; set; }

        public EFCategory()
        {
            Childs = new List<EFCategory>();
        }

        public void SetFromCategory(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Business = category.Business;
            IsActive = category.IsActive;
            SiesaId = category.SiesaId;
            VtexId = category.VtexId;
            
            if(category.Father != null)
            {
                Father = new()
                {
                    Id = category.Father.Id,
                    Name = category.Father.Name,
                    Business = category.Father.Business,
                    IsActive = category.Father.IsActive,
                    SiesaId = category.Father.SiesaId,
                    VtexId = category.Father.VtexId
                };
            }

            if(category.Childs.Count > 0)
            {
                foreach(Category child in category.Childs)
                {
                    EFCategory efChild = new EFCategory();
                    efChild.Id = child.Id;
                    efChild.Name = child.Name;
                    efChild.Business = child.Business;
                    efChild.IsActive = child.IsActive;
                    efChild.SiesaId = child.SiesaId;
                    efChild.VtexId = child.VtexId;
                    efChild.Father = this;
                    this.Childs.Add(efChild);
                }
            }
        }

        public Category GetCategory()
        {
            Category category = new Category(
                    id: this.Id,
                    siesa_id: this.SiesaId,
                    vtex_id: this.VtexId,
                    name: this.Name,
                    business: this.Business,
                    isActive: this.IsActive
                );

            if(this.Father != null)
            {
                Category father = new Category(
                        id: this.Father.Id,
                        siesa_id: this.Father.SiesaId,
                        vtex_id: this.Father.VtexId,
                        name: this.Father.Name,
                        business: this.Father.Business,
                        isActive: this.Father.IsActive
                    );
                category.SetFather(father);
            }
            if(this.Childs.Count > 0)
            {
                foreach(EFCategory efChild in this.Childs)
                {
                    Category child = new Category(
                            id: efChild.Id,
                            siesa_id: efChild.SiesaId,
                            vtex_id: efChild.VtexId,
                            name: efChild.Name,
                            business: efChild.Business,
                            isActive: efChild.IsActive
                        );
                    child.SetFather(category);
                    category.AddChild(child);
                }
            }
            return category;
        }

        public static void BuildCategoryEfModel(EntityTypeBuilder<EFCategory> entityBuilder)
        {
            entityBuilder.ToTable("categories");
            entityBuilder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            entityBuilder.Property(e => e.Name).IsRequired().HasColumnName("name");
            entityBuilder.Property(e => e.SiesaId).HasColumnName("siesa_id");
            entityBuilder.Property(e => e.VtexId).HasColumnName("vtex_id");
            entityBuilder.Property(e => e.IsActive).IsRequired().HasColumnName("is_active");
            //Relations
            entityBuilder.HasMany(e => e.Childs).WithOne(e => e.Father).HasForeignKey("family");
            entityBuilder.HasOne(e => e.Father).WithMany(e => e.Childs);
        }
    }
}
