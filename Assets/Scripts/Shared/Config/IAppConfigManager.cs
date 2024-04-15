using UnityEngine;

namespace Marmalade.TheGameOfLife.Shared
{
    /// <summary>
    /// Object that stores all Config
    /// </summary>
    public interface IAppConfigManager
    {
        void Init();
        T GetData<T>() where T : ScriptableObject;
    }
}
