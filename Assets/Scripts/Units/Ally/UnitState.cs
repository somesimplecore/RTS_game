using UnityEngine;

public class UnitState : MonoBehaviour
{
    public bool canMove;
    public State status;
    public Vector3 followPoint;
    public DamageableObject enemy;
    public Building building;
    public enum State
    {
        idle,
        running,
        attacking,
        working
    }

    public void Init(bool newCanMove, 
        State newStatus = State.idle, 
        Vector3 newFollowPoint = new Vector3(), 
        DamageableObject newEnemy = null, Building 
        newBuilding = null)
    {
        canMove = newCanMove;
        status = newStatus;
        followPoint = newFollowPoint;
        enemy = newEnemy;
        building = newBuilding;
    }
}
