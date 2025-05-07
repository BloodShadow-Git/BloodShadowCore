using SomeMod;

namespace SampleMod
{
    public class SampleMod
    {
        static void Load()
        {
            Console.WriteLine($"{nameof(SampleMod)} on load method");
            Main();
        }
        static void Main()
        {
            Console.WriteLine($"{nameof(SampleMod)} on ep");
            new Bootstrap().CrossModMethod();
        }
    }
}
