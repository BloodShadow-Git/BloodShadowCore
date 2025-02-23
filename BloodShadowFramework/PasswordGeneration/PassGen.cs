using System.Numerics;

namespace BloodShadowFramework.PasswordGeneration
{
    public static class PassGen
    {
        public static void GeneratePassword(string[] chars, byte length, Action<string> action)
        {
            if (length <= 1) { return; }
            BigInteger resultLength = BigInteger.Pow(chars.Length, length);
            ulong[] numbers = new ulong[length];
            for (byte i = 0; i < numbers.Length; i++) { numbers[i] = 0; }
            for (BigInteger x = 0; x < resultLength; x++)
            {
                string result = "";
                for (byte y = 0; y < length; y++) { result += chars[numbers[y]]; }
                action?.Invoke(result);
                numbers[^1]++;
                for (int y = numbers.Length - 1; y >= 0; y--)
                {
                    if (numbers[y] >= (ulong)chars.Length)
                    {
                        numbers[y] = 0;
                        if (y >= 1) { numbers[y - 1]++; }
                    }
                }
            }
        }
    }
}