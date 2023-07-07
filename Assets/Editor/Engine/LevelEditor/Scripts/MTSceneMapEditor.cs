using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using NUnit.Framework;
//using MoonToons.Engine;

namespace MoonToons
{
	[InitializeOnLoad]
	public class MTSceneMapEditor
	{
		public enum Tools
		{
			NONE=1,
			CARVE=2,
			FILL=3,
			TILES=4,
			PROPS=5
		}

		//Initialization variables
		private static bool hasInitialized = false;
		private static Rect toolbarRect;
		private static Vector2 toolbarOffset;

		//Toolbar configuration
		public static int toolbarHeight = 75*3;
		public static int toolbarWidth = 160;
		public static bool showToolbar = false;

		//These are the variables for showing the tools and utilities
		public static bool showOptionsTools = true;
		public static bool showOptionsUtilities = false;
		public static bool generateChunksInEditor = false;

		//This is used to switch between selected tools
		public static Tools selectedTool = Tools.NONE;

		//Key references
		public static bool isDownUp = false;
		public static bool isDownDown = false;
		public static bool isDownLeft = false;
		public static bool isDownRight = false;
		public static int isDownInitializedState = 0;
		private static double previousTimeDelta;
		private static Vector3 cameraPos = new Vector3();

		//For selecting a prefab
		private static string selectedPrefabGUID = "-1";

		static MTSceneMapEditor()
		{
			SceneView.duringSceneGui += OnKeyPressCheck;
		}

		private static void OnKeyPressCheck(SceneView sceneView)
		{

			if (showToolbar)
			{
				if (Event.current.type == EventType.KeyDown)
				{
					switch (Event.current.keyCode)
					{
						case KeyCode.W:
							Event.current.Use();
							isDownUp = true;
							previousTimeDelta = EditorApplication.timeSinceStartup;
							break;
						case KeyCode.A:
							Event.current.Use();
							isDownLeft = true;
							previousTimeDelta = EditorApplication.timeSinceStartup;
							break;
						case KeyCode.S:
							Event.current.Use();
							isDownDown = true;
							previousTimeDelta = EditorApplication.timeSinceStartup;
							break;
						case KeyCode.D:
							Event.current.Use();
							isDownRight = true;
							previousTimeDelta = EditorApplication.timeSinceStartup;
							break;
					}
				}
				else if (Event.current.type == EventType.KeyUp)
				{
					switch (Event.current.keyCode)
					{
						case KeyCode.W:
							Event.current.Use();
							isDownUp = false;
							break;
						case KeyCode.A:
							Event.current.Use();
							isDownLeft = false;
							break;
						case KeyCode.S:
							Event.current.Use();
							isDownDown = false;
							break;
						case KeyCode.D:
							Event.current.Use();
							isDownRight = false;
							break;
					}
				}
				else
				{
					if(isDownUp || isDownRight || isDownLeft ||  isDownDown || isDownInitializedState<20)
					{
						double currentTime = EditorApplication.timeSinceStartup;
						float deltaTime = (float)(currentTime - previousTimeDelta);
						previousTimeDelta = EditorApplication.timeSinceStartup;

						//This gets the sceneview camera's distance to its pivot point
						Camera sceneCamera = SceneView.lastActiveSceneView.camera;
						Vector3 pivot = SceneView.lastActiveSceneView.pivot;
						float zoomDistance = Vector3.Distance(pivot, sceneCamera.transform.position);

						//And now we are calculating the speed with the zoom distance to the pivot point in mind
						float cameraSpeed = 1f * zoomDistance;

						if (isDownUp) { cameraPos.z += 3f * deltaTime * cameraSpeed; }
						if (isDownDown) { cameraPos.z -= 3f * deltaTime * cameraSpeed; }
						if (isDownLeft) { cameraPos.x -= 3f * deltaTime * cameraSpeed; }
						if (isDownRight) { cameraPos.x += 3f * deltaTime * cameraSpeed; }
						SceneView.lastActiveSceneView.pivot = cameraPos;
						SceneView.lastActiveSceneView.rotation = Quaternion.AngleAxis(-45f, Vector3.left);
						isDownInitializedState += 1;

						SceneView.RepaintAll();
					}
				}
			}
		}

		public static void OnDrawSceneGui(int width, int height)
		{
			if (showToolbar)
			{
				bool canCancelMouseEvents = false;
				// Define the position and size of the toolbar container
				if (toolbarOffset == null || hasInitialized == false)
				{
					toolbarOffset = new Vector2(8, height - toolbarHeight - 8);
					hasInitialized = true;
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
				Rect button1Rect = new Rect(10f, 10f, 32f, 30f);
				Rect button2Rect = new Rect(60f, 10f, 32f, 30f);
				Rect button3Rect = new Rect(110f, 10f, 32f, 30f);
				float w = toolbarRect.width - 16;
				float h = toolbarRect.height - 40;

				//(This is for debugging mostly, so i can visualize where the rectarea will be drawn at)
				GUI.DrawTexture(new Rect(8, 32, w, h), EditorGUIUtility.whiteTexture, ScaleMode.ScaleToFit, true, w / h, new Color(0f, 0f, 0f, 0.2f), 0f, 0f);

				GUILayout.BeginArea(new Rect(8 + 4, 32 + 4, w - 8, h - 8));

				//GUILayout.BeginHorizontal();

				var customButtonStyle = new GUIStyle(GUI.skin.button);
				customButtonStyle.fixedHeight = 20f;

				//This is the default color that we will set back to once we finish rendering out the menu options
				Color defaultColor = GUI.backgroundColor;

				/*GUI.backgroundColor = (MTSceneCameraController.isEnabled) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
				if(GUILayout.Button("Camera Controls\n(Status: ON)", customButtonStyle))
				{
					MTSceneCameraController.isEnabled = !MTSceneCameraController.isEnabled;
				}*/

				GUILayout.BeginHorizontal();

				if (GUILayout.Button("Tools"))
				{
					showOptionsTools = true;
					showOptionsUtilities = false;
					SceneView.RepaintAll();
				}

				if (GUILayout.Button("Utilities"))
				{
					showOptionsTools = false;
					showOptionsUtilities = true;
					SceneView.RepaintAll();
				}

				GUILayout.EndHorizontal();

				GUI.backgroundColor = new Color(0f, 1f, 0f, 1f);
				if (showOptionsTools)
				{
					GUILayout.Label("Tools");

					GUI.backgroundColor = (selectedTool==Tools.CARVE) ? new Color(0f,1f,0f,1f) : new Color(1f, 0f, 0f, 1f);
					if (GUILayout.Button("Carve", customButtonStyle)) { selectedTool = Tools.CARVE; }

					GUI.backgroundColor = (selectedTool == Tools.FILL) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
					if (GUILayout.Button("Fill", customButtonStyle)) { selectedTool = Tools.FILL; }

					GUI.backgroundColor = (selectedTool == Tools.TILES) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
					if (GUILayout.Button("Tiles", customButtonStyle)) { selectedTool = Tools.TILES; }

					GUI.backgroundColor = (selectedTool == Tools.PROPS) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
					if (GUILayout.Button("Props", customButtonStyle)) { selectedTool = Tools.PROPS; }
				}

				//Resetting back to the default color
				GUI.backgroundColor = defaultColor;

				if (showOptionsUtilities)
				{
					GUILayout.Label("Utilities");
					if (GUILayout.Button("Save", customButtonStyle))
					{
						MTSceneMapEditorTool_ChunkInterpreter.SaveChunks();
					}
					if (GUILayout.Button("Load", customButtonStyle))
					{
						MTSceneMapEditorTool_ChunkInterpreter.LoadChunks();
					}
					if (GUILayout.Button("Add Chunks (5x5)", customButtonStyle))
					{
						MTSceneMapEditorTool_ChunkInterpreter.MakeChunks();
					}
					GUI.backgroundColor = (generateChunksInEditor) ? Color.green : Color.red;
					if (GUILayout.Button("Generate Chunks(3x3)", customButtonStyle))
					{
						generateChunksInEditor = !generateChunksInEditor;
					}
					GUI.backgroundColor = defaultColor;
				}
				GUI.backgroundColor = defaultColor;

				//GUILayout.EndHorizontal();

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


				GUI.Label(new Rect(8, 10, toolbarRect.width - 16 - 6 - 16, 16), "Map Editor", style);
				// End the toolbar container
				GUI.EndGroup();

				switch(selectedTool)
				{
					case Tools.CARVE:
						Rect carveRect = new Rect(toolbarRect.x + toolbarRect.width, toolbarRect.y, 256, toolbarRect.height);
						GUI.BeginGroup(carveRect, GUI.skin.window);

						GUI.EndGroup();
						break;
					case Tools.FILL:
						Rect fillRect = new Rect(toolbarRect.x + toolbarRect.width, toolbarRect.y, 256, toolbarRect.height);
						GUI.BeginGroup(fillRect, GUI.skin.window);

						GUI.EndGroup();
						break;
					case Tools.TILES:
						Rect tilesRect = new Rect(toolbarRect.x + toolbarRect.width, toolbarRect.y, 256, toolbarRect.height);
						GUI.BeginGroup(tilesRect, GUI.skin.window);

						GUI.EndGroup();
						break;
					case Tools.PROPS:
						Rect propsRect = new Rect(toolbarRect.x + toolbarRect.width, toolbarRect.y, 256, toolbarRect.height);

						GUI.BeginGroup(propsRect, GUI.skin.window);
						w = propsRect.width - 20;
						h = propsRect.height - 40;
						Rect contentRect = new Rect(12, 32, w, h);
						GUI.Label(new Rect(48, 4, propsRect.width -48 - 24 -35 -16, 20), "Props/Prefabs", style);
						GUI.DrawTexture(contentRect, EditorGUIUtility.whiteTexture, ScaleMode.ScaleToFit, true, w / h, new Color(0f, 0f, 0f, 0.2f), 0f, 0f);

						//Here we are rendering the pre-rendered textures. If you do not see the textures here, you can click the refresh icon to re-render the textures

						int column = 0;
						int row = 0;


						var _prefabButtonStyle = new GUIStyle(GUI.skin.button);
						//customButtonStyle.fixedHeight = 20f;
						//This is for rendering all of the prefab buttons to the screen
						GUI.BeginGroup(contentRect, GUI.skin.window);
						Color oldBG = GUI.backgroundColor;

						foreach (Texture2D tex in MTSceneMapEditorTool_PrefabInterpreter.renderTextures.Values)
						{
							//This is our GUID, so we can detectt if we have this object selected or not
							string guid = MTSceneMapEditorTool_PrefabInterpreter.renderTexturesReverseLookup[tex];

							int __w = 32;
							int __h = 32;
							//
							GUI.backgroundColor = (guid == selectedPrefabGUID) ? new Color(0f, 1f, 0f, 1f) : new Color(1f, 0f, 0f, 1f);
							if (GUI.Button(new Rect(((float)column * 40) + 2, 2 + ((float)row * 40), __w, __h), tex, _prefabButtonStyle))
							{
								selectedPrefabGUID = guid;
							}

							column = column + 1;
							if (column >= 6)
							{
								column = 0;
								row += 1;
							}
						}

						GUI.backgroundColor = oldBG;
						GUI.EndGroup();

						float _width = 60.0f;
						float _height = 25f;

						if(GUI.Button(new Rect(propsRect.width - _width - 8, 4, _width, _height), "Refresh"))
						{
							Debug.Log("Re-Rendering Textures");
							MTSceneMapEditorTool_PrefabInterpreter.UpdateGUIds();
							MTSceneMapEditorTool_PrefabInterpreter.UpdateRenderTextures();
							SceneView.RepaintAll();
						}

						GUI.EndGroup();

						//Handling mouse clicks
						if (propsRect.Contains(Event.current.mousePosition))
						{
							canCancelMouseEvents = true;
						}
						break;
				}
				if(selectedTool!=Tools.NONE)
				{
					float _size = 25.0f;
					Rect _closeButtonRect = new Rect(toolbarRect.x + toolbarRect.width + 4 + 8, toolbarRect.y+4, size, size);
					if (GUI.Button(_closeButtonRect, "-", closeButtonStyle))
					{
						// Handle button click
						selectedTool = Tools.NONE;
						SceneView.RepaintAll();
					}
				}


				// Handle mouse input for dragging the toolbar
				//EditorGUIUtility.AddCursorRect(toolbarRect, MouseCursor.MoveArrow);
				if (toolbarRect.Contains(Event.current.mousePosition)) {
					canCancelMouseEvents = true;
				}
				if (canCancelMouseEvents)
				{
					if (Event.current.type == EventType.MouseDown && canCancelMouseEvents)
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
				if (generateChunksInEditor)
				{
					MTSceneMapEditorTool_ChunkInterpreter.GetChunkManager().UpdateChunksAroundEditorCamera();
				}
			}
		}
	}

	[InitializeOnLoad]
	public class MTSceneMapEditorTool_PrefabInterpreter
	{
		public static Dictionary<string, GameObject> storedPrefabs;
		public static Dictionary<GameObject, string> storedPrefabsReverseLookup;

		public static Dictionary<string, Texture2D> renderTextures;
		public static Dictionary<Texture2D, string> renderTexturesReverseLookup;

		static MTSceneMapEditorTool_PrefabInterpreter()
		{
			UpdateGUIds();
			UpdateRenderTextures();
		}

		public static void UpdateGUIds()
		{
			storedPrefabs = new Dictionary<string, GameObject>();
			storedPrefabsReverseLookup = new Dictionary<GameObject, string>();
			string assetPath = "Assets/Resources/Map/Props/";
			var GUIDs = AssetDatabase.FindAssets("t:GameObject", new string[] { assetPath });
			Debug.Log("Obtaining the GUIDs");

			foreach (string guid in GUIDs)
			{
				string _assetPath = AssetDatabase.GUIDToAssetPath(guid);
				GameObject prefabToLoad = (GameObject)AssetDatabase.LoadAssetAtPath(_assetPath, typeof(GameObject));

				storedPrefabs[guid] = prefabToLoad;
				storedPrefabsReverseLookup[prefabToLoad] = guid;
				Debug.Log("Registering prefab: " + prefabToLoad.name);
			}
		}

		public static void UpdateRenderTextures()
		{
			if (renderTextures!=null)
			{
				foreach (Texture2D tex in renderTextures.Values)
				{
					//This is how we will free up render textures
					GameObject.DestroyImmediate(tex);
				}
			}

			renderTextures = new Dictionary<string, Texture2D>();
			renderTexturesReverseLookup = new Dictionary<Texture2D, string>();

			Debug.Log("Rendering new textures");

			//Instantiating all the prefabs, and generating their textures
			foreach (string guid in storedPrefabs.Keys)
			{
				//Creating the camera we are going to use to render the object
				Camera newRenderCamera = new GameObject().AddComponent<Camera>();


				//First we are grabbing the instance from the GUID
				GameObject targetPrefab = storedPrefabs[guid];
				Debug.Log("Rendering prefab: " + targetPrefab.name);

				//Now we are spawning it in, and positioning it where it needs to go
				GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(targetPrefab);
				instantiatedPrefab.transform.position = new Vector3(0, -81920, -81920);

				PrefabPreviewConfig config = instantiatedPrefab.GetComponent<PrefabPreviewConfig>();

				//Configuring the camera
				newRenderCamera.transform.rotation = Quaternion.Euler(45f, 0, 0);
				newRenderCamera.clearFlags = CameraClearFlags.Color;
				newRenderCamera.orthographic = true;
				//Calculating the size based off of the content
				newRenderCamera.orthographicSize = (config) ? config.cameraZoom : 2f;

				newRenderCamera.transform.position = new Vector3(((config) ? config.cameraOffset.x : 0f), -81920 + ((config) ? config.cameraOffset.y : 2f), -81920 + ((config) ? config.cameraOffset.z : 2f));

				//Now we are generating a temporary texture
				RenderTexture tex = RenderTexture.GetTemporary(80, 80, 32, RenderTextureFormat.ARGB32);
				newRenderCamera.targetTexture = tex;
				newRenderCamera.Render();

				//Setting the active render texture, so we can read the pixels
				RenderTexture.active = tex;

				//Generating the texture
				Texture2D previewTexture = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
				previewTexture.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
				previewTexture.Apply();

				//Storing the generated texture
				renderTextures[guid] = previewTexture;
				renderTexturesReverseLookup[previewTexture] = guid;

				//Now we free up the memory
				RenderTexture.active = null;
				newRenderCamera.targetTexture = null;
				RenderTexture.ReleaseTemporary(tex);

				//Cleaning up the instantiated instance
				GameObject.DestroyImmediate (instantiatedPrefab.gameObject);
				newRenderCamera.Render();
				//
				//Freeing up the old camera we used to render the textures
				GameObject.DestroyImmediate(newRenderCamera.gameObject);
			}
		}
	}

	public class MTSceneMapEditorTool_ChunkInterpreter
	{
		public static ChunkManager GetChunkManager()
		{

			ChunkManager retVal = null;
			

			//First we find a chunk manager in the scene
			retVal = GameObject.FindObjectOfType<ChunkManager>();
			if(retVal == null)
			{
				//If we havnt found one, we spawn one in
				GameObject newObj = new GameObject();
				newObj.name = "ChunkManager";
				retVal = newObj.AddComponent<ChunkManager>();
			}

			//For safety measures, we are also setting the singleton
			ChunkManager.SINGLETON = retVal;

			return retVal;
		}

		public static void LoadChunks()
		{
			throw new NotImplementedException();
		}

		public static void SaveChunks()
		{
			throw new NotImplementedException();
		}

		public static void MakeChunks()
		{
			Debug.Log(SceneView.lastActiveSceneView.camera.transform.position);

			//Getting the chunk that the camera is currently located in
			Vector3 cameraPosiition = SceneView.lastActiveSceneView.camera.transform.position;
			Vector2Int currentChunkPos = new Vector2Int((int)cameraPosiition.x/32, (int)cameraPosiition.z/32);

			for(int x = -3; x < 3; x++)
			{
				for(int y = -3; y < 3; y++)
				{
					GetChunkManager().CreateChunk(x + currentChunkPos.x, y + currentChunkPos.y);
				}
			}
		}
	}
}