using System.Diagnostics;
using BloodShadow.Core.Extensions;
using BloodShadow.Core.Logger;

namespace BloodShadow.Core.SaveSystem.EncryptModule
{
    public abstract class EncryptModule
    {
        protected LoggerLabel Logger;
        public EncryptModule() { Logger = new(GetType().Name); }
        public byte[] Encrypt(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, "Invalid bytes to encrypt", null, null);
                return [];
            }
            try { return EncryptInternal(source); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while encrypting bytes.", new StackTrace(1), exception); }
            return [];
        }
        protected abstract byte[] EncryptInternal(byte[] source);
        public async Task<byte[]> EncryptAsync(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, "Invalid bytes to encrypt", null, null);
                return [];
            }
            try { return await EncryptAsyncInternal(source); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while encrypting bytes.", new StackTrace(1), exception); }
            return [];
        }
        protected abstract Task<byte[]> EncryptAsyncInternal(byte[] source);
        public byte[] Decrypt(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, "Invalid bytes to decrypt", null, null);
                return [];
            }
            try { return DecryptInternal(source); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while decrypting bytes.", new StackTrace(1), exception); }
            return [];
        }
        protected abstract byte[] DecryptInternal(byte[] source);
        public async Task<byte[]> DecryptAsync(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLine(MessageChanel.WARN, "Invalid bytes to decrypt", null, null);
                return [];
            }
            try { return await DecryptAsyncInternal(source); }
            catch (Exception exception) { Logger.WriteLine(MessageChanel.ERROR, $"Exception while decrypting bytes.", new StackTrace(1), exception); }
            return [];
        }
        protected abstract Task<byte[]> DecryptAsyncInternal(byte[] source);
    }
}
