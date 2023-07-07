using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class LevelEditor : EditorWindow
{
    /*
    public readonly string CAMERA_NAME = "~~!!TILE PREVIEW CAMERA!!~~";
    public readonly string TILE_NAME = "~~!!TILE PREVIEW TARGET!!~~";
    public readonly Vector3 PREVIEW_LOCATION = new Vector3(42523, 23453, 42352);

    /// <summary>
    /// The menu item which triggers the window to display
    /// </summary>
    [MenuItem("MoonTune Tools/Level Editor")]
    public static void ShowLevelEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow levelEditorWindow = GetWindow<LevelEditor>();
        levelEditorWindow.titleContent = new GUIContent("Level Editor");
    }

    /// <summary>
    /// Creates the  GUI for this editor window. Called
    /// on Unity reloads
    /// </summary>
    public void CreateGUI()
    {
        // Import UXML
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Engine/LevelEditor/UIDocuments/LevelEditor.uxml");
        VisualElement holdingEle = visualTree.Instantiate();

        VisualElement tileParent = holdingEle.Query<VisualElement>("TileParent");
        VisualElement tilePreview = holdingEle.Query<VisualElement>("TilePreview");

        string[] textureGuids = AssetDatabase.FindAssets("t:Texture2D");
        foreach (string texGuid in textureGuids)
        {
            tileParent.Add(CreateTile(texGuid));
        }

        ScrollView scrollView = new ScrollView();
        scrollView.Add(tileParent);

        TwoPaneSplitView tpsv = new TwoPaneSplitView(1, 400, TwoPaneSplitViewOrientation.Vertical);
        tpsv.Add(scrollView);
        tpsv.Add(tilePreview);

        rootVisualElement.Add(tpsv);

        GetPreviewOfTile(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/PL_1_Low_2.fbx"));
        GetPreviewOfTile(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/Cube.prefab"));
    }

    public void OnDestroy()
    {
        DeleteCameraIfExists();
        DeletePreviewTargetIfExists();
    }

    private VisualElement CreateTile(string tileGuid)
    {
        Texture2D tileTex = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(tileGuid));
        Image img = new Image
        {
            scaleMode = ScaleMode.ScaleToFit,
            sprite = Sprite.Create(tileTex, new Rect(0, 0, tileTex.width, tileTex.height), Vector2.zero)
        };
        img.style.width = new StyleLength(new Length(75, LengthUnit.Pixel));
        img.style.height = new StyleLength(new Length(75, LengthUnit.Pixel));

        return img;
    }

    private void GetPreviewOfTile(GameObject objPrefab)
    {
        GameObject camera = CreateCamera();
        Camera cameraComponent = camera.GetComponent<Camera>();

        GameObject tile = Instantiate(objPrefab);
        tile.name = TILE_NAME;
        tile.hideFlags = HideFlags.HideAndDontSave;
        tile.transform.position = PREVIEW_LOCATION;
        Vector3 tileExtents = tile.GetComponent<MeshFilter>().sharedMesh.bounds.size;

        camera.transform.position = PREVIEW_LOCATION + new Vector3(
            0,
            Mathf.Max(tileExtents.x, tileExtents.y, tileExtents.z) + Mathf.Tan(0.5f * Mathf.Deg2Rad * cameraComponent.fieldOfView),
            0
        );

        RenderTexture targetTex = new RenderTexture(75, 75, 16);
        cameraComponent.targetTexture = targetTex;
        cameraComponent.Render();

        //This might not be needed, because you can load and unload previews on
        //demand. I will setup an example script sometime this week for you.
        // - rater193
        SaveRenderTexture(targetTex, string.Format("Assets/Editor/TilePreviews/{0}.png", objPrefab.name));

        //AssetDatabase.CreateAsset(sprite, string.Format("Assets/Editor/{0}.sprite", objPrefab.name));

        DestroyImmediate(tile);
        DestroyImmediate(camera);
    }

    private GameObject CreateCamera()
    {
        GameObject camera = new GameObject(CAMERA_NAME);
        //camera.hideFlags = HideFlags.HideAndDontSave;
        Camera cameraComponent = camera.AddComponent<Camera>();
        cameraComponent.enabled = false;
        camera.transform.rotation = Quaternion.Euler(90, 0, 0);

        return camera;
    }

    private void DeleteCameraIfExists()
    {
        GameObject cam = GameObject.Find(CAMERA_NAME);
        while(cam != null)
        {
            DestroyImmediate(cam);
            cam = GameObject.Find(CAMERA_NAME);
        }
    }

    private void DeletePreviewTargetIfExists() {
        GameObject tile = GameObject.Find(TILE_NAME);
        while (tile != null)
        {
            DestroyImmediate(tile);
            tile = GameObject.Find(TILE_NAME);
        }
    }

    private void SaveRenderTexture(RenderTexture renderTexture, string assetPath)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = renderTexture;

        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;

        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(assetPath, bytes);
        AssetDatabase.Refresh();
    }*/
}
