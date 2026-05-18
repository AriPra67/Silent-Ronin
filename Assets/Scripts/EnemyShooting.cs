using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletpos;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2f)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, bulletpos.position, Quaternion.identity);
    }
}