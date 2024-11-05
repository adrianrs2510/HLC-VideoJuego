using UnityEngine;

public class LoadState : MonoBehaviour
{
    public static bool load;
    public static LoadState loadState;

    public void Awake()
    {
        if (loadState == null)
        {
            loadState = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}