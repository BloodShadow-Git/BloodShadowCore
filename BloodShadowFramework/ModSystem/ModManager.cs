namespace BloodShadowFramework.ModSystem
{
    public class ModManager
    {
        // To every field require "has override" property

        // Ticked Update abstract     => done ===> cancel but abstracts has been stayed
        // Cutscene Manager abstract  => done ===> cancel but abstracts has been stayed
        // UI Controller abstract     => done ===> cancel but abstracts has been stayed
        // Scene Switcher abstract    => done ===> cancel but abstracts has been stayed
        // Bridge Builder abstract    => done ===> cancel but abstracts has been stayed

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

        public override string ToString() => "Project Apollon Mod Loader (PAML)";
    }
}
