using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MoonToons.Editor.LevelEditor
{
	[InitializeOnLoad]
	public class MTLevelEditor
	{
		private static Texture2D texButtonBase;
		public static SceneView targetSceneView;


		//This is the initial method for handling loading the editor options on loading the scene
		static MTLevelEditor()
        {
			//Registering button textures
			texButtonBase = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Engine/LevelEditor/Icons/EditorIcon-Base.psd");

			//Registering the actual rendering method for the scene view
			SceneView.duringSceneGui += OnSceneGUI;

		}

		private static void OnSceneGUI(SceneView sceneView)
		{
			targetSceneView = sceneView;

			//Initial test code, to ensure that the system works
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.white;
			style.normal.background = texButtonBase;
			style.onActive.textColor = Color.black;
			style.fontStyle = FontStyle.Bold;
			style.fontSize = 20;
			Vector3 cameraPosition = sceneView.camera.transform.position;
			//Handles.Label(cameraPosition, "Test", style);

			//Here we are drawing the actual GUI
			//First we have to begin drawing the GUI
			Handles.BeginGUI();

			MTSceneMapEditor.OnDrawSceneGui(sceneView.camera.pixelWidth, sceneView.camera.pixelHeight);
			MTSceneToolbar.OnDrawSceneGui(sceneView.camera.pixelWidth, sceneView.camera.pixelHeight);
			//MTSceneCameraController.OnDrawSceneGui(sceneView.camera.pixelWidth, sceneView.camera.pixelHeight, sceneView);

			//Now we start defining the areas we want to draw
			//Area: Bottom Left - Toolbar
			GUILayout.BeginArea(new Rect(5,sceneView.camera.pixelHeight/2, 48, sceneView.camera.pixelHeight/2));
			//GUILayout.Label("Test");
			//GUILayout.Button("test", style, GUILayout.Width(48f), GUILayout.Height(48f));
            GUILayout.EndArea();
			Handles.EndGUI();
		}
	}

}