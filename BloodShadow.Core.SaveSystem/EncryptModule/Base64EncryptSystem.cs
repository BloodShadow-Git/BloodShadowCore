using System.Text;

namespace BloodShadow.Core.SaveSystem.EncryptModule
{
    public class Base64EncryptModule : EncryptModule
    {
        protected override Task<byte[]> DecryptAsyncInternal(byte[] source) => Task.Run(() => Convert.FromBase64String(Encoding.UTF8.GetString(source)));
        protected override byte[] DecryptInternal(byte[] source) => Convert.FromBase64String(Encoding.UTF8.GetString(source));
        protected override Task<byte[]> EncryptAsyncInternal(byte[] source) => Task.Run(() => Encoding.UTF8.GetBytes(Convert.ToBase64String(source)));
        protected override byte[] EncryptInternal(byte[] source) => Encoding.UTF8.GetBytes(Convert.ToBase64String(source));
    }
}