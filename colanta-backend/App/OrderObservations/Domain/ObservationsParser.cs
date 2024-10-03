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
            var fieldsTask = _repository.GetOrderObservationFields();
            var cutTypesTask = _repository.GetProductCutTypeValues();

            await Task.WhenAll(fieldsTask, cutTypesTask);

            List<ProductObservationField> fields = fieldsTask.Result;
            List<ProductCutTypeValue> cutTypes = cutTypesTask.Result;

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