using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasingController : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject dieImage;
    [SerializeField] private Image fillHealthUi;
    private float currentHealth;
    private Vector2 moveDir;
    private float x, y;

    [Header("Body reference")]
    [SerializeField] Transform bodyTransform;

    [Header("Vfx")]
    [SerializeField] GameObject hitEffect;

    GasingMenager GM;

    [HideInInspector] public bool gasingDie = false;
    int immunCount = 1;

    [HideInInspector] public bool analogUse = false;
    [HideInInspector] public bool startSpin = false;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GasingMenager>();
        gasingDie = false;

        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        x = joystick.Horizontal;
        y = joystick.Vertical;

        if (x != 0 || y != 0)
            analogUse = true;
        else { analogUse = false; }

        if (startSpin)
            bodyTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        
        moveDir = new Vector2(x, y);
    }

    private void FixedUpdate()
    {
        if (gasingDie)
            return;

        rb.AddForce(moveDir * speed);
    }

    private void TakeDamage(Collider2D barrierCollider)
    {
        // Mengurangi HP gasing berdasarkan kecepatan relatif
        float damage = 10; // Atur angka sesuai kebutuhan

        // Mengurangi HP gasing sesuai dengan damage
        currentHealth -= damage;
        fillHealthUi.fillAmount = currentHealth / maxHealth;
        // Menggunakan Debug.Log untuk menampilkan informasi pengurangan HP
        Debug.Log("Gasing terkena Barrier! HP berkurang sebesar " + damage + ". HP saat ini: " + currentHealth);

        if (currentHealth <= 0)
        {
            // Gasing telah mati, lakukan tindakan yang sesuai (misalnya, mengakhiri permainan)
            Die();
        }
    }

    private void Die()
    {
        GameObject dieImageObj = Instantiate(dieImage, transform.position, Quaternion.identity);
        dieImageObj.transform.position = transform.position;

        gasingDie = true;

        Destroy(gameObject);
        Debug.Log("Gasing telah mati! Game Over.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (immunCount > 0)
        {
            immunCount--;
            return;
        }
            

        
        GameManager.instance.Shake(); 
        if (collision.gameObject.CompareTag("Barrier"))
        {
            //sound
            GameManager.instance.StartSfx(GameManager.instance.allSfx[0]);

            // Mendapatkan komponen Collider dari objek Barrier yang ditabrak
            Collider2D barrierCollider = collision.collider;

            // Mengurangi HP gasing
            TakeDamage(barrierCollider);

            GameObject effectTemp = Instantiate(hitEffect, CalculateMiddlePoint(this.transform.position, collision.transform.position), Quaternion.identity);
            effectTemp.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Gasing"))
        {
            //sound
            GameManager.instance.StartSfx(GameManager.instance.allSfx[2]);

            //GameManager.instance.StartParticleSystem(gameObject.transform);
            Instantiate(hitEffect, CalculateMiddlePoint(this.transform.position, collision.transform.position), Quaternion.identity);
        }
    }


    Vector2 CalculateMiddlePoint(Vector2 _from, Vector2 _target)
    {
        return (_from + _target) / 2f;
    }

}
