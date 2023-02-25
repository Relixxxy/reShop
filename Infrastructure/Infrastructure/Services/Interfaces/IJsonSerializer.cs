namespace Infrastructure.Services.Interfaces
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data);

        T Deserialize<T>(string value);
    }
}