using System.Collections;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
    public int destroyTime = 3;
    public BulletPooling bulletPoolManager;// can also just use the 'bulletPool' in the BulletPooling.cs
  
    private void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.CompareTag("Player")) //do somthing 
        StartCoroutine(KillBullet(gameObject, new WaitForSeconds(destroyTime)));
    }
    public IEnumerator KillBullet(GameObject bullet, WaitForSeconds hide_DestroyTime)
    {
        yield return hide_DestroyTime;

        if (bulletPoolManager.isPooling)
        { //check if the bullet is still active
            if (bullet.activeSelf) bulletPoolManager.bulletPool.Release(bullet);
        }
        else Destroy(bullet);
    }
}
