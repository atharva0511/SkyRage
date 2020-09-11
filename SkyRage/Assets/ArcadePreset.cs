
using UnityEngine;

public class ArcadePreset : MonoBehaviour
{
    EventSettings eventHandler;
    public Objective[] objectives;
    public Transform UTSpawner;
    public GameObject upgradeToken;
    public GameObject Life;
    ArcadeManager arcadeManager;
    // Start is called before the first frame update
    void Awake()
    {
        eventHandler = FindObjectOfType<EventSettings>();
        foreach(Objective ob in objectives)
        {
            ob.eventSettings = eventHandler;
        }
    }
    
    public void SpawnUT()
    {
        Instantiate(upgradeToken, UTSpawner.position, Quaternion.identity,this.transform);
    }

    public void SpawnLife()
    {
        Instantiate(Life, UTSpawner.position, Quaternion.identity, this.transform);
    }
}
