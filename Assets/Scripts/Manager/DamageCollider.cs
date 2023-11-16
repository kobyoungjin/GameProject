using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Animator animator;
    Troll troll;
    Boss boss;
 
    private void Start()
    {
        animator = transform.GetComponentInParent<Animator>();
        boss = transform.GetComponent<Boss>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Melee"))
        {
            if (this.gameObject.transform.root.name == "Ʈ��")
            {
                troll.Damaged(30);
                animator.SetTrigger("hit1");
            }
            else if (this.gameObject.transform.root.name == "Ÿ��ź")
                boss.Damaged(30);
            
        }
    }
}
