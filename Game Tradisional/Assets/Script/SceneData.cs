using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Data", menuName = "Scriptable Objects/Scene Data")]
public class SceneData : ScriptableObject
{
    public string sceneTitle;
    public Sprite sceneImage;
    [TextArea]
    public string sceneDescription;
}