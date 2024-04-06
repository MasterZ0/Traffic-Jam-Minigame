using Z3.ObjectPooling;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Hasbro.TheGameOfLife.Shared;

namespace Hasbro.TheGameOfLife.ApplicationManager
{
    public enum AppScene
    {
        ApplicationManager,
        MainMenu,
        TrafficJamMinigame
    }

    /// <summary>
    /// Load and unload scenes
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary> Safe variable </summary>
        private bool loadingScene;
        private AppManager appManager;

        internal void Init(AppManager appManager)
        {
            this.appManager = appManager;
            ServiceLocator.AddService(this);
        }

#if UNITY_EDITOR
        private async UniTask ConfigureScene()
        {
            if (SceneManager.sceneCount > 2)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                throw new ApplicationException("AUTOMATION FAIL\nThere are more than two open scenes");
            }

            // Checks if the active scene is not the ApplicationManager
            Scene activeScene = SceneManager.GetActiveScene();
            Scene appManagerScene = SceneManager.GetSceneByBuildIndex(0);

            if (activeScene == appManagerScene)
            {
                Scene firstScene = SceneManager.GetSceneAt(0);
                Scene secondScene = SceneManager.GetSceneAt(1);
                activeScene = firstScene != appManagerScene ? firstScene : secondScene;

                await UniTask.WaitUntil(() => activeScene.isLoaded);
                SceneManager.SetActiveScene(activeScene);
            }
        }
#endif

        internal async UniTask LoadApplication(AppScene startScene)
        {
#if UNITY_EDITOR
            if (SceneManager.sceneCount > 1)
            {
                ConfigureScene();
                return;
            }
#endif
            loadingScene = true;
            await LoadCurrentScene(startScene);
        }

        public async UniTask LoadScene(AppScene gameScene)
        {
            if (loadingScene)
            {
                throw new ApplicationException("There is already a scene loading process in progress");
            }

            await LoadNextScene(gameScene);
        }

        public async UniTask ReloadScene()
        {
            if (loadingScene)
            {
                throw new ApplicationException("There is already a scene loading process in progress");
            }

            // Get current scene
            Scene activeScene = SceneManager.GetActiveScene();
            AppScene currentScene = (AppScene)activeScene.buildIndex;

            await LoadNextScene(currentScene, UnloadSceneOptions.None);
        }

        #region Private Methods

        private async UniTask LoadNextScene(AppScene sceneToLoad, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
        {
            loadingScene = true;

            appManager.SetActiveLoadingScreen(true);
            ObjectPool.ReturnAllToPool();

            // Unload current
            string activeScene = SceneManager.GetActiveScene().name; // Can be a test scene
            AsyncOperation loadSceneAsync = SceneManager.UnloadSceneAsync(activeScene, options);
            await loadSceneAsync;

            GC.Collect();

            // Load next
            await LoadCurrentScene(sceneToLoad);

            appManager.SetActiveLoadingScreen(false);
        }

        private async UniTask LoadCurrentScene(AppScene sceneToLoad)
        {
            // Load Scene
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneToLoad.ToString(), LoadSceneMode.Additive);
            await loadSceneAsync;

            // Set scene active
            Scene activeScene = SceneManager.GetSceneByName(sceneToLoad.ToString());
            SceneManager.SetActiveScene(activeScene);

            // Wait Awake methods
            await UniTask.WaitForEndOfFrame(this);

            loadingScene = false;
        }
        #endregion
    }
}
