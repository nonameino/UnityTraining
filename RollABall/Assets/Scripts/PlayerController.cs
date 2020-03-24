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
    public int HP = 10;       

    private Rigidbody rb;
    private int count;
    public LayerMask layerMask;
    private List<Vector3> targetStack;
    private bool isMoving;
    private Animator animator;
    private bool stopMove;
    private int currentHP;

    private Image hpValueImg;
    private Text timer;
    private float time;

    private ParticleSystem diePsInstance =  null;
    private ParticleSystem spawnPsInstance = null;

    public void Start() {
        currentHP = HP;
        rb = this.GetComponent<Rigidbody>();
        count = 0;
        countText.text = "Count: 0";
        targetStack = new List<Vector3>();
        isMoving = false;
        animator = this.GetComponent<Animator>();
        StartSpawn();

        hpValueImg = GameObject.Find("HPValue").GetComponent<Image>();
        timer = GameObject.Find("Timer").GetComponent<Text>();
    }

    public void StartSpawn() {

        if (diePsInstance != null && diePsInstance.isPlaying) {
            diePsInstance.Stop();
            Debug.Log("Stop die");
        }

        if (spawnPsInstance == null) {
            spawnPsInstance = Instantiate(spawnPsPrefab ,Vector3.zero , Quaternion.identity);
            spawnPsInstance.transform.parent = transform.parent;
            spawnPsInstance.transform.localPosition = Vector3.zero;
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
        time = time + Time.deltaTime;
        timer.text = Mathf.Round(time / 60).ToString("00") + ":" + (time % 60).ToString("00");
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
                    diePsInstance = Instantiate(diePsPrefab, transform.parent.position, Quaternion.identity);
                    diePsInstance.transform.parent = transform.parent;
                    diePsInstance.transform.localPosition = Vector3.zero;
                }
                if (!diePsInstance.isPlaying) {
                    diePsInstance.Play();
                    Debug.Log("Start Die");
                }

                animator.SetTrigger("Die");
                stopMove = true;

                currentHP = currentHP - 1;
                hpValueImg.fillAmount = 1.0f * currentHP / HP;
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
