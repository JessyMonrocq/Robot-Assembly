using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private DraggableParts itemPrefab;
    [SerializeField] private RobotPartSO assignedRobotPart;

    DraggableParts currentItem;

    private void Awake()
    {
        InitializeNewItem();
    }

    private void Update()
    {
        if (DraggableParts.CurrentDraggedItem != null && DraggableParts.CurrentDraggedItem == currentItem)
        {
            InitializeNewItem();
        }
    }

    public void SetAssignedRobotPart(RobotPartSO robotPart)
    {
        assignedRobotPart = robotPart;
        if (currentItem != null)
        {
            currentItem.GetComponent<DraggableParts>().SetAssignedRobotPart(assignedRobotPart);
        }
    }

    private void InitializeNewItem()
    {
        currentItem = null;

        currentItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
        currentItem.transform.SetParent(transform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.GetComponent<DraggableParts>().SetAssignedRobotPart(assignedRobotPart);
    }
}
