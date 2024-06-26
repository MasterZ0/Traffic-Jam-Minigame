﻿using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.ApplicationManager
{
    /// <summary>
    /// Useful to receive Unity events
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {
        [Header("Scene Changer")]
        [SerializeField] private AppScene scene;

        public void OnChanceScene()
        {
            ServiceLocator.GetService<SceneLoader>().LoadScene(scene);
        }

        public void OnReloadScene()
        {
            ServiceLocator.GetService<SceneLoader>().ReloadScene();
        }
    }
}
