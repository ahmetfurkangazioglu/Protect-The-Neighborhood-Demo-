using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;



public class GameControl : MonoBehaviour
{
    public Camera MyCam;
    [Header("GunSettings")]
    int ActivePlace;
    public GameObject[] Guns;
    public int GunNumberAnimation=0;
    public AudioSource ChangeGunVoice;
    public GameObject Bomb;
    public GameObject BombPoint;

    [Header("NpcSettings")]
    public GameObject[] Enemys;
    public GameObject[] EnemyTargetPoint;
    public GameObject[] EnemyStartPoint;
    public int EnemyCount;
    int RemainingEnemys; //  kalan Düþman
    public TextMeshProUGUI RemainingEnemysText;

    [Header("HealthAndBomb")]
    public float Health;
    public Image HealthImage;

    public TextMeshProUGUI HealthBoxAmount;
    public TextMeshProUGUI BombBoxAmount;

    [Header("GameSettings")]
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public GameObject MenuPanel;
    public GameObject SettingsPanel;
    bool EscControl = false;
    bool EscControlFinish = true;
    public FirstPersonController MyCharacterScript;
    void Start()
    {
     
        RemainingEnemys = EnemyCount;
        RemainingEnemysText.text = RemainingEnemys.ToString();
        ActivePlace = 0;

        StartCoroutine(NpcSettings());

        StartSaveControl();
       
        BombBoxAmount.text = GetSaveBombBox().ToString();
        HealthBoxAmount.text = GetSaveHealthBox().ToString(); 
    }



    void Update()
    {
        InputSettings();
        
    }




    IEnumerator NpcSettings()
    {
        while (true)
        {
            if (EnemyCount > 0)
            {
                yield return new WaitForSeconds(3f);
                int EnemyNumber = Random.Range(0, Enemys.Length);
                int TargetNumber = Random.Range(0, EnemyTargetPoint.Length);
                int EnemyStartNumber = Random.Range(0, EnemyStartPoint.Length);

                GameObject obje = Instantiate(Enemys[EnemyNumber], EnemyStartPoint[EnemyStartNumber].transform.position, Quaternion.identity);
                GameObject Target = EnemyTargetPoint[TargetNumber];
                obje.transform.GetComponent<Enemy>().SetTarget(Target);
                EnemyCount--;
            }
            else
            {
                break;
            }
            
        }       
    }

    void InputSettings()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeGun(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeGun(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeGun(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeGun(3);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeGunWithQ();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (GetSaveBombBox()>0)
            {
                GameObject obje = Instantiate(Bomb, BombPoint.transform.position, BombPoint.transform.rotation);
                Rigidbody rg = obje.GetComponent<Rigidbody>();
                Vector3 Angle = Quaternion.AngleAxis(90, MyCam.transform.forward) * MyCam.transform.forward * 2f;
                rg.AddForce(Angle * 450f);
                SaveBombBox(GetSaveBombBox() - 1);
                BombBoxAmount.text = GetSaveBombBox().ToString();
            }

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (GetSaveHealthBox()>0 && Health<100)
            {
                HealthBox();
                SaveHealthBox(GetSaveHealthBox() - 1);
                HealthBoxAmount.text = GetSaveHealthBox().ToString();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Animator Anim = Guns[GunNumberAnimation].GetComponent<Animator>();
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                 Guns[GunNumberAnimation].GetComponent<Animator>().SetBool("WalkWithD",true);
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {           
                Guns[GunNumberAnimation].GetComponent<Animator>().SetBool("WalkWithD", false);
                     
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Animator Anim = Guns[GunNumberAnimation].GetComponent<Animator>();
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Guns[GunNumberAnimation].GetComponent<Animator>().SetBool("WalkWithA", true);
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Guns[GunNumberAnimation].GetComponent<Animator>().SetBool("WalkWithA", false);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscControlFinish)
            {
                if (!EscControl)
                {
                    MenuOpen();
                }
                else
                {
                    MenuClose();
                }

            }
        }
    }

    void ChangeGun(int GunNumber)
    {
        ChangeGunVoice.Play();
        foreach (GameObject gun in Guns)
        {     
            gun.SetActive(false);
        }
       
        Guns[GunNumber].SetActive(true);
       Guns[GunNumber].GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        Guns[GunNumber].GetComponent<Animator>().Play("ChangeGun", 0, 0f);
        ActivePlace = GunNumber;
        GunNumberAnimation = GunNumber;
    }

    void ChangeGunWithQ()
    {
        ChangeGunVoice.Play();
        ActivePlace++;
        if (ActivePlace<Guns.Length)
        {
            foreach (GameObject gun in Guns)
            {

                gun.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
                gun.GetComponent<Animator>().Play("ChangeGun", 0, 0f);
                gun.SetActive(false);
            }
            Guns[ActivePlace].SetActive(true);

            Guns[ActivePlace].GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
            Guns[ActivePlace].GetComponent<Animator>().Play("ChangeGun", 0, 0f);
        }
        else
        {
            foreach (GameObject gun in Guns)
            {
                gun.SetActive(false);
            }
            ActivePlace = 0;
            Guns[ActivePlace].SetActive(true);
   

        }
      
    }

    public void HealthControl(float damage)
    {
        Health -= damage;
        HealthImage.fillAmount = Health / 100;
        

        if (Health<=0)
        {
            GameOver();
        }
    }
    

    public void HealthBox()
    {
        Health = 100;

        HealthImage.fillAmount = Health / 100;
    }

    public void DeadEnemy()
    {
        RemainingEnemys--;
        RemainingEnemysText.text = RemainingEnemys.ToString();
        if (RemainingEnemys <= 0)
        {
            GameWon();
        }
    }
    void GameWon()
    {
        GameWinPanel.SetActive(true);
        MyCharacterScript.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EscControlFinish = false;
        Time.timeScale = 0;
    }

    void GameOver()
    {
        GameOverPanel.SetActive(true);
        MyCharacterScript.enabled=false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EscControlFinish = false;
        Time.timeScale = 0;
        
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void MenuOpen()
    {
        EscControl = true;
        MenuPanel.SetActive(true);
        MyCharacterScript.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        
    }
   public void MenuClose()
    {
        EscControl = false;
        MenuPanel.SetActive(false);
        MyCharacterScript.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

    }
    
   public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }

    public void RestartGame()
    {
        MyCharacterScript.enabled = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }


    
    void SaveBombBox(int value)
    {
        PlayerPrefs.SetInt("BombBoxAmount", value);
    }
    void SaveHealthBox(int value)
    {
        PlayerPrefs.SetInt("HealthBoxAmount", value);
    }
    int GetSaveBombBox()
    {
        return PlayerPrefs.GetInt("BombBoxAmount");
    }
    int GetSaveHealthBox()
    {
        return PlayerPrefs.GetInt("HealthBoxAmount");
    }
    
    void StartSaveControl()
    {
        if (!PlayerPrefs.HasKey("CheckSave"))
        {
            PlayerPrefs.SetString("CheckSave", "");
            PlayerPrefs.SetInt("Magnum_AllAmount", 200);
            PlayerPrefs.SetInt("Shotgun_AllAmount", 100);
            PlayerPrefs.SetInt("Sniper_AllAmount", 150);
            PlayerPrefs.SetInt("Rifle_AllAmount", 250);
            PlayerPrefs.SetInt("BombBoxAmount", 3);
            PlayerPrefs.SetInt("HealthBoxAmount", 3);
        }
      
    }
}
