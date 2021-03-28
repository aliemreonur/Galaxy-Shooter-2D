using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool _isLastShot = false;
    private int _shieldLives = 3;

    public bool isLastShot
    {
        get
        {
            return _isLastShot;
        }
        set
        {
            _isLastShot = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit1()
    {
        StartCoroutine(HitRoutine1());
    }

   

    IEnumerator HitRoutine1()
    {
        while(true)
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
      
        }
      
    }

}
