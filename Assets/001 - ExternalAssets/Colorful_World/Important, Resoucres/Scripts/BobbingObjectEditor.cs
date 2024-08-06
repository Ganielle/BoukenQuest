using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BobbingObject))]
[CanEditMultipleObjects]
public class BobbingObjectEditor : Editor
{
	SerializedProperty theAmplitude;
	SerializedProperty theSpeed;
	
	public void OnEnable()
	{
		theAmplitude = serializedObject.FindProperty("amplitude");
		theSpeed = serializedObject.FindProperty("speed");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(theAmplitude);
		EditorGUILayout.PropertyField(theSpeed);
		serializedObject.ApplyModifiedProperties();
	}
	
	public void OnSceneGUI()
	{
	}
}

/*
If you want to put 2D GUI objects (GUI, EditorGUI and friends), you need to wrap them in calls to Handles.BeginGUI() and Handles.EndGUI().
*/

