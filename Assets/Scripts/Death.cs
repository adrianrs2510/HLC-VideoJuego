using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CombatPlayerV2 combatPlayerV2;

    [SerializeField] private GameDataControlerV2 gameDataControlerV2;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private HealthBar healthBar;
    
    public void Update()
    {
        if (combatPlayerV2.Isdead())
        {
            Dead();
        }
    }
    
    // Start is called before the first frame update
    public void Dead()
    {
        Time.timeScale = 0f;
        deathScreen.SetActive(true);
        combatPlayerV2.SetDead(false);
    }

    public void Checkpoint()
    {
        gameDataControlerV2.LoadData();
        Time.timeScale = 1f;
        combatPlayerV2.SetDead(false);
        deathScreen.SetActive(false);
        playerAnimator.SetBool("Death", false);
        playerAnimator.Play("Idle");
        combatPlayerV2.SetCurrentHealth(combatPlayerV2.GetMaxHealth());
        healthBar.SetHealth(combatPlayerV2.GetCurrentHealth());

    }
    public void MainMenu()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }
}
