using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissileExplosion : MonoBehaviour
{
    private float damage;
    private List<GameObject> enemies = new List<GameObject> { };
    private GameObject enemy;


    // Use this for initialization
    void Start()
    {
        damage = 50;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitStats(GameObject enemy, float damage)
    {
        SetDamage(damage);
        GetComponent<CircleCollider2D>().enabled = true;
        this.enemy = enemy;
    }

    public void SetDamage(float damage)
    {

        this.damage = damage;
    }

    public void AnimFinish()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player")
        {
            bool isNewEnemy = true;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == col.gameObject)
                {
                    isNewEnemy = false;
                    break;
                }
            }
            if (isNewEnemy)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    col.GetComponent<EnemyBase>().TakeDamage(damage);
                }
                else if(col.gameObject.tag == "Player")
                {
                    col.GetComponent<PlayerController>().TakeDamage(damage);
                }
                enemies.Add(col.gameObject);
            }

        }
    }
}
