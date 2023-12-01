
using UnityEngine;
using UnityEngine.UI;
public class Armor : MonoBehaviour
{
        public int Thickness;
    public bool additional = false;

    void OnTriggerEnter(Collider collision)

    {
        Shell s = collision.gameObject.GetComponent<Shell>();
        s.ArmorPenetration = s.ArmorPenetration - Thickness;
    }
}
