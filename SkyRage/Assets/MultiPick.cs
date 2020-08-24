
using UnityEngine;

public class MultiPick : MonoBehaviour {

    public bool isObjective = true;
    public int pickCount = 0;
    public int zoneRadius = 150;
    public bool showGizmo = true;

    public void Start()
    {
        Count();
    }

    public void OnDrawGizmos()
    {
        if (!showGizmo) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }

    public void OnCollect()
    {
        Count();
        if (pickCount <= 0)
        {
            GetComponent<Objective>().Completed();
        }
    }

    void Count()
    {
        int c = 0;
        foreach(Pickup p in GetComponentsInChildren<Pickup>())
        {
            if(!p.picked)
                c++;
        }
        pickCount = c;
    }
}
