using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public List<Attack_kind> attack_kind;
    float last_attack_time = 0;
    float last_comboEnd = 0;
    public int combocnt = 0;
    RigidbodyConstraints2D i;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        i = GetComponent<RigidbodyConstraints2D>();
    }

    void Update()
    {
        if(combocnt > attack_kind.Count-1)
        {
            combocnt = 0;
            anim.SetFloat("punch_speed", 1);
        }
        if(Input.GetMouseButtonDown(0) && !anim.GetBool("onattacking") && !anim.GetBool("on_dash"))
        {
            Attack_();
        }
        
        ExitAttack();
    }
    void Attack_()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
        i= RigidbodyConstraints2D.FreezePositionY;
        if(Time.time - last_comboEnd > 0.5f * attack_kind[combocnt].speed && combocnt <= attack_kind.Count-1)
        {
            CancelInvoke("EndCombo");

            if(Time.time - last_attack_time >= 0.2f * attack_kind[combocnt].speed)
            {
                anim.runtimeAnimatorController = attack_kind[combocnt].animator;
                // anim.speed = attack_kind[combocnt].speed;
                anim.SetFloat("punch_speed", attack_kind[combocnt].speed);
                anim.Play("Punchs",0,0);
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                combocnt++;
                Invoke("stop",0.2f);
                last_attack_time = Time.time;
                if(combocnt == attack_kind.Count)
                {
                    anim.SetBool("onLastattack", true);
                    combocnt = 0;
                }
                else
                {
                    anim.SetBool("onLastattack", false);
                }
                if(combocnt > attack_kind.Count-1)
                {
                    anim.SetBool("onLastattack", false);
                    anim.SetFloat("punch_speed", 1);
                    combocnt = 0;
                }
            }
        }
    }
    void stop()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    }
    void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            anim.SetFloat("punch_speed", 1);
            Invoke("EndCombo",1f);
            anim.SetBool("onLastattack", false);
        }
    }
    void EndCombo()
    {
        combocnt = 0;
        i= RigidbodyConstraints2D.FreezePosition;
        last_comboEnd = Time.time;
        anim.SetFloat("punch_speed", 1);
    }
}