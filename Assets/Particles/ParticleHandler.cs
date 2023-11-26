using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public bool Destroy = true;
    public float TimeToDestroy_Deactivate;


    void OnEnable()
    {
        Debug.Log("START");
        if(Destroy){
            Destroy(gameObject, TimeToDestroy_Deactivate);
        }
        else{
            StartCoroutine(deactivate());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator deactivate(){
        yield return new WaitForSeconds (TimeToDestroy_Deactivate);
        gameObject.SetActive(false);
    }
}
