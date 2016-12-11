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
        while (Time.time - time < SearchTime || sticksInPile.Count > 1)
        {
            Collider[] objs = Physics.OverlapSphere(transform.position, radius);

            for (int i = 0; i < objs.Length; i++)
                if (objs[i].CompareTag("Item"))
                {
                    if(!sticksInPile.Contains(objs[i].gameObject))
                    {
                        AddStick(objs[i].gameObject);
                    }
                }

            if (sticksInPile.Count > 1)
            {
                tag = "Pile";
                countSign.gameObject.SetActive(true);
            } else
            {
                tag = "Untagged";
                countSign.gameObject.SetActive(false);
            }

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Destroying the pile");
        Destroy(gameObject);
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

    public GameObject RemoveStick()
    {
        GameObject stick = sticksInPile[sticksInPile.Count - 1];
        sticksInPile.Remove(stick);
        stick.tag = "Item";
        stick.AddComponent<Rigidbody>();
        stick.AddComponent<BoxCollider>();
        return stick;
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
