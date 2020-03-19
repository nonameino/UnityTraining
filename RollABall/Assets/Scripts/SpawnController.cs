using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject spawnObject;
    public LayerMask layerMask;

    private static SpawnController instance;
    public static SpawnController Instance() {
        return instance;
    }

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        SpawnController.instance = this;
    }

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
                GameObject pickup = spawnObject.Spawn(hit.point);
                int colorID = (Random.Range(0, 3) % 3); //0:Red, 1:Green, 2:Blue
                pickup.GetComponentInChildren<Collider>().enabled = true;
                // pickup.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                pickup.GetComponentInChildren<Animator>().SetInteger("ColorID", colorID);
            }
        }
    }

    public void ReturnPickUpToPool(GameObject pickup) {
        pickup.GetComponent<Collider>().enabled = false;
        pickup.GetComponent<Animator>().SetTrigger("Die");
        // pickup.transform.parent.gameObject.Kill();
    }
}
