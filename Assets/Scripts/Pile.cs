using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Pile : MonoBehaviour {
    public int index;
    public List<GameObject> sticksInPile;
    public Billboard countSign;
    public float searchRadiusMultiplier;
    private SphereCollider sph;
    public float radius;
    public float SearchTime;

    private float time;

	// Use this for initialization
	void Start () {
        sticksInPile = new List<GameObject>();

        countSign.gameObject.SetActive(false);

        StartCoroutine(SearchForSticks());

        time = Time.time;

        gameObject.tag = "Untagged";
    }

    private IEnumerator SearchForSticks()
    {
        bool enoughSticks = false;
        Collider[] objs = { null };

        while (Time.time - time < SearchTime)
        {
            objs = Physics.OverlapSphere(transform.position, radius);
            int countedNearSticks = 0;

            for (int i = 0; i < objs.Length; i++)
                if (objs[i].CompareTag("Item"))
                    countedNearSticks++;

            if (countedNearSticks > 1)
            {
                enoughSticks = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        if(enoughSticks)
        {
            tag = "Pile";
            countSign.gameObject.SetActive(true);

            for(int i = 0; i < objs.Length;i++)
            {
                if(objs[i].CompareTag("Item"))
                    AddStick(objs[i].gameObject);
            }
        }
        else
        {
            Debug.Log("Destroying the searcher");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        radius = ((sticksInPile.Count+1) * searchRadiusMultiplier);

        countSign.text.text = sticksInPile.Count + " STICKS";

        SearchForNearbyPiles();
    }

    private void SearchForNearbyPiles()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, radius + 0.25f);

        for(int i = 0; i < objs.Length;i++)
        {
            if (objs[i].gameObject == gameObject)
                continue;

            if (objs[i].GetComponent<Pile>() && objs[i].CompareTag("Untagged")) {
                Debug.Log("Destroying " + objs[i].name);
                Destroy(objs[i].gameObject);
            }
        }
    }

    public void AddStick(GameObject stick)
    {
        sticksInPile.Add(stick);
        stick.transform.parent = transform;
        stick.tag = "Untagged";
        Destroy(stick.GetComponent<Rigidbody>());
        Destroy(stick.GetComponent<Collider>());
    }

    public void SplitPile()
    {

    }

    void OnDrawGizmos()
    {
        Color c = Color.green;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
