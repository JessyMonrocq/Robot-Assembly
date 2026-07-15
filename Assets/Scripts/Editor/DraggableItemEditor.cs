using UnityEditor;

[CustomEditor(typeof(DraggableParts))]
public class DraggableItemEditor : Editor
{
    SerializedProperty robotPartsListProperty;
    SerializedProperty assignedRobotPartProperty;

    private void OnEnable()
    {
        robotPartsListProperty = serializedObject.FindProperty("robotPartsListSO");
        assignedRobotPartProperty = serializedObject.FindProperty("assignedRobotPart");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Editor.DrawPropertiesExcluding(serializedObject, "robotPartsListSO", "assignedRobotPart");
        EditorGUILayout.PropertyField(robotPartsListProperty);

        RobotPartsListSO listSO = robotPartsListProperty.objectReferenceValue as RobotPartsListSO;
        RobotPartSO current = assignedRobotPartProperty.objectReferenceValue as RobotPartSO;

        if (listSO != null && listSO.RobotPartsList != null && listSO.RobotPartsList.Count > 0)
        {
            string[] options = new string[listSO.RobotPartsList.Count];
            for (int i = 0; i < options.Length; i++)
            {
                var so = listSO.RobotPartsList[i];
                options[i] = (so != null && !string.IsNullOrEmpty(so.PartName)) ? so.PartName : (so != null ? so.name : "<null>");
            }

            int currentIndex = -1;
            for (int i = 0; i < listSO.RobotPartsList.Count; i++)
            {
                if (listSO.RobotPartsList[i] == current)
                {
                    currentIndex = i;
                    break;
                }
            }

            int selected = EditorGUILayout.Popup("Assigned Robot Part", currentIndex, options);
            if (selected != currentIndex && selected >= 0 && selected < listSO.RobotPartsList.Count)
            {
                assignedRobotPartProperty.objectReferenceValue = listSO.RobotPartsList[selected];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Requires a RobotPartsListSO with at least one RobotPartSO item", MessageType.Info);
            EditorGUILayout.PropertyField(assignedRobotPartProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
