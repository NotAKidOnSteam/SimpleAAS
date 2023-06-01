﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NAK.SimpleAAS
{
    [CustomEditor(typeof(NAKSimpleAASParameters))]
    public class NAKSimpleAASParametersEditor : Editor
    {
        private SerializedProperty _avatar;
        private SerializedProperty _simpleAASParameters;
        private ReorderableList _parameterList;

        private void OnEnable()
        {
            _avatar = serializedObject.FindProperty("avatar");
            _simpleAASParameters = serializedObject.FindProperty("simpleAASParameters");
            InitializeReorderableList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_avatar);
            _parameterList.DoLayoutList();

            if (GUILayout.Button("Sync To Avatar"))
            {
                SyncSettingsToAvatar((NAKSimpleAASParameters)target);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeReorderableList()
        {
            _parameterList = new ReorderableList(serializedObject, _simpleAASParameters, true, true, true, true)
            {
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty element = _parameterList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                },
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Modular Settings")
            };
        }

        private static void SyncSettingsToAvatar(NAKSimpleAASParameters modularSettings)
        {
            modularSettings.avatar.avatarSettings.settings.Clear();
            foreach (var settings in modularSettings.simpleAASParameters)
            {
                foreach (var setting in settings.settings)
                {
                    modularSettings.avatar.avatarSettings.settings.Add(setting);

                    Debug.Log(setting.setting.usedType);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
