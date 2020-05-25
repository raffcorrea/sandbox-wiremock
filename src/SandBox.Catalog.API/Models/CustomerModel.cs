namespace SandBox.Catalog.API.Models
{
    public class CustomerModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Cep { get; set; }
        public CepModel CepDetails { get; set; }
    }
}
