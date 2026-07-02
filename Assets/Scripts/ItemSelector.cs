using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private DraggableItem itemPrefab;
    [SerializeField] private RobotPartSO assignedRobotPart;

    DraggableItem currentItem;

    private void Awake()
    {
        InitializeNewItem();
    }

    private void Update()
    {
        if (DraggableItem.CurrentDraggedItem != null && DraggableItem.CurrentDraggedItem == currentItem)
        {
            InitializeNewItem();
        }
    }

    public void SetAssignedRobotPart(RobotPartSO robotPart)
    {
        assignedRobotPart = robotPart;
        if (currentItem != null)
        {
            currentItem.SetAssignedRobotPart(assignedRobotPart);
        }
    }

    private void InitializeNewItem()
    {
        currentItem = null;

        currentItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
        currentItem.transform.SetParent(transform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.SetAssignedRobotPart(assignedRobotPart);
    }
}
