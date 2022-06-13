using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHoleDestory : MonoBehaviour
{
    private Vector3 objectDestory = new(0.2f, 0.2f, 0.2f);
    void OnTriggerStay(Collider other)
    {

        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (!rigidbody || rigidbody.CompareTag("BlackHole") || rigidbody.gameObject.layer == 1) { return; }
        if (other.transform.localScale.x <= 0.01f)
        {
            if (other.name == "Player")
            {
                Destroy(other.transform.parent.parent.gameObject);
                // Destroy(CharacterCtrl._CharacterCtrl.transform.parent.parent.gameObject);
                CenterRotate.shootEnergy = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
            Destroy(other.gameObject);
        }
        else
        {
            if (other.name == "Player") { other.transform.localScale -= objectDestory; }
            else
                other.transform.localScale -= objectDestory;
            rigidbody.mass /= 2;
        }
    }
}
