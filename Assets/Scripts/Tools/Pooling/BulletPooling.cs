using System.Collections;
using UnityEngine;
using UnityEngine.Pool;//base on Stack
/// <summary>
///https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html
/// </summary>

public class BulletPooling : MonoBehaviour
{
    public bool isPooling = true;
    [SerializeField] Transform firePoint;
    [SerializeField] int bulletSpeed = 10;
    [SerializeField] GameObject bulletPrefab;
    [Tooltip("The default number of objects to have in the pool, when the space is not enough, the pool will auto expand(stack).")]
    [SerializeField] int defaultPoolSize = 100;
    [Tooltip("The maximum number of objects to have in the pool. 0 = no maximum.")]
    [SerializeField] int maxPoolSize = 1000;
    public ObjectPool<GameObject> bulletPool;
    [Header("Below For Debug Use:")]
    [SerializeField] private int activeCount, inactiveCount, totalCount;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new ObjectPool<GameObject>(OnCreatPoolItem, OnGetItemFromPool, OnReleaseItemFromPool, OnDestroyItemFromPool, true, defaultPoolSize, maxPoolSize);
    }
    private void Update()
    {
        activeCount = this.bulletPool.CountActive;
        inactiveCount = this.bulletPool.CountInactive;
        totalCount = this.bulletPool.CountAll;

        Shoot();
    }

    GameObject tempbullet;
    void Shoot()
    {
        //if use pooling, then use the Get() method to get a bullet from the pool, otherwise create a new one by Instantiate()
        tempbullet = isPooling ? bulletPool.Get() : //the Get() method will return an object from the pool, or create a new one if the pool is empty.
         Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, this.transform);
        if (!tempbullet) return;
        tempbullet.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        tempbullet.GetComponent<PoolBullet>().bulletPoolManager = this;
        StartCoroutine(KillBullet(tempbullet, new WaitForSeconds(8)));//kill the bullet whatever after 5 seconds
    }
    /// <summary>
    /// This method will be called when the bullet is hit something, or the time is up.
    /// </summary>
    public IEnumerator KillBullet(GameObject bullet, WaitForSeconds hide_DestroyTime)
    {
        yield return hide_DestroyTime;

        if (isPooling)
        { //check if the bullet is still active
            if (bullet && bullet.activeSelf) bulletPool.Release(bullet);
        }
        else Destroy(bullet);
    }

    /***Below are the callback methods for the ObjectPool***/

    /// <summary> 
    /// 在对象池中创建对象时调用; This method will be called when the the Get() method is first time called and there still have space in the pool.
    /// </summary>
    private GameObject OnCreatPoolItem()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, this.transform);
        bullet.SetActive(true);
        //Debug.Log("OnCreatPoolItem");
        return bullet;
    }
    /// <summary>
    /// 当对象池超过容量时，或者对象被销毁时调用，一般不会发生，除非调用了Clean 或者 Dispose 方法
    ///This method will be called when the pool is full, ot Clean() or Dispose() method is called.
    /// </summary>
    private void OnDestroyItemFromPool(GameObject obj)
    {
        Destroy(obj);
        //Debug.Log("OnDestroyItemFromPool");
    }
    /// <summary>
    /// 从对象池中获取对象时调用; This method will be called when the the Get() method is called.
    /// </summary>
    private void OnGetItemFromPool(GameObject bullet)
    {
        if (bullet) bullet.SetActive(true);
        //Debug.Log("OnGetItemFromPool");
    }
    /// <summary>
    /// 当对象放回对象池时调用; This method will be called when the the Release() method is called.
    /// </summary>
    private void OnReleaseItemFromPool(GameObject bullet)
    {
        bullet.SetActive(false);
        //Reset the bullet's position
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;
        // bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Debug.Log("OnReleaseItemFromPool");
    }
}
