using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Shared;
using Marmalade.TheGameOfLife.TrafficJam;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Config
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(GlobalConfig), fileName = "New" + nameof(GlobalConfig))]
    public class GlobalConfig : ScriptableObject, IAppConfigManager
    {
        [SerializeField] private GeneralConfig generalConfig;
        [SerializeField] private CarTargetFollowerConfig carTargetFollowerConfig;
        [SerializeField] private TrafficJamConfig trafficJamConfig;
        [SerializeField] private CarConfig carConfig;
        [SerializeField] private AiTrafficJamConfig aiTrafficJamConfig;

        private readonly Dictionary<Type, ScriptableObject> dic = new();

        [Button("Update Services")]
        public void Init()
        {
            GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
               .Where(t => typeof(ScriptableObject).IsAssignableFrom(t.FieldType))
               .ToList()
               .ForEach(f =>
               {
                   ScriptableObject service = (ScriptableObject)f.GetValue(this);

                   dic[f.FieldType] = service;
                   ServiceLocator.AddService(service);
               });

            ServiceLocator.AddService(this);
        }

        public T GetData<T>() where T : ScriptableObject
        {
            Type key = typeof(T);
            return (T)dic[key];
        }
    }
}
