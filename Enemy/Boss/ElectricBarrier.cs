using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBarrier : MonoBehaviour
{
    protected GameController gc;
    private float timer;
    private bool knock = false;
    private float knockBackForce = 500f;

    public GameObject electricBarrier;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetLoc = gc.pc.transform.position;
        targetLoc.z = 0f;

        Vector3 objectPos = transform.position;
        targetLoc.x = targetLoc.x - objectPos.x;
        targetLoc.y = targetLoc.y - objectPos.y;
        float angle = Mathf.Atan2(targetLoc.y, targetLoc.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if(Time.timeScale != 0 && knock)
            timer -= Time.unscaledDeltaTime;
        if (knock && timer <= 0)
            FinishKnockBack();

    }



    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && !knock)
        {
            Debug.Log("what");
            KnockBackPlayer();
            
        }
    }
    

    private void KnockBackPlayer()
    {
        timer = 1f;
        knock = true;
        gc.pc.enabled = false;
        gc.pc.TakeDamage(25);
        gc.pc.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector3 direction = (gc.pc.transform.position - electricBarrier.transform.position);
        gc.pc.GetComponent<Rigidbody2D>().AddForce(direction * knockBackForce);
    }

    private void FinishKnockBack()
    {
        gc.pc.enabled = true;
        knock = false;
    }
}
