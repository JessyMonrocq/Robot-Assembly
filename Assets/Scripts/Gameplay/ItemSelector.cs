using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private DragItem itemPrefab;
    [SerializeField] private RobotPartSO assignedRobotPart;

    DragItem currentItem;

    private void Awake()
    {
        InitializeNewItem();
    }

    private void Update()
    {
        if (DragItem.CurrentDraggedItem != null && DragItem.CurrentDraggedItem == currentItem)
        {
            InitializeNewItem();
        }
    }

    public void SetAssignedRobotPart(RobotPartSO robotPart)
    {
        assignedRobotPart = robotPart;
        if (currentItem != null)
        {
            currentItem.GetComponent<RobotDraggable>().SetAssignedRobotPart(assignedRobotPart);
        }
    }

    private void InitializeNewItem()
    {
        currentItem = null;

        currentItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
        currentItem.transform.SetParent(transform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.GetComponent<RobotDraggable>().SetAssignedRobotPart(assignedRobotPart);
    }
}
