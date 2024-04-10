using UnityEngine;
using UnityEditor;

namespace Z3.Paths.UnityEditor
{
    [CustomEditor(typeof(BezierReference))]
    public class BezierReferenceEditor : Editor {

        private BezierReference bezierReference;
        private BezierCurve BezierCurve { get => bezierReference.BezierCurve; set => bezierReference.BezierCurve = value; }

        private void OnEnable() {
            bezierReference = target as BezierReference;
            Undo.undoRedoPerformed += UndoCallback;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
        }

        private void UndoCallback()
        {
            SceneView.RepaintAll();
        }

        public override void OnInspectorGUI()
        {
            EditorUtils.DrawScriptHeader(bezierReference);

            EditorUtils.Separator();

            GUIStyle guiStyle = EditorStyles.boldLabel;
            guiStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Bezier Curve", guiStyle);

            BezierCurve.DrawBezierCurveInspector();

            if (GUI.changed)
                Changes();
        }

        private void OnSceneGUI()
        {

            BezierCurve.DrawBezierCurveScene(bezierReference.transform, Color.green, Color.red);

            if (GUI.changed)
                Changes();
        }

        private void Changes()
        {
            Undo.RecordObject(bezierReference, "Change curve");
        }
    }
}