using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;
using Hasbro.TheGameOfLife.Shared;

namespace Hasbro.TheGameOfLife.Editor
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

            AssetDatabase.LoadAssetAtPath<AppConfig>(ProjectPath.AppConfigAsset).Init();
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
