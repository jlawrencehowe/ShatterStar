using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class waveBlast : EnemyBullet
    {

    bool hitPlayer = false;



	public new void InitStats(float speed, float damage, Quaternion rot, float bulletdecay, int pierceAmount, bool isExplosive, bool isHoming){
		SetSpeed (speed);
		SetDamage (damage);
		SetRotation (rot);
		SetBulletDecay (bulletdecay);
		SetExplosive (isExplosive);
		SetPierceAmount (pierceAmount);
		homing = isHoming;
		Destroy (gameObject, bulletDecay);
	}


    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !hitPlayer)
        {

            hitPlayer = true;
            col.GetComponent<PlayerController>().TakeDamage(damage);
            
        }

    }




}

