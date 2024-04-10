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

        public bool Loading { get; private set; } = true;

        private CancellationTokenSource initializationCts = new();

        private void Awake()
        {
            Config.Init((IDataManager)appConfig);
            sceneLoader.Init(this);

            UniTask.Create(async () =>
            {
                SetActiveLoadingScreen(true);

                await sceneLoader.LoadApplication(AppScene.MainMenu);

                SetActiveLoadingScreen(false);

            }).AttachExternalCancellation(initializationCts.Token);
        }

        private void OnDestroy()
        {
            initializationCts.Dispose();
        }

        public async UniTask WaitLoadEnd() => await UniTask.WaitUntil(() => !Loading);

        public void SetActiveLoadingScreen(bool active)
        {
            Loading = active;
            loadingScreen.SetActive(active);
        }
    }
}
