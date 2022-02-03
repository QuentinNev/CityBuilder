using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeed : MonoBehaviour
{
    private void Awake()
    {
        timeScale = (int)Time.timeScale;
    }

    public enum Speed { Normal, Fast, Fastest }

    static int timeScale;
    public static int TimeScale
    {
        get
        {
            return timeScale;
        }
    }

    const int NORMAL_SPEED = 1;
    const int FAST_SPEED = 2;
    const int FASTEST_SPEED = 10;

    public static void SetGameSpeed(int speed)
    {
        switch ((Speed)speed)
        {
            case Speed.Normal:
                Time.timeScale = timeScale = NORMAL_SPEED;
                break;
            case Speed.Fast:
                Time.timeScale = timeScale = FAST_SPEED;
                break;
            case Speed.Fastest:
                Time.timeScale = timeScale = FASTEST_SPEED;
                break;

            default:
                Debug.Log("BRUH");
                break;
        }
    }
}
