using UnityEngine;

[CreateAssetMenu(menuName = "Custom Cursor", fileName = "NewCursorDefinition")]
public class CursorDefinition : ScriptableObject
{
    public string cursorName;
    public Texture2D texture;
    public Vector2 hotSpot;
}
