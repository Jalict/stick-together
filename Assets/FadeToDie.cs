using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToDie : MonoBehaviour {
    public float timeToStartFade = 0f;
    public float timeToDeath = 1f;

    private Material material;
    private Color originalColor;
    private Color fadingColor;
    private float step;

	// Use this for initialization
	void Start () {
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;
        fadingColor = material.color;
        fadingColor.a = 0f;
        step = 0f;

        StartCoroutine(Die());
	}

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(timeToStartFade);

        while(step < 1f)
        {
            material.color = Color.Lerp(originalColor, fadingColor, step);

            step += (1/timeToDeath) * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
