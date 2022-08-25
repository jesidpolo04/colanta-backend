namespace colanta_backend.App.Specifications.Domain
{
    using System.Collections.Generic;
    public class Specification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Value { get; set; }

        public Specification()
        {}
        public Specification(int Id, List<string> Value)
        {
            this.Id = Id;
            this.Value = Value;
        }
        public Specification(int Id, string Name, List<string> Value)
        {
            this.Id = Id;
            this.Name = Name;
            this.Value = Value;
        }
    }
}
