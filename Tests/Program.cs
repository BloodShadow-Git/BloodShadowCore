using BloodShadow.Core.Extensions;
using BloodShadow.Core.HttpRequests;
using BloodShadow.Core.ModSystem;
using R3;
using System.Net;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Tests
{
    public class Program
    {
        static async Task Main()
        {
            //ModManager mgr = new("Mods");
            //mgr.State.Skip(1).Subscribe(_ => Console.WriteLine($"{mgr.Name}: {mgr.State.CurrentValue}"));
            //mgr.Progress.Skip(1).Subscribe(_ => Console.WriteLine($"{mgr.Name}: {mgr.Progress.CurrentValue:P1}"));
            //mgr.Load();
            //Console.WriteLine(mgr.LoadedMods.ArrayToString(input => $"{input.ModData.Header.Name} " +
            //$"v{input.ModData.Header.Version} ({input.ModData.RunFileName}): https://{input.ModData.Header.URL}", true));
            //Console.WriteLine(mgr.Errors.ArrayToString(true));

            //ModHeader SampleModHeader = new()
            //{
            //    Version = new(0, 0, 0, 1)
            //};
            //ModHeader SomeModHeader = new()
            //{
            //    Version = new(0, 0, 0, 1)
            //};

            //const string SomeMod = nameof(SomeMod);
            //const string SampleMod = nameof(SampleMod);

            //new JsonSaveSystem().Save("mod1.json", new ModData()
            //{
            //    Header = new()
            //    {
            //        Version = new(0, 0, 0, 1),
            //        URL = SampleMod
            //    },
            //    RunFileName = SampleMod,
            //    Dependes = [
            //        new()
            //        {
            //            Header = new()
            //            {
            //                Name = SomeMod,
            //                Version = new(0, 0, 0, 1),
            //                URL = SomeMod
            //            },
            //            ModVersionDependesType = ModVersionDependesType.MoreOrEquals
            //        }
            //    ]
            //});
            //new JsonSaveSystem().Save("mod2.json", new ModData()
            //{
            //    Header = new()
            //    {
            //        Version = new(0, 0, 0, 1),
            //        URL = SomeMod,
            //    },
            //    RunFileName = SomeMod,
            //});

            const string URI = "https://github.com/BloodShadow-Git/WifiPasswordGenerator.git";
            const string Path = "./test/file.zip";

            await HttpRequests.Download(URI, Path);

            Console.ReadKey(false);
        }
    }
}