using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour {

    [SerializeField]
    private float currentValue;
    [SerializeField]
    private float targetValue;
    [SerializeField]
    private float maxDelta;

    private void Start() {
        StartCoroutine(Cor());
    }

    IEnumerator Cor() {
        while (!Mathf.Approximately(currentValue, targetValue)) {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime / maxDelta);
            yield return null;
        }
    }
}
