namespace SocialNetwork.Cache;

public interface ICacheService
{
    /// <summary>
    /// Генерация уникального ключа для сохранения в хранилище
    /// </summary>
    string KeyGenerate();

    /// <summary>
    /// Положить данные по ключу в хранилище
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="data">Данные</param>
    /// <param name="storeTime">Время которое данные будут хранится</param>
    Task<bool> Put<T>(string key, T data, TimeSpan? storeTime = null);

    /// <summary>
    /// Установить время хранения для данных
    /// </summary>
    Task SetStoreTime(string key, TimeSpan? storeTime = null);

    /// <summary>
    /// Получить данные из кеша по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="resetLifeTime">Нужно ли обновить время хранения</param>
    Task<T> Get<T>(string key, bool resetLifeTime = true);

    /// <summary>
    /// Удалить данные из кеша по ключу
    /// </summary>
    Task<bool> Delete(string key);
}