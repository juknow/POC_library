using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "/So/Requests")]
public class RequestData : ScriptableObject
{
    public RequestType requestType;
    public string description;
    public float timeLimit;
    public float satisfactionPenalty;
}
