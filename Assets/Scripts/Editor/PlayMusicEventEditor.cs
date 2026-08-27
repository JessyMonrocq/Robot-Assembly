using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayMusicEvent))]
public class PlayMusicEventEditor : Editor
{
    private SerializedProperty musicNameProp;

    private void OnEnable()
    {
        musicNameProp = serializedObject.FindProperty("musicName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PlayMusicEvent playMusicEvent = (PlayMusicEvent)target;

        AudioManager audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            audioManager = Object.FindFirstObjectByType<AudioManager>();
        }

        if (audioManager == null || audioManager.musics == null || audioManager.musics.Length == 0)
        {
            EditorGUILayout.HelpBox("No AudioManager with musics found in the scene. You can still type a music name manually.", MessageType.Warning);
            EditorGUILayout.PropertyField(musicNameProp, new GUIContent("Music Name"));
        }
        else
        {
            string[] names = new string[audioManager.musics.Length];
            for (int i = 0; i < audioManager.musics.Length; i++)
                names[i] = audioManager.musics[i].name;

            int currentIndex = System.Array.IndexOf(names, musicNameProp.stringValue);
            if (currentIndex < 0) currentIndex = 0;

            int selected = EditorGUILayout.Popup("Music", currentIndex, names);
            musicNameProp.stringValue = names[selected];
        }

        serializedObject.ApplyModifiedProperties();
    }
}