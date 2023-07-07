using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEditor.PackageManager;


#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace MoonToons
{
    [ExecuteInEditMode]
    public class ChunkManager : MonoBehaviour
    {
        public static ChunkManager SINGLETON;
        public Dictionary<string, GameObject> chunks = new Dictionary<string, GameObject>();
		public static bool hasInitialized = false;

		// Start is called before the first frame update
		private void Start()
		{
			Initialize();
		}

		void Initialize()
        {
			if (hasInitialized == false)
			{
				SINGLETON = this;
				Debug.Log("Scanning for children");
				for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
				{
					GameObject child = transform.GetChild(childIndex).gameObject;
					if(child.GetComponent<WorldChunk>() != null)
					{
						WorldChunk childWorldChunk = child.GetComponent<WorldChunk>();
						AddChunk(childWorldChunk.ChunkX, childWorldChunk.ChunkY, childWorldChunk.gameObject);
						childWorldChunk.gameObject.SetActive(false);
					}
				}
				hasInitialized = true;
			}
		}


        // Update is called once per frame
        void Update()
        {
#if (UNITY_EDITOR)
            //This code is ran inside the editor
			if(EditorApplication.isPlaying)
            {
				UpdateChunksAroundGameCamera();

			}
            else
			{
				//UpdateChunksAroundEditorCamera();
			}
#else
            //This code is ran inside the compiled game
            UpdateChunksAroundGameCamera()
#endif
		}

		void UpdateChunksAroundGameCamera()
        {

        }

#if (UNITY_EDITOR)
        public void UpdateChunksAroundEditorCamera()
        {
			Vector3 cameraPosiition = SceneView.lastActiveSceneView.camera.transform.position;
			Vector2Int currentChunkPos = new Vector2Int((int)cameraPosiition.x / 32, (int)cameraPosiition.z / 32);

			for (int x = -1; x < 1; x++)
			{
				for (int y = -1; y < 1; y++)
				{
					CreateChunk(x + currentChunkPos.x, y + currentChunkPos.y);
				}
			}
		}
#endif

		private string GetChunkKey(int x, int y)
        {
            return x + "_" + y;
        }

		public void RegisterChunk(int x, int y, GameObject chunk)
        {
			
        }

        public void CreateChunk(int x, int y)
		{
			Initialize();

			GameObject prefabTileReference = Resources.Load<GameObject>("Map/Tiles/GroundTileTemplate");
			//Getting where we want to store the chunk for future reference
			string chunkKey = GetChunkKey(x, y);

            //If we already have a chunk created, then we want to remove the old chunk
			if (chunks.ContainsKey(chunkKey))
			{

				GameObject chunk = chunks[chunkKey];

                if(chunk==null)
				{
					DestroyImmediate(chunk);
					chunks.Remove(chunkKey);
                }
                else
				{
					return;
				}
			}

            //Now we generate the new chunk
			GameObject newObj = new GameObject();
            newObj.name = x + "_" + y;
            newObj.transform.position = new Vector3(x*32, 0, y*32);
            newObj.transform.parent = transform;

            //Adding and configuring the chunks
            WorldChunk worldChunk = newObj.AddComponent<WorldChunk>();
			worldChunk.ChunkX = x;
			worldChunk.ChunkY = y;


			for (int _x = 0; _x < 32; _x++)
            {
                for(int _y = 0; _y < 32; _y++)
                {
                    
                    GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(prefabTileReference);
					newTile.transform.parent = newObj.transform;
                    newTile.transform.localPosition = new Vector3(_x, 0 ,_y);
					PrefabUtility.RecordPrefabInstancePropertyModifications(newTile);
				}
			}


			//and store in for future reference
			AddChunk(x, y, newObj);


		}

        private void AddChunk(int x, int y, GameObject chunk)
		{
			string chunkKey = GetChunkKey(x, y);
			if (chunks.ContainsKey(chunkKey))
			{

				if (chunk == null)
				{
				}
				else
				{
					return;
				}
			}
			chunks.Add(chunkKey, chunk);
		}
    }
}