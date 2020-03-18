using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // public GameObject collector;
    public GameObject spawnObject;
    // public int spawnNumber;
    public LayerMask layerMask;

    private static SpawnController instance;
    public static SpawnController Instance() {
        return instance;
    }

    // private List<GameObject> pickupPool;
    // private List<GameObject> pickupList;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        SpawnController.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // pickupPool = new List<GameObject>();
        // pickupList = new List<GameObject>();

        // for(int i=0; i<spawnNumber; i++) {
        //     GameObject pickup = Instantiate(spawnObject, Vector3.zero, Quaternion.identity);
        //     pickup.SetActive(false);
        //     pickup.transform.parent = collector.transform;
        //     pickupPool.Add(pickup);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask)) {
                // GameObject game = Instantiate(spawnObject, hit.point, Quaternion.identity);
                // game.transform.parent = collector.transform;
                // game.SetActive(true);
                // GameObject pickup = null;
                // if (pickupPool.Count > 0) {
                //     pickup = pickupPool[0];
                //     pickupPool.RemoveAt(0);
                // }
                // else {
                //     pickup = Instantiate(spawnObject, Vector3.zero, Quaternion.identity);
                //     pickup.transform.parent = collector.transform;
                // }

                // pickup.transform.position = hit.point;
                // pickup.SetActive(true);
                spawnObject.Spawn(hit.point);
            }
        }
    }

    public void ReturnPickUpToPool(GameObject pickup) {
        // pickup.SetActive(false);
        // pickupList.Remove(pickup);
        // pickupPool.Add(pickup);
        pickup.transform.parent.gameObject.Kill();
    }
}
