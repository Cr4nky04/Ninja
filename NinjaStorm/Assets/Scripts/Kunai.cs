using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject Hit_VFX;
    // Start is called before the first frame update
    void Start()
    {
        Oninit();
    }

    public void Oninit()
    {
        rb.velocity = transform.right * 10f;
        Invoke(nameof(OnDespawn), 1.5f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Instantiate(Hit_VFX, transform.position, transform.rotation);
            collision.GetComponent<Character>().OnHit(30f);
            OnDespawn();
        }
    }
}
