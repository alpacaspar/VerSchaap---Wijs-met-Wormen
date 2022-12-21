using UnityEngine;

public class CronScheduler : MonoBehaviour
{
    public static CronScheduler instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    ///     Fires method immedetialy and keeps repeating every x-seconds
    ///     Sample method:
    ///     CronScheduler.instance.SetRepeat("MyMethod", 2.5f);
    /// </summary>
    /// <param name="methodName">
    ///     Name of the method that should be repeated
    /// </param>
    /// <param name="interval">
    ///     Amount of time in seconds between every time that the method is fired
    /// </param>
    public void SetRepeat(MonoBehaviour reference, string methodName, float interval)
    {
        reference.InvokeRepeating(methodName, 0, interval);
    }
}
