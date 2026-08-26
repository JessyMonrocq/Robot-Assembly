using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaySoundEvent))]
public class PlaySoundEventEditor : Editor
{
    private SerializedProperty soundNameProp;

    private void OnEnable()
    {
        soundNameProp = serializedObject.FindProperty("soundName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PlaySoundEvent playSoundEvent = (PlaySoundEvent)target;

        AudioManager audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            audioManager = Object.FindFirstObjectByType<AudioManager>();
        }

        if (audioManager == null || audioManager.sounds == null || audioManager.sounds.Length == 0)
        {
            EditorGUILayout.HelpBox("No AudioManager with sounds found in the scene. You can still type a sound name manually.", MessageType.Warning);
            EditorGUILayout.PropertyField(soundNameProp, new GUIContent("Sound Name"));
        }
        else
        {
            string[] names = new string[audioManager.sounds.Length];
            for (int i = 0; i < audioManager.sounds.Length; i++)
                names[i] = audioManager.sounds[i].name;

            int currentIndex = System.Array.IndexOf(names, soundNameProp.stringValue);
            if (currentIndex < 0) currentIndex = 0;

            int selected = EditorGUILayout.Popup("Sound", currentIndex, names);
            soundNameProp.stringValue = names[selected];
        }

        serializedObject.ApplyModifiedProperties();

        // Optional: draw rest of the default inspector if needed
        // DrawDefaultInspector();
    }
}
