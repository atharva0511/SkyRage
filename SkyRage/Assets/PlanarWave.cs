using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanarWave : MonoBehaviour
{
    float startTime = 0;
    public float fadeTime = 3;
    public float scaleFactor = 100;
    public Renderer[] rends;
    Vector3 initScale;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        initScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < startTime + fadeTime)
        {
            foreach(Renderer rend in rends)
            {
                rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, (startTime + fadeTime - Time.time) / fadeTime);
            }
            transform.localScale = Vector3.Lerp(initScale, scaleFactor * initScale, (Time.time - startTime) / fadeTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
