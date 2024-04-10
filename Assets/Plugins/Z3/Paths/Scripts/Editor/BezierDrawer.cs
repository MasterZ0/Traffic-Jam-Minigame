using UnityEditor;
using UnityEngine;

namespace Z3.Paths.UnityEditor 
{   
    /// <summary>
    /// Note to developers: Please describe what this class does.
    /// </summary>
    public static class BezierDrawer 
    { 
        private static GUILayoutOption[] ButtonStyle => new GUILayoutOption[] { GUILayout.MaxWidth(120) };
        
        private const string SpaceBetweenPoints = "Space Between Points";
        private const string Loop = "Loop";
        private const string AddCurve = "Add Curve";
        private const string DeleteCurve = "Delete Curve";

        private const string StartPosition = "Start Position";
        private const string StartTangent = "Start Tangent";
        private const string EndTangent = "End Tangent";
        private const string EndPosition = "End Position";

        private const float MinSlider = 0.02f;
        private const float MaxSlider = 2f;
        
        private const float DefaultBezierWidth = 1f;
        private const float DefaultPointsRadius = 0.02f;

        public static void DrawBezierPathInspector(this PathPack pathPack, bool guiEnabled) {

            // Draw title and loop toggle
            GUILayout.BeginVertical("Box");
            {
                pathPack.spaceBetweenPoints = EditorGUILayout.Slider(SpaceBetweenPoints, pathPack.spaceBetweenPoints, MinSlider, MaxSlider);
                pathPack.loop = EditorGUILayout.Toggle(Loop, pathPack.loop);

                Vector3 lastEnd = pathPack.curves[0].endPosition;
                for (int i = 0; i < pathPack.curves.Count; i++)
                {
                    BezierCurve curve = pathPack.curves[i];

                    // Draw Buttons
                    EditorGUILayout.Space(10);
                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                    if (GUILayout.Button(AddCurve, ButtonStyle))
                    {

                        if (i == pathPack.curves.Count - 1)
                        {
                            BezierCurve newCurve = new BezierCurve(lastEnd);
                            pathPack.curves.Add(newCurve);
                        }
                        else
                        {
                            BezierCurve newCurve = new BezierCurve()
                            {
                                startPosition = curve.endPosition,
                                startTangent = curve.endPosition,
                                endTangent = curve.endPosition,
                                endPosition = curve.endPosition,
                            };

                            pathPack.curves.Insert(i + 1, newCurve);
                        }

                        return;
                    }

                    GUI.enabled = guiEnabled && pathPack.curves.Count > 1;
                    if (GUILayout.Button(DeleteCurve, ButtonStyle))
                    {
                        pathPack.curves.Remove(curve);
                        return;
                    }
                    GUI.enabled = guiEnabled;

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(10);

                    // First point is zero or last
                    GUI.enabled = false;
                    if (i == 0)
                    {
                        curve.startPosition = EditorGUILayout.Vector3Field(StartPosition, Vector3.zero);
                    }
                    else
                    {
                        curve.startPosition = EditorGUILayout.Vector3Field(StartPosition, lastEnd);
                    }
                    GUI.enabled = guiEnabled;

                    curve.startTangent = EditorGUILayout.Vector3Field(StartTangent, curve.startTangent);
                    curve.endTangent = EditorGUILayout.Vector3Field(EndTangent, curve.endTangent);

                    // Last point and is loop
                    if (i == pathPack.curves.Count - 1 && pathPack.loop)
                    {
                        GUI.enabled = false;
                        curve.endPosition = EditorGUILayout.Vector3Field(EndPosition, pathPack.curves[0].startPosition);
                        GUI.enabled = guiEnabled;
                    }
                    else
                    {
                        curve.endPosition = EditorGUILayout.Vector3Field(EndPosition, curve.endPosition);
                        lastEnd = curve.endPosition;
                    }

                    if (i < pathPack.curves.Count - 1)
                    {
                        EditorGUILayout.Space(10);
                    }
                }
            }
            

            GUILayout.EndVertical();
        }

        public static void DrawBezierPathHandlesScene(this PathPack bezierPath, Transform from, Color lineColor)
        {
            bool shift = Event.current.shift;

            Vector3 lastEnd = Vector3.zero;
            for (int i = 0; i < bezierPath.curves.Count; i++)
            {
                BezierCurve curve = bezierPath.curves[i];

                Vector3 startPosition;
                Vector3 startTangent = from.TransformPoint(curve.startTangent);
                Vector3 endTangent = from.TransformPoint(curve.endTangent);
                Vector3 endPosition = from.TransformPoint(curve.endPosition);
                if (i == 0)
                {
                    startPosition = from.TransformPoint(curve.startPosition);
                }
                else
                {
                    startPosition = lastEnd;
                    curve.startPosition = from.InverseTransformPoint(startPosition);
                }

                if (!shift)
                {
                    startTangent = Handles.PositionHandle(startTangent, Quaternion.identity);
                    endTangent = Handles.PositionHandle(endTangent, Quaternion.identity);

                }

                if (curve.startTangent != from.InverseTransformPoint(startTangent))
                {
                    curve.startTangent = from.InverseTransformPoint(startTangent);
                }
                if (curve.endTangent != from.InverseTransformPoint(endTangent))
                {
                    curve.endTangent = from.InverseTransformPoint(endTangent);
                }

                if (i < bezierPath.curves.Count - 1 || !bezierPath.loop)
                {
                    Vector3 oldEndPosition = endPosition;
                    endPosition = Handles.PositionHandle(endPosition, Quaternion.identity);

                    if (curve.endPosition != from.InverseTransformPoint(endPosition))
                    {
                        curve.endPosition = from.InverseTransformPoint(endPosition);

                        if (shift) // updates nearby tangents 
                        {
                            Vector3 difference = endPosition - oldEndPosition;
                            curve.endTangent = bezierPath.curves[i].endTangent + difference;

                            if (i < bezierPath.curves.Count - 1)
                            {
                                BezierCurve nextCurve = bezierPath.curves[i + 1];
                                nextCurve.startTangent = nextCurve.startTangent + difference;
                            }
                        }
                    }
                }

                Handles.color = lineColor;
                if (!shift)
                {
                    Handles.DrawLine(startPosition, startTangent);
                    Handles.DrawLine(endPosition, endTangent);

                }

                lastEnd = endPosition;
            }
        }

        public static void DrawBezierCurveInspector(this BezierCurve bezierCurve)
        {
            GUILayout.BeginVertical("Box");
            {
                GUI.enabled = false;
                bezierCurve.startPosition = EditorGUILayout.Vector3Field(StartPosition, Vector3.zero);
                GUI.enabled = true;
                bezierCurve.startTangent = EditorGUILayout.Vector3Field(StartTangent, bezierCurve.startTangent);
                bezierCurve.endTangent = EditorGUILayout.Vector3Field(EndTangent, bezierCurve.endTangent);
                bezierCurve.endPosition = EditorGUILayout.Vector3Field(EndPosition, bezierCurve.endPosition);
            }
            GUILayout.EndVertical();
        }
        
        public static void DrawBezierCurveScene(this BezierCurve bezierCurve, Transform from, Color curveColor, Color lineColor, float lineWidth = DefaultBezierWidth)
        {
            Vector3 worldStartPosition = from.TransformPoint(bezierCurve.startPosition);
            Vector3 worldStartTangent = from.TransformPoint(bezierCurve.startTangent);
            Vector3 worldEndTangent = from.TransformPoint(bezierCurve.endTangent);
            Vector3 worldEndPosition = from.TransformPoint(bezierCurve.endPosition);

            // Handles
            worldStartTangent = Handles.PositionHandle(worldStartTangent, Quaternion.identity);
            worldEndTangent = Handles.PositionHandle(worldEndTangent, Quaternion.identity);
            worldEndPosition = Handles.PositionHandle(worldEndPosition, Quaternion.identity);

            // Check if handles changed
            Vector3 startTangent = from.InverseTransformPoint(worldStartTangent);
            if (startTangent != bezierCurve.startTangent)
            {
                bezierCurve.startTangent = startTangent;
            }

            Vector3 endTangent = from.InverseTransformPoint(worldEndTangent);
            if (endTangent != bezierCurve.endTangent)
            {
                bezierCurve.endTangent = endTangent;
            }

            Vector3 endPosition = from.InverseTransformPoint(worldEndPosition);
            if (endPosition != bezierCurve.endPosition)
            {
                bezierCurve.endPosition = endPosition;
            }

            // Bezier Curve
            Handles.color = lineColor;
            Handles.DrawLine(worldStartPosition, worldStartTangent);
            Handles.DrawLine(worldEndPosition, worldEndTangent);

            Handles.DrawBezier(worldStartPosition, worldEndPosition, worldStartTangent, worldEndTangent, curveColor, Texture2D.normalTexture, lineWidth);
        }

        public static void DrawConstantSpeedPath(this PathPack pathPack, Transform from, float pointRadius = DefaultPointsRadius)
        {
            Handles.color = Color.cyan;
            foreach (Vector3 point in pathPack.pathPoints)
            {
                Handles.DrawWireDisc(from.TransformPoint(point), SceneView.lastActiveSceneView.rotation * Vector3.forward, pointRadius);
            }
        }

        public static void DrawBezierPathCurvesScene(this PathPack bezierPath, Transform from, Color curveColor, float bezierWidth = DefaultBezierWidth)
        {
            for (int i = 0; i < bezierPath.curves.Count; i++)
            {
                BezierCurve curve = bezierPath.curves[i];

                Vector3 startPosition = from.TransformPoint(curve.startPosition);
                Vector3 startTangent = from.TransformPoint(curve.startTangent);
                Vector3 endTangent = from.TransformPoint(curve.endTangent);
                Vector3 endPosition = from.TransformPoint(curve.endPosition);

                Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, curveColor, Texture2D.whiteTexture, bezierWidth);
            }
        }
    }
}