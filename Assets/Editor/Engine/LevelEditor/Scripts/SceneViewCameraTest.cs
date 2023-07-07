using UnityEngine;
using UnityEditor;

public class SceneViewCameraTest : ScriptableObject
{
	[MenuItem("Test/Move Scene View Camera")]
	static public void MoveSceneViewCamera()
	{

		/*
		Vector3 position = SceneView.lastActiveSceneView.pivot;
		position.z = 0f;
		position.y = 4f;
		position.x = -4f;
		SceneView.lastActiveSceneView.pivot = position;
		SceneView.lastActiveSceneView.rotation = Quaternion.AngleAxis(-45f, Vector3.left);

		SceneView.lastActiveSceneView.Repaint();
		*/
		if (Camera.current != null) // Check if a camera is active in the Scene View
		{
			Camera sceneCamera = SceneView.lastActiveSceneView.camera;
			Vector3 pivot = SceneView.lastActiveSceneView.pivot;
			float distance = Vector3.Distance(pivot, sceneCamera.transform.position);

			Debug.Log("Distance: " + distance);
		}
	}
}