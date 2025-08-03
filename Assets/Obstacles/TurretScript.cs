using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView; // this shot an error
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public GameObject Bullet;
    private bool CanShoot = true;
    public Transform SpawnPoint;

    public float Direction; // -1 = left 1 = right
    public float Speed;
    public float RateOfFire;
    // Update is called once per frame
    void Update()
    {
        if (CanShoot)
        {
            CanShoot = false;
            var ShotBullet = Instantiate(Bullet, SpawnPoint.transform.position, SpawnPoint.rotation, SpawnPoint);
            ShotBullet.GetComponent<BulletScript>().Direction = Direction;
            ShotBullet.GetComponent<BulletScript>().BulletSpeed = Speed;
            GetComponent<AudioSource>().Play();
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer ()
    {
        yield return new WaitForSeconds(RateOfFire);
        CanShoot = true;
    }
}
