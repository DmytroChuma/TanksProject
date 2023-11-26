using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TankShell
{
    public int Calibre;
    public float Speed;
    public float Mass;
    public int ArmorPenetration;
    public int ReboundAngle;
    public int PosibleRicochetChance;
    [Range(0.0f, 2.0f)]
    public float Slope;
    public enum Type
    {
        ArmorPiercing
    }
    public Type type;
}


public class TurretController : MonoBehaviour
{

    [Header("Object Settings")]
    public Transform Turret;
    public Transform Gun;

    [Header("Turret Rotate Settings")]
    public float TurretSpeed = 2f;
    public float TurretMax = 360f, TurretMin = -360f;

    [Header("Gun Rotate Settings")]
    public float GunSpeed = 2f;
    public float GunMax = 360f, GunMin = -360f;

    [Header("Fire Settings")]
    public float FireRate = 1f;
    public float NextFire = 0f;

    public GameObject Effect;

    [Header("Shells Spawn")]
    public GameObject Shell;
    public Transform SpawnPoint;

    [Header("Shell Settings")]
    public List<TankShell> Shells;
    public int CurrentShell = 0;

    void Start()
    {
        float temp = GunMax;
        GunMax = GunMin;
        GunMin = temp;
    }


    void TankShot()
    {
        Effect.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float steering = 0;
        float gunSteering = 0;

        if (steering != 0)
        {
            float change = steering * TurretSpeed * Time.deltaTime;

            if (checkRotate(Turret.localEulerAngles.y, Turret.localEulerAngles.y + change, TurretMax, TurretMin))
            {
                Turret.transform.Rotate(0, change, 0, Space.Self);
            }

        }

        if (gunSteering != 0)
        {
            float change = gunSteering * GunSpeed * Time.deltaTime;

            if (checkRotate(Gun.localEulerAngles.x, Gun.localEulerAngles.x + change, GunMax, GunMin))
            {
                Gun.transform.Rotate(change, 0, 0, Space.Self);
            }
        }

    }

    private bool checkRotate(float value, float change, float max, float min)
    {

        value = (change > 180) ? change - 360 : change;
        if (value < 0 && value <= min)
        {
            return false;
        }
        else if (value > 0 && value >= max)
        {
            return false;
        }
        return true;
    }

}
