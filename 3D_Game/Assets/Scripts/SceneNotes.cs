using UnityEditor;
using UnityEngine;

public class SceneNotes : MonoBehaviour
{
    [Header("Scene Notes")]

    [TextArea(2, 8)]
    public string NoteText = "Write your notes here...";

    public NoteType noteType = NoteType.Zama;

    [Header("Appearance")]

    public Color noteColor = Color.yellow;

    [Range(0.1f, 2f)]
    public float markerSize = 0.25f;

    [Range(0.1f, 2f)]
    public float textScale = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = noteColor;

        Gizmos.DrawSphere(transform.position, markerSize);

        Gizmos.DrawLine(transform.position,
                transform.position + Vector3.up * 0.5f);

        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);

        style.normal.textColor = noteColor;
        style.fontSize = Mathf.RoundToInt(12 * textScale);
        style.fontStyle = FontStyle.Bold;

        style.padding = new RectOffset(8, 8, 5, 5);

        string title = "[" + noteType.ToString() + "]";

        Handles.Label(transform.position + Vector3.up * 0.55f, title + "\n" + NoteText, style);
    }

    public enum NoteType
    {
        Zama, Angela, Favour, Label
    }
}
