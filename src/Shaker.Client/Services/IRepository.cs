namespace Shaker.Client.Services; 

public interface IRepository {
    Task<T?> GetData<T>(string jsonName);
    Task UpdateData<T>(string jsonName, T data);
    Task AddDataAsync<T>(string jsonName, T data);
    Task DeleteDataAsync(string jsonName);
}