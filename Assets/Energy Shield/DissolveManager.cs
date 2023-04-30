using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveManager : MonoBehaviour
{
    public float duration = 1f;
    public Material material;
    public GameObject Shield_Distort;
    public AnimationCurve curve;

    private float timer = 0;
    private bool doAction = false, Show = false;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        Show = false;
        SetShield(Show);
    }
    private void OnDisable()
    {
        material.SetFloat("_DissolveThreshold", 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && doAction == false)
        {
            timer = 0;
            Show = !Show;
            SetShield(Show);
        }


        if (doAction)
        {
            if (Show)
            {
                Update_Show();
            }
            else
            {
                Update_Hide();
            }
        }
    }

    private void Update_Show()
    {
        timer += Time.deltaTime;
        float t = curve.Evaluate(Mathf.Clamp01(timer / duration));
        transform.localScale = Vector3.Lerp(Vector3.zero, 3 * Vector3.one, t + 0.5f);
        material.SetFloat("_DissolveThreshold", t);
        if (timer > duration)
        {
            Shield_Distort.SetActive(Show);
            doAction = false;
        }
    }

    private void Update_Hide()
    {
        timer += Time.deltaTime;
        Shield_Distort.SetActive(Show);
        float t = curve.Evaluate(1 - Mathf.Clamp01(timer / duration));
        transform.localScale = Vector3.Lerp(Vector3.zero, 3 * Vector3.one, t + 0.5f);
        material.SetFloat("_DissolveThreshold", t);
        if (timer > duration)
        {
            doAction = false;
        }
    }

    [ColorUsage(true, true)] public Color interactionColor;
    private void OnCollisionEnter(Collision other)
    {
        Renderer render = other.gameObject.GetComponent<Renderer>();
        if (render == null) return;
        interactionColor = render.material.GetColor("_EmissionColor");
        PlayerBrain.instance.shootEnergy -= 2;
        if (PlayerBrain.instance.shootEnergy <= 5)
        {
            Show = false;
            timer = 0;
            SetShield(Show);
        }
        //change the interactionColor intensity to 2
        interactionColor *= 5f;


        Shield.instance.AddInteractionData(other.contacts[0].point, interactionColor);
    }
    private void SetShield(bool show)
    {
        doAction = true;
        Show = show;

        col.enabled = Show;
    }
}