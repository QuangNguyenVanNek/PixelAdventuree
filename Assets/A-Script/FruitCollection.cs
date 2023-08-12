using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCollection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator anim;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            anim.Play("Collected");
        }
    }
    
}