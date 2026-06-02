using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitbox;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            Attack();
        }
    }

    void Attack()
    {
        hitbox.SetActive(true);
        Invoke("StopAttack", 0.3f);
    }

    void StopAttack()
    {
        hitbox.SetActive(false);
    }
}