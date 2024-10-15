using UnityEngine;

public class SwitchV2 : MonoBehaviour
{   
    [SerializeField] private bool active = false;
    [SerializeField] private bool dentro;

    [Header("Sound")]

	[SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip switchSound;



    private void Update()
    {
        if (dentro && Input.GetKeyDown(KeyCode.E))
        {
            active = !active; // Cambiar el estado del interruptor al presionar la tecla E
            audioSource.PlayOneShot(switchSound);
        }
    }

    public bool GetActive()
    {
        return active;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        dentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
        dentro = true;
        }
        dentro = false;
    }
}
