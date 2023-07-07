using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace MoonToons {
	public class MTSceneToolbar
	{
		private static bool hasInitailized = false;
		private static Rect toolbarRect;
		private static Vector2 toolbarOffset;
		public static int toolbarHeight = 80;
		public static int toolbarWidth = 220;
		public static bool showToolbar = false;

		static MTSceneToolbar()
		{

		}

		[MenuItem("MoonTune Tools/Toggle SceneView Toolbar %m", priority = 0)]
		public static void toggleToolbar()
		{
			showToolbar = !showToolbar;
			SceneView.RepaintAll();
		}

		public static void OnDrawSceneGui(int width, int height)
		{
			if (MTSceneMapEditor.showToolbar) return;

			if (showToolbar)
			{

				// Define the position and size of the toolbar container
				if (toolbarOffset == null || hasInitailized == false)
				{
					toolbarOffset = new Vector2(48, 8);
					hasInitailized = true;
				}
				//Updating the toolbar draw position to where we want to move to
				toolbarRect = new Rect(toolbarOffset.x, toolbarOffset.y, toolbarWidth, toolbarHeight);


				//The hover cursor image
				//EditorGUIUtility.AddCursorRect(
				//	new Rect(toolbarRect.x, toolbarRect.y + (toolbarRect.height / 2) + 4, toolbarRect.width, toolbarRect.height), MouseCursor.MoveArrow); // Move AddCursorRect inside BeginGroup

				// Begin the toolbar container
				GUI.BeginGroup(toolbarRect, GUI.skin.window);
				GUI.backgroundColor = Color.white;
				GUI.contentColor = Color.white;
				GUI.color = Color.white;

				//The two lines at the top of the 
				GUI.Box(new Rect(16, 4, toolbarRect.width - 32 - 6 - 16, 2), "");
				GUI.Box(new Rect(16, 8, toolbarRect.width - 32 - 6 - 16, 2), "");

				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.MiddleCenter;


				// Define the positions and sizes of the three toolbar buttons
				Rect button1Rect = new Rect(10f, 10f, 40f, 30f);
				Rect button2Rect = new Rect(60f, 10f, 40f, 30f);
				Rect button3Rect = new Rect(110f, 10f, 40f, 30f);
				float w = toolbarRect.width - 16;
				float h = toolbarRect.height - 40;

				//(This is for debugging mostly, so i can visualize where the rectarea will be drawn at)
				GUI.DrawTexture(new Rect(8, 32, w, h), EditorGUIUtility.whiteTexture, ScaleMode.ScaleToFit, true, w / h, new Color(0f, 0f, 0f, 0.2f), 0f, 0f);

				GUILayout.BeginArea(new Rect(8 + 4, 32 + 4, w - 8, h - 8));

				GUILayout.BeginHorizontal();

				var customButtonStyle = new GUIStyle(GUI.skin.button);
				customButtonStyle.fixedHeight = 32f;

				//This is the default color that we will set back to once we finish rendering out the menu options
				Color defaultColor = GUI.backgroundColor;

				/*GUI.backgroundColor = (MTSceneCameraController.isEnabled) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
				if(GUILayout.Button("Camera Controls\n(Status: ON)", customButtonStyle))
				{
					MTSceneCameraController.isEnabled = !MTSceneCameraController.isEnabled;
				}*/

				GUI.backgroundColor = (MTSceneMapEditor.showToolbar) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
				if (GUILayout.Button("Map Editor", customButtonStyle))
				{
					MTSceneMapEditor.showToolbar = !MTSceneMapEditor.showToolbar;
					MTSceneMapEditor.isDownInitializedState = 0;
					SceneView.RepaintAll();
				}

				//Resetting back to the default color
				GUI.backgroundColor = defaultColor;

				GUILayout.EndHorizontal();

				GUILayout.EndArea();


				GUIStyle closeButtonStyle = new GUIStyle(GUI.skin.button);
				closeButtonStyle.normal.textColor = Color.red;
				closeButtonStyle.hover.textColor = Color.red;
				closeButtonStyle.active.textColor = Color.red;

				float size = 25.0f;
				Rect closeButtonRect = new Rect(toolbarRect.width - 21 - 12, 4, size, size);

				if (GUI.Button(closeButtonRect, "X", closeButtonStyle))
				{
					// Handle button click
					showToolbar = false;
					SceneView.RepaintAll();
				}


				GUI.Label(new Rect(8, 12, toolbarRect.width - 16 - 6 - 16, 12), "Moon Toons Toolbar", style);
				// End the toolbar container
				GUI.EndGroup();


				// Handle mouse input for dragging the toolbar
				//EditorGUIUtility.AddCursorRect(toolbarRect, MouseCursor.MoveArrow);
				if (toolbarRect.Contains(Event.current.mousePosition)) {
					if (Event.current.type == EventType.MouseDown && toolbarRect.Contains(Event.current.mousePosition))
					{
						GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
						Event.current.Use();
					}
					if (Event.current.type == EventType.MouseDrag && GUIUtility.hotControl == GUIUtility.GetControlID(FocusType.Passive))
					{
						toolbarOffset += Event.current.delta;
						Event.current.Use();
					}
				}
			}
		}
	}
}