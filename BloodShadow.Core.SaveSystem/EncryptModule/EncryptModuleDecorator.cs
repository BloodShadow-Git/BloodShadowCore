namespace BloodShadow.Core.SaveSystem.EncryptModule
{
    public sealed class EncryptModuleDecorator(params EncryptModule[] modules) : EncryptModule
    {
        private IEnumerable<EncryptModule> _modules = modules;
        protected override byte[] EncryptInternal(byte[] source) => _modules.Aggregate(source, (cur, module) => module.Encrypt(cur));
        protected override async Task<byte[]> EncryptAsyncInternal(byte[] source)
        {
            byte[] cur = source;
            foreach (EncryptModule encryptModule in _modules) { cur = await encryptModule.EncryptAsync(cur); }
            return cur;
        }
        protected override byte[] DecryptInternal(byte[] source) => _modules.Reverse().Aggregate(source, (cur, module) => module.Decrypt(cur));
        protected override async Task<byte[]> DecryptAsyncInternal(byte[] source)
        {
            byte[] cur = source;
            foreach (EncryptModule encryptModule in _modules.Reverse()) { cur = await encryptModule.DecryptAsync(cur); }
            return cur;
        }
    }
}