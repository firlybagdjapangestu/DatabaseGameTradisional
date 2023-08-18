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

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        x = joystick.Horizontal;
        y = joystick.Vertical;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        moveDir = new Vector2(x, y);
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDir * speed);
    }

    private void TakeDamage(Collider2D barrierCollider)
    {
        // Mengurangi HP gasing berdasarkan kecepatan relatif
        float damage = 10; // Atur angka sesuai kebutuhan

        // Mengurangi HP gasing sesuai dengan damage
        currentHealth -= damage;
        fillHealthUi.fillAmount = currentHealth / 100;
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

        Destroy(gameObject);
        Debug.Log("Gasing telah mati! Game Over.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.StartSfx(GameManager.instance.allSfx[0]);
        GameManager.instance.Shake(); 
        if (collision.gameObject.CompareTag("Barrier"))
        {
            // Mendapatkan komponen Collider dari objek Barrier yang ditabrak
            Collider2D barrierCollider = collision.collider;

            // Mengurangi HP gasing
            TakeDamage(barrierCollider);
        }
        if (collision.gameObject.CompareTag("Gasing"))
        {
            GameManager.instance.StartParticleSystem(gameObject.transform);
        }
    }

}
