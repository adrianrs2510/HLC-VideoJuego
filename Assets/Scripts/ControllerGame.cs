using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerGames : MonoBehaviour
{
    private static ControllerGames instance;

    [SerializeField] private GameObject[] point_control;

    [SerializeField] private GameObject player;

    private int indexPoint_control;

    private void Awake()
    {
        instance = this;
        point_control = GameObject.FindGameObjectsWithTag("Point_control");
        indexPoint_control = PlayerPrefs.GetInt("index_point");
        Instantiate(player, point_control[indexPoint_control].transform.position, Quaternion.identity);
    }
    public void LastPoint_control(GameObject point)
    {
        for(int i = 0; i < point_control.Length; i++)
        {
            if(point_control[i] == point)
            {
                PlayerPrefs.SetInt("index_point", i);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown("e"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
