using UnityEngine;

namespace Marmalade.TheGameOfLife.Shared
{
    /// <summary>
    /// Object that stores all Config
    /// </summary>
    public interface IDataManager
    {
        void Init();
        T GetData<T>() where T : ScriptableObject;
    }

    public static class Config
    {
        private static IDataManager dataManager;

        public static void Init(IDataManager instance)
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
