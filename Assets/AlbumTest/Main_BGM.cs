using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_BGM : MonoBehaviour {
    IEnumerator BGMRoutine;

    private AudioSource _Audio_BGM;

    private void Awake()
    {
        _Audio_BGM = GetComponent<AudioSource>();
    }

    public void BGM_In()
    {
        if (BGMRoutine != null) StopCoroutine(BGMRoutine);
        BGMRoutine = Routine_BGM_In();
        StartCoroutine(BGMRoutine);
    }

    public void BGM_Out()
    {
        if (BGMRoutine != null) StopCoroutine(BGMRoutine);
        BGMRoutine = Routine_BGM_Out();
        StartCoroutine(Routine_BGM_Out());
    }

    private IEnumerator Routine_BGM_In()
    {
        _Audio_BGM.Play();
        for (float t = 0.0f; t < 2.0f; t += Time.deltaTime)
        {
            _Audio_BGM.volume = Mathf.Lerp(0.0f, 1.0f, t / 2.0f);
            yield return null;
        }
    }

    private IEnumerator Routine_BGM_Out()
    {
        for (float t = 0.0f; t < 2.0f; t += Time.deltaTime)
        {
            _Audio_BGM.volume = Mathf.Lerp(1.0f, 0.0f, t / 2.0f);
            yield return null;
        }
        _Audio_BGM.Stop();
    }
}
