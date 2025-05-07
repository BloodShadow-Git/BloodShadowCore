namespace SomeMod
{
    public class Bootstrap
    {
        static void Load() { Console.WriteLine($"{nameof(Bootstrap)} loaded"); }

        public void CrossModMethod() { Console.WriteLine($"Hello from {nameof(Bootstrap)}"); }
    }
}
