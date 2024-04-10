using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Z3.Paths.UnityEditor
{
    [CustomEditor(typeof(PathReference))]
    public class PathReferenceEditor : Editor {

        private PathReference pathReference;

        #region Properties
        private string SerializedPath { get => pathReference.SerializedPath; set => pathReference.SerializedPath = value; }
        private bool UsingAsset => pathReference.UsingAsset;
        private PathPack PathPack { get => pathReference.PathPack; set => pathReference.PathPack = value; }
        #endregion

        #region Consts
        /// <summary>The amount of subdivision of the bezier curve along t </summary>
        private const int SubdivisionResolution = 1000;

        // Dialog
        private const string Title = "Create Reference";
        private const string Message = "Create a Serialized Reference or a Path Pack?\n\n" +
                    "Serialized Reference is saved with the PathReference and you can use direct scene references within it.\n\n" +
                    "Path Pack is an asset file and can be reused between any Path Reference.\n\n" +
                    "You can convert from one type to the other at any time.";
        private const string Ok = "Path Pack";
        private const string Cancel = "Cancel";
        private const string Alt = "Serialized Reference";

        private const string HelpText = "You needs a Path Pack.\nAssign or Create a new one.";

        // Buttons
        private const string ConvertToAsset = "Convert to asset";
        private const string ConvertToSerialized = "Convert to serialized reference";
        private const string Delete = "Delete serialized reference";
        private const string CreateNew = "Create New";
        #endregion

        private void OnEnable() {
            pathReference = target as PathReference;

            if (!UsingAsset)
            {
                pathReference.SelfDeserialize();
            }

            Undo.undoRedoPerformed += UndoCallback;
        }

        private void OnDisable()
        {

            Undo.undoRedoPerformed -= UndoCallback;
        }

        private void UndoCallback()
        {
            if (!UsingAsset)
                pathReference.SelfDeserialize();

            SceneView.RepaintAll();
        }

        #region Inspector
        public override void OnInspectorGUI() {

            EditorUtils.DrawScriptHeader(pathReference);

            if (PathPack == null)
            {
                DisplayWithoutReference();
                return;
            }

            if (UsingAsset)
            {
                PathPack lastPathPack = PathPack;
                PathPack = EditorGUILayout.ObjectField("Path Pack", PathPack, typeof(PathPack), false) as PathPack;

                if (PathPack != lastPathPack)
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);

                if (PathPack == null) // Asset removed 
                    return;

                if (GUILayout.Button(ConvertToSerialized))
                {
                    Undo.RecordObject(pathReference, ConvertToSerialized);

                    PathAux data = new PathAux(PathPack);
                    SerializedPath = JsonUtility.ToJson(data);
                    pathReference.SelfDeserialize();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(Delete))
                {
                    Undo.RecordObject(pathReference, Delete);

                    SerializedPath = null;
                    PathPack = null;
                    SceneView.RepaintAll();

                    EditorUtility.SetDirty(pathReference);
                    return;
                }

                if (GUILayout.Button(ConvertToAsset))
                {
                    string path = EditorUtility.SaveFilePanelInProject("Create Asset of type " + typeof(PathPack).ToString(), typeof(PathPack).Name + ".asset", "asset", "");
                    
                    if (string.IsNullOrEmpty(path))
                        return;

                    Undo.RecordObject(pathReference, ConvertToAsset);
                    AssetDatabase.CreateAsset(PathPack, path);
                    AssetDatabase.SaveAssets();

                    SerializedPath = null;
                    return;
                }
            }

            EditorUtils.Separator();

            bool guiEnable = !Application.isPlaying;
            GUI.enabled = guiEnable;

            ValidatePathPack();

            GUIStyle guiStyle = EditorStyles.boldLabel;
            guiStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Bezier Path", guiStyle);

            PathPack.DrawBezierPathInspector(guiEnable);

            if (GUI.changed)
            {
                SceneView.RepaintAll();
                Changes();
            }
        }

        public void ValidatePathPack()
        {
            // Minimum Count
            if (PathPack.curves.Count == 0)
            {
                PathPack.curves.Add(new BezierCurve(Vector3.zero));
            }
        }

        private void DisplayWithoutReference()
        {
            EditorGUILayout.HelpBox(HelpText, MessageType.Warning);

            PathPack = EditorGUILayout.ObjectField("Path Pack", pathReference.PathPack, typeof(PathPack), false) as PathPack;

            GUI.enabled = !Application.isPlaying;
            if (GUILayout.Button(CreateNew))
            {
                int value = EditorUtility.DisplayDialogComplex(Title, Message, Ok, Cancel, Alt);

                if (value == 0)
                {
                    NewAsAsset();
                }
                else if (value == 2)
                {
                    NewAsInstance();
                    pathReference.SelfSerialize();
                }
            }
            GUI.enabled = true;
        }
        #endregion

        private void OnSceneGUI()
        {
            if (PathPack == null)
                return;

            if (!Application.isPlaying)
            {
                PathPack.DrawBezierPathHandlesScene(pathReference.transform, Color.red); // TODO: Fix scale 0
                Repaint();
            }

            PathPack.DrawBezierPathCurvesScene(pathReference.transform, Color.cyan);
            PathPack.DrawConstantSpeedPath(pathReference.transform);

            if (GUI.changed)
            {
                Changes();
            }
        }

        private void Changes()
        {
            GeneratePathPoints();

            if (UsingAsset) // Undo + Save
            {
                Undo.RegisterCompleteObjectUndo(PathPack, "Change scriptable");
                EditorUtility.SetDirty(PathPack);
            }
            else
            {
                Undo.RecordObject(pathReference, "Change serialization");
                pathReference.SelfSerialize();
            }
        }

        #region Subdivision points
        /// <summary>
        /// Subdivide the path to find points along transition
        /// </summary>
        private void GeneratePathPoints()
        {
            List<Vector3> subdivisionsPoints = new List<Vector3>();

            foreach (BezierCurve curve in PathPack.curves)
            {
                float transitionSize = 1f / SubdivisionResolution;

                for (int i = 0; i <= SubdivisionResolution; i++)
                {
                    float t = transitionSize * i;
                    subdivisionsPoints.Add(curve.GetTransitionPoint(t));
                }
            }

            CalculateEquidistantPoints(subdivisionsPoints);
        }

        /// <summary>
        /// Filter the subdivisions and generate points with even distance
        /// </summary>
        /// <param name="subdivisionsPoints">Points along the curve with uneven distance</param>
        private void CalculateEquidistantPoints(List<Vector3> subdivisionsPoints)
        {
            List<Vector3> points = new List<Vector3>();
            points.Add(subdivisionsPoints[0]);
            Vector3 previousPoint = subdivisionsPoints[0];

            float spaceBetweenPoint = PathPack.spaceBetweenPoints;
            float distanceLastPoint;

            for (int i = 1; i < subdivisionsPoints.Count - 1; i++)
            {
                distanceLastPoint = Vector3.Distance(previousPoint, subdivisionsPoints[i]);

                while (distanceLastPoint >= spaceBetweenPoint)
                {
                    float overshootDistance = distanceLastPoint - spaceBetweenPoint;
                    Vector3 newEquidistantPoint = subdivisionsPoints[i] + (previousPoint - subdivisionsPoints[i]).normalized * overshootDistance;
                    points.Add(newEquidistantPoint);
                    previousPoint = newEquidistantPoint;
                    distanceLastPoint = Vector3.Distance(previousPoint, subdivisionsPoints[i]);
                }
            }

            float pathLength = 0f;
            for (int i = 1; i < points.Count - 1; i++) // Calculate path lengh
            {
                pathLength += Vector3.Distance(points[i - 1], points[i]);
            }

            PathPack.pathLength = pathLength;
            PathPack.pathPoints = points;
        }
        #endregion

        #region Asset Creator
        public void NewAsInstance()
        {
            PathPack newGraph = (PathPack)CreateInstance(typeof(PathPack));
            Undo.RecordObject(pathReference, "New Instance");
            PathPack = newGraph;
            EditorUtility.SetDirty(pathReference);
        }

        public void NewAsAsset()
        {
            PathPack pathPack = (PathPack)EditorUtils.CreateAssetInProject(typeof(PathPack));
            if (pathPack == null)
                return;

            Undo.RecordObject(pathReference, "New Asset");
            PathPack = pathPack;
            EditorUtility.SetDirty(pathReference);
            EditorUtility.SetDirty(pathPack);
        }
        #endregion
    }
}