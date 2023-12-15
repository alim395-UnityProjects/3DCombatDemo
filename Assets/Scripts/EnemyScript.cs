using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    int health;
    bool attackable;

    void Start()
    {
        health = 3;
        attackable = true;
        GetComponent<Animator>().SetBool("isCombat", true);
    }

    void Update()
    {

    }

    public bool IsAttackable()
    {
        return attackable;
    }
}
