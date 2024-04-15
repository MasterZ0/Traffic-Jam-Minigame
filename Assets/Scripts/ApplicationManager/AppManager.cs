using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using Marmalade.TheGameOfLife.Shared;

namespace Marmalade.TheGameOfLife.ApplicationManager
{
    /// <summary> Initialize application </summary>
    public class AppManager : MonoBehaviour
    {
        [Header("App Manager")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private ScriptableObject appConfig;

        private static bool Loading { get; set; } = true;

        private CancellationTokenSource initializationCts = new();

        private void Awake()
        {
            AppConfig.Init((IAppConfigManager)appConfig);
            sceneLoader.Init(this);

            UniTask.Create(async () =>
            {
                SetActiveLoadingScreen(true);

                await sceneLoader.LoadApplication(AppScene.MainMenu);

                SetActiveLoadingScreen(false);

            }).AttachExternalCancellation(initializationCts.Token);
        }

        public static UniTask WaitLoadingEnd() => UniTask.WaitUntil(() => !Loading);

        private void OnDestroy()
        {
            initializationCts.Dispose();
        }

        public void SetActiveLoadingScreen(bool active)
        {
            Loading = active;
            loadingScreen.SetActive(active);
        }
    }
}
