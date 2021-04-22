using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Animator shooterAnimator;
    private Bullet bullet;

    public NeoAgent neo;
    public Gun gun;

    public Vector3 GunPosition { get { return gun.transform.position; } }

    void Awake()
    {
        shooterAnimator = GetComponent<Animator>();
    }

    public void Reset()
    {
        if(bullet != null)
        {
            Destroy(bullet.gameObject);
            bullet = null;
        }
        shooterAnimator.Play("Pistol-Attack-L1");
    }

    public void Shoot(int shooting)
    {
        bullet = gun.Shoot(transform.parent, this);
    }

    public Bullet GetBullets()
    {
        return bullet;
    }

    public void HitWall()
    {
        neo.AddReward(10f);
        neo.EndEpisode();
    }

    public void HitAgent()
    {
        neo.AddReward(-10f);
        neo.EndEpisode();
    }
}
