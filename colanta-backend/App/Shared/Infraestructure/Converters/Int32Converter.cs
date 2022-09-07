namespace colanta_backend.App.Shared.Infraestructure.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public class Int32Converter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.Number)
            {
                decimal number = reader.GetDecimal();
                return Decimal.ToInt32(number);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
