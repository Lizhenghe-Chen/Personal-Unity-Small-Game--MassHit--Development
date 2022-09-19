using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class AirCraftModeSwitch : MonoBehaviour
{
    [Header("For PlayerBrain PositionConstraint target")][SerializeField] private Transform PlayerBrain;
    [Header("For Player switch Positoin Rest")][SerializeField] private Transform Player, Plane;

    // [SerializeField] private bool isPlayer, isPlane;
    private void Update()
    {
        Player.position = PlayerBrain.position;
    }
    private void OnEnable()//when aircraft is enabled, 
    {
        Plane.forward = this.transform.parent.parent.GetComponent<Rigidbody>().velocity = Player.GetComponent<Rigidbody>().velocity;
        PlayerBrain.GetComponent<PositionConstraint>().SetSource(0, new ConstraintSource() { sourceTransform = this.transform, weight = 1 });
        Plane.position = Player.position;
    }
    private void OnDisable()//when aircraft is disabled, switch back to player
    {
        Player.GetComponent<Rigidbody>().velocity = this.transform.parent.parent.GetComponent<Rigidbody>().velocity;
        PlayerBrain.GetComponent<PositionConstraint>().SetSource(0, new ConstraintSource() { sourceTransform = Player, weight = 1 });
        Player.position = Plane.position;
    }
    void SetBrainPositionConstraint()
    {
        PlayerBrain.GetComponent<PositionConstraint>().SetSource(0, new ConstraintSource() { sourceTransform = Player, weight = 1 });
    }
    // void RestPosition()
    // {
    //     if (isPlayer) { Player.position = Plane.position; }
    //     if (isPlane) { Plane.position = Player.position; }
    // }
}
