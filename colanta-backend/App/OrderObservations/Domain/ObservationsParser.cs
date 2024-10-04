namespace colanta_backend.App.OrderObservations.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ObservationsParser
    {
        private readonly OrderObservationsRepository _repository;

        public ObservationsParser(OrderObservationsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Parsea las observaciones codificadas a observaciones leibles por humanos
        /// </summary>
        /// <param name="observations"></param>
        /// <returns></returns>
        public async Task<string> Parse(string observations){
            //TODO: Para todo el repositorio, averiguar como implementar adecuadamente accessos concurrentes a la base de datos.
            List<ProductObservationField> fields = _repository.GetOrderObservationFields().Result;
            List<ProductCutTypeValue> cutTypes = _repository.GetProductCutTypeValues().Result;

            foreach (ProductObservationField field in fields)
            {
                observations = observations.Replace(field.Code, field.Description);
            }

            foreach (ProductCutTypeValue cutType in cutTypes)
            {
                observations = observations.Replace(cutType.Code, cutType.Description);
            }

            return observations;
        }
    }
}