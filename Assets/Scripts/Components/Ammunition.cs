using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour, ITankComponent
{
    public int HitPoints = 10;
    public GameObject Turret;

    public void BreakDown()
    {
        Rigidbody turretrb = Turret.AddComponent<Rigidbody>();
        turretrb.mass = 10000;
        turretrb.AddForce(new Vector3(Random.Range(-3f, 3f) * 10000f, 100000f, Random.Range(-2f, 2f) * 10000f), ForceMode.Impulse);
        turretrb.AddTorque(new Vector3(Random.Range(-1f, 1f) * 100f, Random.Range(-1f, 1f) * 100f, Random.Range(-1f, 1f) * 100f));
        Turret.transform.parent = null;
        MeshCollider ms = Turret.AddComponent<MeshCollider>();
        ms.convex = true;
        Destroy(gameObject);
    }

    public void TakeDamage()
    {

    }

    public void testF()
    {

        Debug.Log("test interface");
    }
}
