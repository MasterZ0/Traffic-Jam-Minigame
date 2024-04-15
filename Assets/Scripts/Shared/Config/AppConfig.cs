using UnityEngine;

namespace Marmalade.TheGameOfLife.Shared
{

    public static class AppConfig
    {
        private static IAppConfigManager dataManager;

        public static void Init(IAppConfigManager instance)
        {
            dataManager = instance;
            dataManager.Init();
        }

        public static T GetData<T>() where T : ScriptableObject
        {
            return dataManager.GetData<T>();
        }
    }
}
