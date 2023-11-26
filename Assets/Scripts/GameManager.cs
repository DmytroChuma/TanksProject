using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currTank;
    public List<GameObject> tanks;
    public Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject tank = tanks[currTank];
        tank.GetComponent<TankController>().enabled = true;
    }

    public int GetNextTank()
    {
        GameObject tank = tanks[currTank];
        tank.GetComponent<TankController>().enabled = false;
        currTank = currTank + 1;
        if (currTank > tanks.Count - 1)
        {
            currTank = 0;
        }
        tank = tanks[currTank];
        tank.GetComponent<TankController>().enabled = true;
        return currTank;
    }
}
