using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Shared;
using Marmalade.TheGameOfLife.TrafficJam;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Data
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(AppConfig), fileName = "New" + nameof(AppConfig))]
    public class AppConfig : ScriptableObject, IDataManager
    {
        [SerializeField] private CarTargetFollowerConfig carTargetFollowerConfig;
        [SerializeField] private GeneralConfig generalConfig;
        [SerializeField] private TrafficJamConfig trafficJamConfig;
        [SerializeField] private CarConfig carConfig;

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
