using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
/*
namespace CustomInput
{
    [CustomEditor(typeof(MobileInputButton), true)]
    [CanEditMultipleObjects]
    public class MIBEditor : ButtonEditor
    {
        SerializedProperty keyState;

        protected override void OnEnable()
        {
            base.OnEnable();

            keyState = serializedObject.FindProperty("keyState");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(keyState);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}*/