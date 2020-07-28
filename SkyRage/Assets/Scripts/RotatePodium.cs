
using UnityEngine;

public class RotatePodium : MonoBehaviour {

    public Transform podium;
    public float speed = 1;

    public void RotPod()
    {
        podium.Rotate(0,-2*Input.GetAxis("Mouse X")*speed,0);
    }
}
