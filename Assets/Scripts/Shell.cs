using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shell : MonoBehaviour
{

    public int calibre = 20;
    public float speed = 0f;
    public float mass = 1f;
    public int ArmorPenetration = 30;
    public int reboundAngle = 90;
    [Range(0.0f, 2.0f)]
    public float slope;

    public float TimeToDestroy = 5f;
    private float destroyTime;

    private Rigidbody rb;
    private bool collision = false;
    Vector3 myVelocity;

    private float collisionAngle;
    private Vector3 normal;

    private bool richochet = false;

    public GameObject ShellType;
    public GameObject Splinter;
    private bool type = false;

    public GameObject sparks;

    public Text txt;
    private const string breaking = "Пробив";
    private const string noBreaking = "Не пробив";
    private const string ricochet = "Рикошет";
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = this.mass;
        rb.velocity = transform.forward * speed;

        GameObject t = GameObject.Find("Text");
        txt = t.GetComponent<Text>();
        txt.text = "";
    }


    private void OnCollisionEnter(Collision collision)
    {

        this.collision = true;
        normal = collision.contacts[0].normal;

        destroyTime = Time.time + TimeToDestroy;
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Armor") && collision.collider.gameObject.layer != LayerMask.NameToLayer("AdditionalArmor"))
        {
            Destroy(gameObject);
            return;
        }

        Instantiate(sparks, gameObject.transform.position, gameObject.transform.rotation);

        int armorThickness = collision.gameObject.GetComponent<Armor>().Thickness;

        if (myVelocity.normalized.z == 0)
        {
            myVelocity = transform.forward * speed;
        }

        float collisionAngle = Mathf.Abs(90 - (Vector3.Angle(myVelocity.normalized, normal)));
        collisionAngle = 90 - collisionAngle;

        float armor = Mathf.Abs(collision.gameObject.GetComponent<Armor>().Thickness / Mathf.Cos(collisionAngle * Mathf.PI / 180));

        ArmorPenetration = (int)(ArmorPenetration * Mathf.Cos(collisionAngle * Mathf.PI / 180));

        Debug.Log("Collision Angle:" + collisionAngle + "  AP: " + ArmorPenetration + "  Armor: " + (int)Mathf.Round(armor));

        if (collisionAngle > reboundAngle || richochet)
        {
            var shellSpeed = myVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(myVelocity.normalized, normal);
            rb.velocity = direction * Mathf.Max(shellSpeed, 30f);
            txt.text += ricochet;
            txt.color = Color.yellow;
        }
        else if (collisionAngle < reboundAngle && ArmorPenetration > (int)Mathf.Round(armor))
        {

            Debug.Log(Mathf.Cos(collisionAngle * Mathf.PI / 180));

            txt.text += breaking + (int)Mathf.Round(armor) + "  " + collisionAngle;
            txt.color = Color.green;
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Armor"))
            {
                GameObject NewSplinter = Instantiate(Splinter, gameObject.transform.position, gameObject.transform.rotation);
                NewSplinter.transform.localScale = transform.localScale;
                Splinter s = NewSplinter.GetComponent<Splinter>();
                s.speed = speed / 2;
                BoxCollider bc = GetComponent<BoxCollider>();
                BoxCollider sbc = NewSplinter.GetComponent<BoxCollider>();
                sbc.size = bc.size;
            }

            else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AdditionalArmor"))
            {
                GameObject ShellType1 = Instantiate(ShellType, gameObject.transform.position, gameObject.transform.rotation);
                Shell s = ShellType1.GetComponent<Shell>();
                s.calibre = calibre;
                s.mass = mass;
                s.ArmorPenetration = ArmorPenetration - (int)Mathf.Round(armor);
                s.reboundAngle = reboundAngle;
                s.speed = speed / 2;
                s.sparks = sparks;
                ShellType1.transform.localScale = transform.localScale;
                BoxCollider bc = GetComponent<BoxCollider>();
                BoxCollider sbc = ShellType1.GetComponent<BoxCollider>();
                sbc.size = bc.size;
            }

            Destroy(gameObject);

        }

        else if (collisionAngle < reboundAngle && ArmorPenetration < (int)Mathf.Round(armor))
        {
            txt.text += noBreaking;
            txt.color = Color.red;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!collision)
        {
            myVelocity = rb.velocity;
        }
        if (Time.time > destroyTime && collision)
        {
            Destroy(gameObject);
        }
    }
}
