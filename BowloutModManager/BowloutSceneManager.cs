using BowloutModManager.BowloutModded;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BowloutModManager
{
    public class BowloutSceneManager
    {
        public BowloutSceneManager() 
        {
            SceneManager.activeSceneChanged += OnSceneLoadedBowloutHook;
        }

        ~BowloutSceneManager()
        {
            SceneManager.activeSceneChanged -= OnSceneLoadedBowloutHook;
        }

        void OnSceneLoadedBowloutHook(Scene previousScene, Scene newScene)
        {
            if (newScene.name == "MainMenu")
                OnMainMenuLoaded(newScene);
        }

        void OnMainMenuLoaded(Scene rootScene)
        {
            SetupMainMenu setupMainMenu = new GameObject("[Bowlout Mod] Setup Main Menu").AddComponent<SetupMainMenu>();
        }
    }
}
