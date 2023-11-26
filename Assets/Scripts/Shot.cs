
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float FireRate = 1f;
    public float NextFire = 0f;
    public GameObject Shell;
    public Transform SpawnPoint;
    public Animation anim;
    public GameObject manager;

    public void TankShot()
    {
        TankShell sh = manager.GetComponent<GameManager>().tanks[0].GetComponent<TurretController>().Shells[0];
        GameObject bullet = Instantiate(Shell, SpawnPoint.position, SpawnPoint.rotation);
    }
}
