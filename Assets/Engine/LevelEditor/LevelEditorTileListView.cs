using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class LevelEditor
{
    private List<string> _assetGuids;

    public VisualElement LevelEditorTileListVieww(List<string> assetGuids)
    {
        _assetGuids = assetGuids;

        ListView listView = new ListView();
        listView.makeItem = MakeItem;
        listView.bindItem = BindItem;
        listView.itemsSource = assetGuids;
        listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        return listView;
    }

    private VisualElement MakeItem()
    {
        VisualElement ve = new VisualElement();
        ve.Add(new Label("Hello!22222"));
        ve.Add(new Image());
        return ve;
    }

    private void BindItem(VisualElement item, int index)
    {
        //string assetGuid = _assetGuids[index]
        Texture2D assetGuid = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:Texture2D")[0]));
        Image img = (item[1] as Image);
        img.StretchToParentWidth();
        img.scaleMode = ScaleMode.ScaleToFit;
        
        (item[1] as Image).sprite = Sprite.Create(assetGuid, new Rect(0, 0, assetGuid.width, assetGuid.height), Vector2.zero);
    }
}
