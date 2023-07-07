using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MoonToons.Editor.LevelEditor
{
    [InitializeOnLoad]
    public class MTSceneCameraController
    {
        public static bool isEnabled = false;
		private static double lastUpdateTime;
		private static bool key_down_w = false;
		private static bool key_down_a = false;
		private static bool key_down_s = false;
		private static bool key_down_d = false;
		static MTSceneCameraController()
        {
			lastUpdateTime = EditorApplication.timeSinceStartup;

		}

		public static void OnDrawSceneGui(int pixelWidth, int pixelHeight, SceneView sceneView)
		{



			//Updating the deltaTime
			double deltaTime = EditorApplication.timeSinceStartup - lastUpdateTime;
			lastUpdateTime = EditorApplication.timeSinceStartup;

			if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp)
			{
				if (Event.current.type == EventType.KeyDown)
				{
					switch (Event.current.keyCode)
					{
						case KeyCode.W:
							key_down_w = true;
							Debug.Log("W Pressed");
							break;
						case KeyCode.A:
							key_down_a = true;
							break;
						case KeyCode.S:
							key_down_s = true;
							break;
						case KeyCode.D:
							key_down_d = true;
							break;
					}
				}
				if (Event.current.type == EventType.KeyUp)
				{
					switch (Event.current.keyCode)
					{
						case KeyCode.W:
							key_down_w = false;
							Debug.Log("W Released");
							break;
						case KeyCode.A:
							key_down_a = false;
							break;
						case KeyCode.S:
							key_down_s = false;
							break;
						case KeyCode.D:
							key_down_d = false;
							break;
					}
				}
			}
			else
			{

			}
			if (isEnabled)
            {
                sceneView.Repaint();

				/*if (key_down_w)
				{
					Debug.Log("Translating camera?" + 1 * (float)deltaTime * 100);

					sceneView.camera.transform.position = Vector3.zero;
					//sceneView.camera.transform.Translate(1 * (float)deltaTime * 100, 0, 0);
				}*/

				Camera sceneCamera = SceneView.lastActiveSceneView.camera;
				sceneCamera.transform.position = Vector3.zero;

				Debug.Log(Event.current.type);

				//EditorUtility.SetDirty(null);
			}
			/*if (
					Event.current.type == EventType.KeyUp
				||	Event.current.type == EventType.KeyDown
				||	Event.current.type == EventType.MouseLeaveWindow
				||	Event.current.type == EventType.MouseEnterWindow
				||	Event.current.type == EventType.MouseUp
				||	Event.current.type == EventType.MouseMove
				||	Event.current.type == EventType.MouseDown)
			{
				sceneView.camera.transform.position = Vector3.zero;
				Event.current.Use();
				sceneView.Repaint();
			}*/
		}
	}
}