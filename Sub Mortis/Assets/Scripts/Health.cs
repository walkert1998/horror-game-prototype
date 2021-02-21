using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.PostProcessing;
using UnityEngine.UI;

//[System.Serializable]
public class Health : MonoBehaviour
{
    public int currentHealth;
    public int startingHealth;
    [SerializeField]
    private int maxHealth = 100;
    //public Slider healthBar;
    public int bleedAmount = 0;
    public bool playerBleeding = false;
    public float bleedTime = 3.0f;
    //public HitScreen hitScreen;
    //public Image barImage;
    //AudioSource source;
    //public AudioClip heartbeatSound;
    //GameSettings gameSettings;
    public GameObject bloodSpawner;
    public GameObject bloodPool;
    // Start is called before the first frame update
    void Start()
    {
        //gameSettings = GetComponent<GameSettings>();
        //healthBar.value = currentHealth;
        //source = GetComponent<AudioSource>();
        if (startingHealth != maxHealth)
        {
            currentHealth = startingHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.L))
        //    DamageCharacter(1);
        //else if (Input.GetKeyDown(KeyCode.K))
        //    HealCharacter(1);
        //else if (Input.GetKeyDown(KeyCode.J))
        //    StartBleeding(1);

        //if (!gameSettings.GetHealthBar())
        //    healthBar.gameObject.SetActive(false);
        //else
        //    healthBar.gameObject.SetActive(true);

        //if (Time.time > bleedTime && playerBleeding)
        //{
        //    DamageCharacter(bleedAmount);
        //    //Instantiate(bloodPool, bloodSpawner.transform.position, Quaternion.identity);
        //    //source.PlayOneShot(heartbeatSound);
        //    bleedTime += 1.0f;
        //}
    //}

    public void DamageCharacter(int Amount)
    {
        if (currentHealth - Amount > 0)
            currentHealth -= Amount;
        else
            currentHealth = 0;
        Debug.Log("Character received " + Amount + " damage");
        //if (gameSettings.GetInteractionPrompts())
        //healthBar.value = currentHealth;
        //hitScreen.TookDamage();
    }

    public void HealCharacter(int Amount)
    {
        if (currentHealth + Amount < maxHealth)
            currentHealth += Amount;
        else
            currentHealth = maxHealth;
        //if (gameSettings.GetInteractionPrompts())
        //healthBar.value = currentHealth;
    }

    public IEnumerator HealCharacterOverTime(int Amount, float time)
    {
        int healPerSecond = Amount / (int)time;
        int seconds = 0;
        while (seconds < (int) time)
        {
            if (currentHealth + healPerSecond < maxHealth)
                currentHealth += healPerSecond;
            else
                currentHealth = maxHealth;
            //if (gameSettings.GetInteractionPrompts())
            //healthBar.value = currentHealth;
            seconds++;
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator DamageCharacterOverTime(int Amount, float time)
    {
        int damagePerSecond = Amount / (int)time;
        int seconds = 0;
        while (seconds < (int)time)
        {
            if (currentHealth - damagePerSecond > 0)
                currentHealth -= damagePerSecond;
            else
                currentHealth = 0;
            //if (gameSettings.GetInteractionPrompts())
            //healthBar.value = currentHealth;
            seconds++;
            yield return new WaitForSeconds(1);
        }
    }

    public void StopBleeding()
    {
        playerBleeding = false;
        bleedAmount = 0;
        //barImage.color = Color.white;
    }

    public void StartBleeding(int amount)
    {
        playerBleeding = true;
        //barImage.color = Color.red;
        bleedAmount = amount;
        bleedTime = Time.time + 3.0f;
    }
}
