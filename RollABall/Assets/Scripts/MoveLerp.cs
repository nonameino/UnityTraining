using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLerp : MonoBehaviour
{
    public GameObject target;
    public float duration = 1.0f;
    public float time = 0.0f;

    private float rate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 1.0f)
            time += Time.deltaTime;
        else
            time = 0.0f;

        this.transform.position = Vector3.Lerp(transform.position, target.transform.position, time);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, time);
    }
}
