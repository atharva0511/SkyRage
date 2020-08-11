using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;

public class Explosion : MonoBehaviour {

    public float explosionForce = 4;

    private IEnumerator Start()
    {
        // wait one frame because some explosions instantiate debris which should then
        // be pushed by physics force
        //yield return null;

        float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;

        float r = 20 * multiplier;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in cols)
        {
            if (col.GetComponentInParent<Destructible>() != null)
            {
                float v = 5000*multiplier / (20 + (col.transform.position - this.transform.position).sqrMagnitude);
                col.GetComponentInParent<Destructible>().TakeDamage(v,5);
            }
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }
        yield return null;
        foreach (var rb in rigidbodies)
        {
            if(rb!=null)
                rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }

        //TODO
        Destroy(this.gameObject,4f);
    }
}
