using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : EnemyBase
{
    [SerializeField] private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnim();
    }

    public override void UpdateAnim()
    {
        //base.UpdateAnim();
        if (isDead)
        {
            anim.Play("BeeDead");
        }
    }
}
