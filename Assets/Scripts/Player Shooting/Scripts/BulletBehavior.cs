using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float normalBulletSpeed = 3f;
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private LayerMask whatDestroysBullet;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDestroyTime(); //Destroys the bullet after a certain period of time
        SetVelocity(); //Sets the speed of the bullet
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //is the collision within the whatDestroysBullet layerMask
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            //spawn particles

            //play sound fx

            //screen shake

            //damage 

            //destroy the bullet
            Destroy(gameObject);
        }
    }

    private void SetVelocity() 
    {
        rb.linearVelocity = transform.right * normalBulletSpeed; 
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

}
