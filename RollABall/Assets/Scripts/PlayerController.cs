﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;               

    private Rigidbody rb;
    private int count;
    public LayerMask layerMask;
    private List<Vector3> targetStack;
    private bool isMoving;
    private Animator animator;

    public void Start() {
        rb = this.GetComponent<Rigidbody>();
        count = 0;
        countText.text = "Count: 0";
        targetStack = new List<Vector3>();
        isMoving = false;
        animator = this.GetComponent<Animator>();
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
        if (targetStack.Count > 0) {
            this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, targetStack[0], speed*Time.deltaTime);
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
