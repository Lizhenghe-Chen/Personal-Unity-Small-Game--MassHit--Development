using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCubeAction : MonoBehaviour
{
    public Material hittedObjectMaterial;
    public List<string> takeActionHitterList = new List<string>();
    private void OnCollisionEnter(Collision other)
    {
        if (takeActionHitterList.Contains(other.gameObject.tag))
        {
            GetComponent<MeshRenderer>().material = hittedObjectMaterial;
            GetComponent<Light>().enabled = true;
            GetComponent<Rigidbody>().useGravity = false;
            if (CharacterCtrl._CharacterCtrl.HitObjects.Contains(gameObject)) { return; }
            CharacterCtrl._CharacterCtrl.HitObjects.Add(gameObject);
            // CharacterCtrl._CharacterCtrl.Test.Enqueue(gameObject);
        }

    }
}
