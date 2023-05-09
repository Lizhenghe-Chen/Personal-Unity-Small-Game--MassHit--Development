using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 
/// </summary>
public class TrajectoryProjection : MonoBehaviour
{
    public Scene simulateScene;
    public PhysicsScene physicsSimulateScene;
    [SerializeField] GameObject[] allObjects;
    private readonly Dictionary<Transform, Transform> notStaticObject = new Dictionary<Transform, Transform>();
    // Start is called before the first frame update
    void Awake()
    {
        CreatePhysicsScene();
    }
    private void Update()
    {
        foreach (var obj in notStaticObject)
        {
            obj.Key.position = obj.Value.position;
            obj.Key.rotation = obj.Value.rotation;
        }
    }
    void CreatePhysicsScene()
    {
        //below step is going to create a extra scene for physics simulation, and get the physics scene
        simulateScene = SceneManager.CreateScene("SimulateScene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsSimulateScene = simulateScene.GetPhysicsScene();
        // Get all the game objects that has any type of collider
        allObjects = FindObjectsOfType<GameObject>().Where(obj => obj.GetComponent<Collider>() != null).ToArray();

        // Loop through all the game objects, instantiate them in the physics scene
        foreach (GameObject obj in allObjects)
        {
            var fakeObj = Instantiate(obj, obj.transform.position, obj.transform.rotation);
            //if object is static, then add it to notStaticObject array
            if (obj.isStatic) notStaticObject.Add(obj.transform, fakeObj.transform);

            fakeObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(fakeObj, simulateScene);
        }
    }

}