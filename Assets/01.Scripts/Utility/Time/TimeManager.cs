using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour, IManager
{
    private bool isStoped;

    public void Stop(float time, float duration)
    {

        if (isStoped) return;

        StartCoroutine(StopCo(time, duration));

    }

    private IEnumerator StopCo(float time, float duration)
    {

        isStoped = true;

        Time.timeScale = time;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;

        isStoped = false;

    }

    public void UpdateState(GameState state)
    {

    }
}
