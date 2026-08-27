namespace BloodShadow.Core.SaveSystem.SerializeModule
{
    using System.Diagnostics;
    using BloodShadow.Core.Extensions;
    using BloodShadow.Core.Logger;
    public abstract class SerializeModule
    {
        protected LoggerLabel Logger;
        public SerializeModule() { Logger = new(GetType().Name); }
        public byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                Logger.WriteLine(MessageChanel.WARN, "Empty source object", null, null);
                return [];
            }
            byte[] result = [];
            try { result = SerializeInternal(obj); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Serialize exception", new StackTrace(1), exception); }
            finally { if (!result.Valid() && obj != null) { Logger.WriteLine(MessageChanel.WARN, "Empty serialize result but object to serialize is not null", null, null); } }
            return result;
        }
        protected abstract byte[] SerializeInternal(object obj);
        public async Task<byte[]> SerializeAsync(object obj)
        {
            if (obj == null)
            {
                Logger.WriteLine(MessageChanel.WARN, "Empty source object", null, null);
                return [];
            }
            byte[] result = [];
            try { result = await SerializeAsyncInternal(obj); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Serialize exception", new StackTrace(1), exception); }
            finally { if (!result.Valid() && obj != null) { Logger.WriteLine(MessageChanel.WARN, "Empty serialize result but object to serialize is not null", null, null); } }
            return result;
        }
        protected abstract Task<byte[]> SerializeAsyncInternal(object obj);
        public T? Deserialize<T>(byte[] source)
        {
            if (!source.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty source. If it normal you can ignore this warning", null, null); }
            T? result = default;
            try { result = DeserializeInternal<T>(source); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Deserialize exception", new StackTrace(1), exception); }
            finally { if (source.Valid() && result == null) { Logger.WriteLine(MessageChanel.WARN, "Empty deserialize result but source string is not empty", null, null); } }
            return result!;
        }
        protected abstract T DeserializeInternal<T>(byte[] source);
        public async Task<T?> DeserializeAsync<T>(byte[] source)
        {
            if (!source.Valid()) { Logger.WriteLine(MessageChanel.WARN, "Empty source. If it normal you can ignore this warning", null, null); }
            T? result = default;
            try { result = await DeserializeAsyncInternal<T>(source); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, "Deserialize exception", new StackTrace(1), exception); }
            finally { if (source.Valid() && result == null) { Logger.WriteLine(MessageChanel.WARN, "Empty deserialize result but source string is not empty", null, null); } }
            return result!;
        }
        protected abstract Task<T> DeserializeAsyncInternal<T>(byte[] source);
    }
}
