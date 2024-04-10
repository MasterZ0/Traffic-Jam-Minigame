using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;
using Marmalade.TheGameOfLife.Shared;
using Marmalade.TheGameOfLife.Data;

namespace Marmalade.TheGameOfLife.Editor
{
    /// <summary>
    /// Initialize the AppConfig and load the ApplicationManager scene
    /// </summary>
    [InitializeOnLoad]
    public static class Startup 
    {
        static Startup()
        {
            EditorSceneManager.sceneOpened += LoadGameManager;

            AppConfig appConfig = AssetDatabase.LoadAssetAtPath<AppConfig>(ProjectPath.AppConfigAsset);
            Config.Init(appConfig);
        }

        private static void LoadGameManager(Scene scene, OpenSceneMode mode)
        {
            if (mode == OpenSceneMode.Single && scene.buildIndex != 0)
            {
                OpenSceneMode openSceneMode = scene.buildIndex == -1 ? OpenSceneMode.AdditiveWithoutLoading : OpenSceneMode.Additive;
                EditorSceneManager.OpenScene(ProjectPath.ApplicationManagerScene, openSceneMode);
            }
        }
    }
}
