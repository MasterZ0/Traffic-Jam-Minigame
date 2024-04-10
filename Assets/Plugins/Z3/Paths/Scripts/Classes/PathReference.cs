using System;
using System.Collections.Generic;
using UnityEngine;

namespace Z3.Paths {

    /// <summary>
    /// Used to draw curves by editor class and reference the curve in the world space
    /// </summary>
    public class PathReference : MonoBehaviour 
    {
        [SerializeField] private PathPack pathPack;
        [SerializeField] private string serializedPath;

        public PathPack PathPack
        {
            get
            {
                if(pathPack == null)
                    SelfDeserialize();
                
                return pathPack;
            }
            set => pathPack = value;
        }

        #region Editor Propeties
        public string SerializedPath { get => serializedPath; set => serializedPath = value; }
        public bool UsingAsset => string.IsNullOrEmpty(serializedPath);
        #endregion

        public void Awake()
        {
            if (!UsingAsset)
                SelfDeserialize();
        }

        public void SelfDeserialize()
        {
            if (!string.IsNullOrEmpty(serializedPath))
            {
                PathPack = JsonUtility.FromJson<PathAux>(serializedPath);
            }
        }

        public void SelfSerialize()
        {
            PathAux data = new PathAux(pathPack);
            serializedPath = JsonUtility.ToJson(data);
        }
    }

    /// <summary>
    /// Used to auxiliate the serialization of the path pack
    /// </summary>
    [Serializable]
    public struct PathAux 
    {
        public float spaceBetweenPoints;
        public bool loop;
        public List<BezierCurve> curves;
        public List<Vector3> pathPoints;
        public float pathLength;

        public PathAux(PathPack pathPack) // PathPack to PathAux
        {
            loop = pathPack.loop;
            curves = pathPack.curves;
            pathPoints = pathPack.pathPoints;
            pathLength = pathPack.pathLength;
            spaceBetweenPoints = pathPack.spaceBetweenPoints;
        }

        public static implicit operator PathPack(PathAux pathAux) // PathAux to PathPack
        {
            PathPack pathPack = ScriptableObject.CreateInstance<PathPack>();

            pathPack.spaceBetweenPoints = pathAux.spaceBetweenPoints;
            pathPack.curves = pathAux.curves;
            pathPack.pathPoints = pathAux.pathPoints;
            pathPack.pathLength = pathAux.pathLength;
            pathPack.spaceBetweenPoints = pathAux.spaceBetweenPoints;

            return pathPack;
        }
    }
}