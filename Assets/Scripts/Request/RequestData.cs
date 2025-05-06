using UnityEngine;

[CreateAssetMenu(menuName = "SO/Requests")]
public class RequestData : ScriptableObject
{
    public RequestType requestType;
    public string description;
    public float timeLimit;
    public float satisfactionPenalty;
}