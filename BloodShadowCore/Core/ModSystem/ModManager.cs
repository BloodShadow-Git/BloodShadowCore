namespace BloodShadow.Core.ModSystem
{
    using BloodShadow.Core.OrGraph;
    using R3;
    using SaveSystem;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class ModManager
    {
        // To every field require "has override" property

        // Ticked Update abstract     => done ===> cancel but abstracts has been stayed
        // Cutscene Manager abstract  => done ===> cancel but abstracts has been stayed
        // UI Controller abstract     => done ===> cancel but abstracts has been stayed
        // Scene Switcher abstract    => done ===> cancel but abstracts has been stayed

        // Main Menu                  => n
        // Game                       => n

        // Boot UI Set                => n
        // Cutscene UI Set            => n
        // Boot UI Origin             => n

        // Mod load queue:

        // 1. Check all mods for dependens
        // 2. Load all not exisiting mods from dependes url
        // 3. Load assemblies
        // 4. Load asset bundles
        // 5. Register scenes
        // 6. Load and/or register items to managers
        // 7. Start mod entrypoint (abstracts)

        // To find

        // items,
        // dungeons,
        // enemies,
        // achivments,
        // etc.

        // Planned use stringed ID
        // ID type: "modName:itemID" where game is base "base:" exsample "base:cheapSword"
        // This system requiers databases for each type like:

        // 1. Items (abstracts)                                                                         ====> created by manager
        // 2. Dungeons (scene to load - dungeon data (enemies, rewards, allow MP, dungeon settings))    ====> created by manager
        // 3. Enemies                                                                                   ====> created by manager
        // 4. Achivments                                                                                ====> created by manager
        // 5. Quests                                                                                    ====> created by manager
        // 6. Events (inherited from quests)                                                            ====> created by manager
        // 7. Characters                                                                                ====> created by manager
        // 8. Vehicles                                                                                  ====> created by manager
        // 9. GameObject                                                                                ====> created by manager

        // And save pair ID - state or count


        // Mod header structure

        // Name
        // Description
        // Version
        // Dependeds
        // Localizations
        // Default localization

        // Main menu
        // Game
        // Scripts (["someCode.dll"])
        // Asset bundles (["someAssetBundle"])
        // Mod scenes ("SceneName": "Scene/path/scene.unity")
        // Items asset bundles ([someItemAssetBundle])
        // Enemies asset bundles ([someEnemyAssetBundle])

        public ReadOnlyReactiveProperty<string> State => _state;
        private readonly ReactiveProperty<string> _state;

        public ReadOnlyReactiveProperty<float> Progress => _progress;
        private readonly ReactiveProperty<float> _progress;

        public IEnumerable<IReadOnlyLoadedModData> LoadedMods => _loadedMods;
        protected readonly List<LoadedModData> _loadedMods;

        public IEnumerable<string> Errors => _errors;
        protected readonly List<string> _errors;

        public IReadOnlyOrGraph<IReadOnlyModHeader> Mods => _mods;
        protected readonly OrGraph<ModHeader> _mods;

        private readonly Dictionary<ModHeader, LoadedModData> _headerPairs;

        protected const string NOHEADER = $"_{nameof(NOHEADER)}";
        protected const string INVALIDHEADER = $"_{nameof(INVALIDHEADER)}";
        protected const string NORUNFILE = $"_{nameof(NORUNFILE)}";
        protected const string CYCLEDEPENDS = $"_{nameof(CYCLEDEPENDS)}";

        protected const string LOADINGMODHEADERS = "Loading mod headers";
        protected const string BUILDINGDEPENDES = "Building dependes";
        protected const string CHECKINGFORMISSINGMODS = "Checking for missing mods";
        protected const string DOWNLOADINGMISSINGMODS = "Downloading missing mods";
        protected const string RUNNINGMODSENTRYPOINT = "Running mods entrypoint";

        protected string _modPath;
        protected SaveSystem _saveSystem;
        protected BindingFlags _flags;
        protected string _loadMethodName;

        protected const BindingFlags DEFAULTBINDIONGFLAGS = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        protected const string DEFAULTLOADMETHODNAME = "Load";
        protected static readonly SaveSystem DEFAULTSAVESYSTEM = new JsonSaveSystem();

        public ModManager(string modPath) : this(modPath, DEFAULTSAVESYSTEM, DEFAULTBINDIONGFLAGS, DEFAULTLOADMETHODNAME) { }
        public ModManager(string modPath, SaveSystem saveSystem) : this(modPath, saveSystem, DEFAULTBINDIONGFLAGS, DEFAULTLOADMETHODNAME) { }
        public ModManager(string modPath, SaveSystem saveSystem, string loadMethodName) : this(modPath, saveSystem, DEFAULTBINDIONGFLAGS, loadMethodName) { }
        public ModManager(string modPath, BindingFlags flags) : this(modPath, DEFAULTSAVESYSTEM, flags, DEFAULTLOADMETHODNAME) { }
        public ModManager(string modPath, BindingFlags flags, string loadMethodName) : this(modPath, DEFAULTSAVESYSTEM, flags, loadMethodName) { }
        public ModManager(string modPath, SaveSystem saveSystem, BindingFlags flags) : this(modPath, saveSystem, flags, DEFAULTLOADMETHODNAME) { }

        public ModManager(string modPath, SaveSystem saveSystem, BindingFlags flags, string loadMethodName)
        {
            _loadedMods = [];
            _errors = [];
            _mods = new();
            _modPath = modPath;
            _saveSystem = saveSystem;
            _flags = flags;
            _loadMethodName = loadMethodName;
            _headerPairs = [];
            _state = new();
            _progress = new();
            _saveSystem.CheckDirectory(_modPath);
        }

        public virtual void Load()
        {
            LoadModData();
            if (_errors.Count > 0) { return; }
            BuildDepends();
            LoadMods();
        }

        protected void LoadModData()
        {
            _state.Value = LOADINGMODHEADERS;
            _progress.Value = 0;
            _loadedMods.Clear();
            _errors.Clear();
            _headerPairs.Clear();

            List<Task> tasks = [];
            IEnumerable<string> filesToCheck = Directory.EnumerateDirectories(_modPath);
            int filesCount = filesToCheck.Count();
            for (int i = 0; i < filesCount; i++)
            {
                _progress.Value = (float)i / filesCount;

                string directory = filesToCheck.ElementAt(i);
                if (directory.Contains(NOHEADER) || directory.Contains(INVALIDHEADER) || directory.Contains(NORUNFILE) || directory.Contains(CYCLEDEPENDS)) { continue; }
                FileInfo modHeaderFile = new(Path.Combine(directory, "mod.json"));
                if (!modHeaderFile.Exists) { Rename(directory, NOHEADER); continue; }
                tasks.Add(_saveSystem.LoadAsync<ModData>(modHeaderFile.FullName, data =>
                {
                    if (data == null) { Rename(directory, INVALIDHEADER); return; }
                    FileInfo modRunData = new(data.RunFileName);
                    if (string.IsNullOrEmpty(modRunData.Extension))
                    {
                        string loadFile = Directory.EnumerateFiles(directory, $"{data.RunFileName}.*").FirstOrDefault();
                        if (loadFile == null) { Rename(directory, NORUNFILE); return; }
                        modRunData = new(loadFile);
                        data.RunFileName = modRunData.FullName;
                    }
                    if (string.IsNullOrEmpty(data.RunFileName) || !modRunData.Exists) { Rename(directory, NORUNFILE); return; }
                    if (string.IsNullOrEmpty(data.Header.Name)) { data.Header.Name = Path.GetFileNameWithoutExtension(data.RunFileName); }
                    LoadedModData lmd = new(data, Path.GetRelativePath(Directory.GetCurrentDirectory(), directory));
                    _loadedMods.Add(lmd);
                    _headerPairs.Add(lmd.ModData.Header, lmd);
                }, false, false));
            }
            Task.WaitAll([.. tasks]);

            _state.Value = CHECKINGFORMISSINGMODS;
            _progress.Value = 0;
            List<ModHeader> toLoad = [];
            IEnumerable<LoadedModData> modsToCheck = from search in _loadedMods
                                                     where search.ModData.Dependes != null
                                                     select search;
            int dependesSum = modsToCheck.Sum(input => input.ModData.Dependes.Length);
            for (int x = 0; x < modsToCheck.Count(); x++)
            {
                LoadedModData lmd = modsToCheck.ElementAt(x);
                for (int y = 0; y < lmd.ModData.Dependes.Length; y++)
                {
                    _progress.Value = (float)((x * modsToCheck.Count()) + y) / dependesSum;
                    if (!_headerPairs.ContainsKey(lmd.ModData.Dependes[y].Header))
                    {
                        if (lmd.ModData.Dependes[y].Header.URL == null)
                        {
                            _errors.Add($"No URL for mod: {lmd.ModData.Header.Name} v.{lmd.ModData.Header.Version}");
                            continue;
                        }
                        toLoad.Add(lmd.ModData.Dependes[y].Header);
                    }
                }
            }


        }

        protected void BuildDepends()
        {
            _state.Value = BUILDINGDEPENDES;
            List<(ModHeader item, ModHeader parent)> pairs = [];
            foreach (LoadedModData loadedMod in _loadedMods)
            {
                if (loadedMod.ModData.Dependes == null || loadedMod.ModData.Dependes.Length == 0) { pairs.Add((loadedMod.ModData.Header, null)); continue; }
                bool addToGraph = true;
                List<(ModHeader item, ModHeader parent)> localPairs = new(loadedMod.ModData.Dependes.Length);
                foreach (ModDependes item in loadedMod.ModData.Dependes)
                {
                    if (!_headerPairs.ContainsKey(item.Header)) { }
                    localPairs.Add((loadedMod.ModData.Header, item.Header));
                }
                if (addToGraph) { pairs.AddRange(localPairs); }
            }
        }

        protected void LoadMods()
        {
            _state.Value = RUNNINGMODSENTRYPOINT;
            for (int i = 0; i < _mods.GetDeep(); i++)
            {
                List<Task> tasks = [];
                foreach (ModHeader mh in _mods.GetItems(i))
                {
                    LoadedModData lmd = _headerPairs[mh];
                    if (!_mods.EndElements.Contains(mh)) { tasks.Add(Load(lmd)); }
                    else { Load(lmd); }
                }
                Task.WaitAll([.. tasks]);
            }
        }

        protected virtual Task Load(LoadedModData modData)
        {

            return Task.CompletedTask;
        }

        protected static void Rename(string directory, string message) { if (!directory.Contains(message)) { Directory.Move(directory, $"{directory}{message}"); } }

        public virtual string Name { get; } = "BSML";
        public virtual string FullName { get; } = "Blood Shadow Mod Loader";

        public override string ToString() => $"{FullName} ({Name})";
    }
}
