using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[InitializeOnLoad]
public class SceneViewCameraControls : Editor
{
	static SceneViewCameraControls()
	{
		SceneView.duringSceneGui += OnBeforeGui;
	}

	private static void OnBeforeGui(SceneView sceneView)
	{
		sceneView.Repaint();

	}
}