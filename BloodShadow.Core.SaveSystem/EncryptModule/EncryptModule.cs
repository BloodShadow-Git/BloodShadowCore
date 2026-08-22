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
                Logger.WriteLineWarning("Invalid bytes to encrypt");
                return [];
            }
            try { return EncryptInternal(source); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while encrypting bytes.", exception); }
            return [];
        }
        protected abstract byte[] EncryptInternal(byte[] source);
        public async Task<byte[]> EncryptAsync(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLineWarning("Invalid bytes to encrypt");
                return [];
            }
            try { return await EncryptAsyncInternal(source); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while encrypting bytes.", exception); }
            return [];
        }
        protected abstract Task<byte[]> EncryptAsyncInternal(byte[] source);
        public byte[] Decrypt(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLineWarning("Invalid bytes to decrypt");
                return [];
            }
            try { return DecryptInternal(source); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while decrypting bytes.", exception); }
            return [];
        }
        protected abstract byte[] DecryptInternal(byte[] source);
        public async Task<byte[]> DecryptAsync(byte[] source)
        {
            if (!source.Valid())
            {
                Logger.WriteLineWarning("Invalid bytes to decrypt");
                return [];
            }
            try { return await DecryptAsyncInternal(source); }
            catch (Exception exception) { Logger.WriteLineException($"Exception while decrypting bytes.", exception); }
            return [];
        }
        protected abstract Task<byte[]> DecryptAsyncInternal(byte[] source);
    }
}
