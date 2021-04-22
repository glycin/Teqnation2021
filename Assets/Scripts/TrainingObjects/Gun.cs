using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    public Bullet Shoot(Transform parent, Shooter shooter)
    {
        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, parent);
        var bulletComp = bullet.GetComponent<Bullet>();
        bulletComp.Fly(shooter);
        var newRot = new Vector3(0f, Random.Range(-2.0f, 2.0f), Random.Range(-1.5f, 1.5f));
        transform.localRotation = Quaternion.Euler(newRot);
        return bulletComp;
    }
}
