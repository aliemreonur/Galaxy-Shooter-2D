using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDetectvv : MonoBehaviour
{
    Laser _laser;
    Enemy2 enemy;
    // Start is called before the first frame update
    void Start()
    {
        //enemy = GetComponentInChildren<Enemy>();
        enemy = GetComponentInParent<Enemy2>().GetComponent<Enemy2>();
        if (enemy == null)
        {
            Debug.LogError("Enemy body could not get the enemy script from the parent");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // if(enemy.isDead) { Destroy(this.gameObject, 0.9f); }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            Debug.Log("Enemy Laser is approaching!"); // works fine
            //tell the enemy to move to one side. 
            enemy.EvadeShot();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.EvadeOver();
    }
}