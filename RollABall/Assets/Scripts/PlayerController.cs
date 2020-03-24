using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;   
    public ParticleSystem diePsPrefab;
    public ParticleSystem spawnPsPrefab;          

    private Rigidbody rb;
    private int count;
    public LayerMask layerMask;
    private List<Vector3> targetStack;
    private bool isMoving;
    private Animator animator;
    private bool stopMove;

    private ParticleSystem diePsInstance =  null;
    private ParticleSystem spawnPsInstance = null;

    public void Start() {
        rb = this.GetComponent<Rigidbody>();
        count = 0;
        countText.text = "Count: 0";
        targetStack = new List<Vector3>();
        isMoving = false;
        animator = this.GetComponent<Animator>();
        StartSpawn();
    }

    public void StartSpawn() {

        if (diePsInstance != null && diePsInstance.isPlaying) {
            diePsInstance.Stop();
            Debug.Log("Stop die");
        }

        if (spawnPsInstance == null) {
            spawnPsInstance = Instantiate(spawnPsPrefab ,Vector3.zero , Quaternion.identity);
            spawnPsInstance.transform.parent = transform.parent;
        }
        if (!spawnPsInstance.isPlaying) {
            spawnPsInstance.Play();
            Debug.Log("Start Spawn");
        }

        this.transform.parent.position = Vector3.zero;
        stopMove = true;
    }

    public void EndSpawn() {
        if (spawnPsInstance.isPlaying) {
            spawnPsInstance.Stop();
            Debug.Log("Stop Spawn");
        }

        stopMove = false;
        if (targetStack.Count > 0) {
            isMoving = true;
            animator.SetTrigger("Move");
        }

    }

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100, layerMask)) {

                //Add list target
                targetStack.Add(raycastHit.point);
                
                if(!isMoving) {
                    isMoving = true;
                    animator.SetTrigger("Move");
                }
            }
        }
    }

    public void FixedUpdate() {
        if (targetStack.Count > 0 && !stopMove) {
            this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, targetStack[0], speed*Time.deltaTime);
            this.transform.LookAt(targetStack[0]);
        }
    }

    protected void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.CompareTag("PickUp")) {
            //Update count
            count = count + 1;
            SetCountText(count);
            
            //Remove collider position
            targetStack.RemoveAt(GetIndexOfCollider(other));
            if (targetStack.Count == 0 && isMoving) {
                isMoving = false;
                animator.SetTrigger("Idle");
            }
            
            Animator pickUpAnimator = other.GetComponent<Animator>();
            if (pickUpAnimator.GetCurrentAnimatorClipInfo(1)[0].clip.name.Equals("PickUpColorRed")) {
                if (diePsInstance == null) {
                    diePsInstance = Instantiate(diePsPrefab, Vector3.zero, Quaternion.identity);
                    diePsInstance.transform.parent = transform.parent;
                }
                if (!diePsInstance.isPlaying) {
                    diePsPrefab.Play();
                    Debug.Log("Start Die");
                }

                animator.SetTrigger("Die");
                stopMove = true;
            }

            //Disactive collider object
            // other.transform.parent.gameObject.SetActive(false);
            SpawnController.Instance().ReturnPickUpToPool(other.gameObject);
        }
    }

    private int GetIndexOfCollider(Collider collider) {
        return targetStack.IndexOf(GetParentPositionOfCollider(collider));
    }

    private Vector3 GetParentPositionOfCollider(Collider collider) {
        return collider.transform.parent.transform.position;
    }

    private void SetCountText(int count) {
        countText.text = "Count: " + count.ToString();
    }
}
