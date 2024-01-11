using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class LevelSelectWindow : EditorWindow
{
    [MenuItem("Tools/LevelSelectWindow %#l")]
    public static void ShowWindow()
    {
        GetWindow<LevelSelectWindow>("Select Level");
    }
    
    private void OnGUI()
    {
        GUILayout.Space(15);
        GUILayout.Label("SELECT LEVEL On Build Index\n");
            
        GUIStyle customStyle = new GUIStyle(GUI.skin.button);
        customStyle.fontStyle = FontStyle.Bold;
        customStyle.fontSize = 15;
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label($"({i})  {GetSceneNameWithIndex(i)}", EditorStyles.boldLabel);

            if (GUILayout.Button("Open", customStyle, GUILayout.Height(20), GUILayout.Width(65)))
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(GetScenePathWithIndex(i));
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        GUILayout.Space(15);
        GUILayout.Label("SELECT LEVEL On Scene Folder\n");
        GUILayout.Space(5);

        bool sameNameFounded = false;
        int falseIndexCount = 0;
        string[] sceneGUIDs = GetSceneGUIDs();
        
        for (int i = 0; i < sceneGUIDs.Length; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(sceneGUIDs[i]));

            for (int j = 0; j < SceneManager.sceneCountInBuildSettings; j++)
            {
                if (string.Equals(GetSceneNameWithIndex(j), sceneName))
                {
                    sameNameFounded = true;
                    break;
                }
            }

            if (sameNameFounded)
            {
                falseIndexCount++;
                sameNameFounded = false;
                continue;
            }
            
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label($"({i-falseIndexCount})  {sceneName}", EditorStyles.boldLabel);

            if (GUILayout.Button("Open", customStyle, GUILayout.Height(20), GUILayout.Width(65)))
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(sceneGUIDs[i]));
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
    
    private string GetSceneNameWithIndex(int index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        return path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
    }
    
    private string GetScenePathWithIndex(int index)
    {
        return SceneUtility.GetScenePathByBuildIndex(index);
    }
    
    private string[] GetSceneGUIDs()
    {
        string folderPath = "Assets/Scenes";
        return AssetDatabase.FindAssets("t:SceneAsset", new[] { folderPath });
    }
}
