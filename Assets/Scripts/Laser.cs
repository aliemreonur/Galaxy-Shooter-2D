using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _laserSpeed = 7f;
    private bool _isEnemyLaser = false;
    private bool _enemyReverseShot = false;
    // Start is called before the first frame update
  
    public bool isEnemyLaser
    {
        get
        {
            return _isEnemyLaser;
        }
    }

    public bool EnemyReverseShot
    {
        get
        {
            return _enemyReverseShot;
        }
        set
        {
            _enemyReverseShot = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private  void MoveDown()
    {
        if(_enemyReverseShot)
        {
            _laserSpeed = -7f;
        }
        else
        {
            _laserSpeed = 7f;
        }
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y > 10f)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemy()
    {
        _isEnemyLaser = true;
        transform.tag = "EnemyLaser";
    }

    public void EnemyShotUp()
    {
        StartCoroutine(EnemyReverseShotRoutine());
    }

  IEnumerator EnemyReverseShotRoutine()
    {
        _enemyReverseShot = true;
        yield return new WaitForSeconds(0.5f);
        _enemyReverseShot = false;
        yield return new WaitForSeconds(0.5f);
    }


}
