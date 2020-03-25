using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoroutinesController : MonoBehaviour
{
    public InputField task1Input;
    public InputField task2Input;
    public Text task1Text;
    public Text task2Text;
    public Text result;

    public bool canDoTaskParallel = false;
    public bool task1Done = false;
    public bool task2Done = false;

    private Coroutine task1;
    private Coroutine task2;

    // Start is called before the first frame update
    void Start()
    {
        task1Input.text = "10";
        task2Input.text = "20";
    }

    // Update is called once per frame
    void Update()
    {
        if (string.IsNullOrEmpty(result.text)) {
            if (canDoTaskParallel && task1Done && task2Done) {
                result.text = "All Done!";
            }
            else if (!canDoTaskParallel && (task1Done || task2Done)) {
                result.text = "Done!";
            }
        }
    }

    public void DoParallel() {
        canDoTaskParallel = true;
        result.text = "";
        task1Done = false;
        task2Done = false;
        task1 = StartCoroutine(DoTask1());
        task2 = StartCoroutine(DoTask2());
    }

    public void DoRace() {
        canDoTaskParallel = false;
        result.text = "";
        task1Done = false;
        task2Done = false;
        task1 = StartCoroutine(DoTask1());
        task2 = StartCoroutine(DoTask2());
    }

    IEnumerator DoTask1() {
        int size = int.Parse(task1Input.text);
        for(int i=1; i<=size; i++) {
            task1Text.text = i.ToString() + "/" + size.ToString();
            yield return new WaitForSeconds(0.5f);
        }
        task1Done = true;
        if (!canDoTaskParallel)
            StopCoroutine(task2);
    }

    IEnumerator DoTask2() {
        int size = int.Parse(task2Input.text);
        for(int i=1; i<=size; i++) {
            task2Text.text = i.ToString() + "/" + size.ToString();
            yield return new WaitForSeconds(0.5f);
        }
        task2Done = true;
        if (!canDoTaskParallel)
            StopCoroutine(task1);
    }
}
