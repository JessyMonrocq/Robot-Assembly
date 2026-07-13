using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RequestsListSO", menuName = "Scriptable Objects/RequestsListSO")]
public class RequestsListSO : ScriptableObject
{
    public List<RequestSO> RequestList;
}
