using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void LoadBattle(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void GetNextTank()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.GetNextTank();
    }
}
