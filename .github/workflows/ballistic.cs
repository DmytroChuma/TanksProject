using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballistic : MonoBehaviour
{

    public Transform spawnPoint;
    public Transform targetPoint;
    public Transform turret;

    public float TurretRotateSpeed;

    public Transform cannon;
    public Transform cannonPoint;
    public float GunMax;
    public float GunMin;
    public float CannonRotationSpeed;

    float g = Physics.gravity.y;
    float h;
    Vector3 a;

    // Start is called before the first frame update
    void Start()
    {
        float temp = GunMax;
        GunMax = GunMin;
        GunMin = temp;
        TurretRotateSpeed = TurretRotateSpeed * Mathf.PI / 180;
        CannonRotationSpeed = CannonRotationSpeed * Mathf.PI / 180;
        h = spawnPoint.position.y;
        a = cannon.eulerAngles;
    }

    void FixedUpdate()
    {
        Vector3 fromTo = targetPoint.position - turret.position;
        Vector3 fromToCannon = targetPoint.position - cannonPoint.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);
        Vector3 fromToY = new Vector3(fromToCannon.x, 0f, 0f);

        float dist = Vector3.Distance(spawnPoint.position, targetPoint.position);

        float fi = 180 / Mathf.PI * Mathf.Atan((float)dist / h);
        float angle = ((180 / Mathf.PI * Mathf.Acos((((Mathf.Abs((float)9.8) * Mathf.Pow(dist, 2)) / Mathf.Pow(815, 2)) - h) / (Mathf.Sqrt(Mathf.Pow(h, 2) + Mathf.Pow(dist, 2))))) + fi) / 2;

        turret.transform.rotation = Quaternion.Lerp(turret.rotation, Quaternion.LookRotation(fromTo), Time.deltaTime * TurretRotateSpeed);
        turret.transform.localEulerAngles = new Vector3(0, turret.localEulerAngles.y, 0);

        cannonPoint.rotation = Quaternion.LookRotation(fromToCannon);

        float value = (cannonPoint.localEulerAngles.x + ((90 - angle) * -1) > 180) ? cannonPoint.localEulerAngles.x + ((90 - angle) * -1) - 360 : cannonPoint.localEulerAngles.x + ((90 - angle) * -1);

        if (value > GunMax)
        {
            cannonPoint.transform.localEulerAngles = new Vector3(GunMax, 0, 0);
        }
        else if (value < GunMin)
        {
            cannonPoint.transform.localEulerAngles = new Vector3(GunMin, 0, 0);
        }
        else
        {
            cannonPoint.transform.localEulerAngles = new Vector3(cannonPoint.localEulerAngles.x + ((90 - angle) * -1), 0, 0);
        }

        cannon.rotation = Quaternion.RotateTowards(cannon.rotation, cannonPoint.rotation, CannonRotationSpeed * Time.deltaTime);

        cannon.transform.localEulerAngles = new Vector3(cannon.localEulerAngles.x, 0, 0);
    }

}
