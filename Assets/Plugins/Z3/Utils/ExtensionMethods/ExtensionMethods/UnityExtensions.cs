using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Z3.Utils.ExtensionMethods
{
    /// <summary>
    /// General Extensions
    /// </summary>
    public static class UnityExtensions
    {
        public static T CloneT<T>(this T node) where T : Object
        {
            return Object.Instantiate(node);
        }

        public static bool ObjectNullCheck(this object target)
        {
            return target == null || (target is Object obj && !obj);
        }

        public static void DoActionNextFrame(this MonoBehaviour monoBehaviour, Action action) // Prevents Event system bugs
        {
            monoBehaviour.StartCoroutine(CallNextFrame(action));
        }

        /// <summary> Prevents Event system bugs </summary>
        public static void SelectWithDelay(this Selectable selectable)
        {
            selectable.StartCoroutine(CallNextFrame(selectable.Select));
        }

        /// <summary> Prevents Event system bugs </summary>
        public static void SelectWithDelay(this MonoBehaviour monoBehaviour, GameObject gameObject)
        {
            monoBehaviour.StartCoroutine(CallNextFrame(() => EventSystem.current.SetSelectedGameObject(gameObject)));
        }

        private static IEnumerator CallNextFrame(Action action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }

        public static int ToIntLayer(this LayerMask layer) => (int)Mathf.Log(layer.value, 2f);
    }
}