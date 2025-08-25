using UnityEngine;
using System.Linq;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Invoke(nameof(InitializeDoorLocks), 0.1f);
    }

    public void InitializeDoorLocks()
    {
        var lockableDoors = Door.AllDoors.Where(d => d.canBeLocked).ToList();
        var shuffled = lockableDoors.OrderBy(_ => Random.value).ToList();

        int doorsToLock = Mathf.Min(4, lockableDoors.Count);
        for (int i = 0; i < shuffled.Count; i++)
        {
            shuffled[i].isLocked = i < doorsToLock;
            shuffled[i].ApplyLockState();

            Debug.Log($"{Time.time}: {shuffled[i].doorName} => {(shuffled[i].isLocked ? "LOCKED" : "UNLOCKED")}");
        }
    }
}
