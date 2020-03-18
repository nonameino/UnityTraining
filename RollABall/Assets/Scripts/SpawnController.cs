using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject collector;
    public GameObject spawnObject;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask)) {
                GameObject game = Instantiate(spawnObject, hit.point, Quaternion.identity);
                game.transform.parent = collector.transform;
                game.SetActive(true);
            }
        }
    }
}
