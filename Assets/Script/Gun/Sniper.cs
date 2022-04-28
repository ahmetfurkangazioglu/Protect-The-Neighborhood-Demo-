using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sniper : MonoBehaviour
{
  //  SerializeField

    Animator animator;

    [Header("Settings")]
    public bool FireControl;
    float inFireFrequency;//ates etme sýklýgý
    public float outFireFrequenc; //ates etme sýklýk
    public float Range;
    public GameObject BulletBoxCreate;

    [Header("Voices")]
    public AudioSource FireVoice;
    public AudioSource ChangeMagazin;
    public AudioSource ReloadAfterFireVoice;
    public AudioSource EmptyMagVoice;
    public AudioSource BulletBoxVoice;

    [Header("Effects")]
    public ParticleSystem FireEffect;
    public ParticleSystem BulletTrack; //kursun izi
    public ParticleSystem BloodEffect;

    [Header("Other")]
    public Camera myCamera;
    public string Pref;


    [Header("Gun Settings")]
    public int AllBulletAmount;//toplam mermi sayýsý
    public int MagCapacity; // sarjör kapasitesi
    public int RemainingBullets; // sarjörde kalan mermi
    public TextMeshProUGUI AllBullet_Amount;
    public TextMeshProUGUI Remaining_Bullet;
    public int Damage;
    public GameObject EmptyBulletPoint;
    public GameObject EmptyBullet;


    public GameObject Bullet;
    public GameObject BulletPoint;

    [Header("ZoomSettings")]
    public GameObject ZoomCross;
    public GameObject Cross;
    public float MyCamfieldOfView;
    public float ZoomfieldOfView;
    [HideInInspector] public bool ZommCrossControl = false;


    void Start()
    {
        
        if (!PlayerPrefs.HasKey("CheckSaveMag"+Pref))
        {
            PlayerPrefs.SetString("CheckSaveMag"+Pref, "");
            PlayerPrefs.SetInt(Pref + "_RemainingBullet", MagCapacity);
        }

        MyCamfieldOfView = myCamera.fieldOfView;

          RemainingBullets= PlayerPrefs.GetInt(Pref + "_RemainingBullet");
        AllBulletAmount = PlayerPrefs.GetInt(Pref + "_AllAmount");
        BulletWriteText();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     

            if (Input.GetKey(KeyCode.Mouse0))
        {
            if (FireControl && Time.time > inFireFrequency && RemainingBullets != 0)
            {
                Fire();
                inFireFrequency = Time.time + outFireFrequenc;
            }
            if (RemainingBullets <= 0)
            {
                EmptyMagVoice.Play();
            }

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (RemainingBullets < MagCapacity && AllBulletAmount != 0)
            {

                if (MagCapacity < AllBulletAmount || MagCapacity >= AllBulletAmount)
                {
                    animator.Play("ChangeMagazin");

                }
            }
           


        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeBullet();
           
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!ZommCrossControl)
            {
                animator.SetBool("Zoom", true);
            }
            if (ZommCrossControl)
            {
                GunSettingsControl();
              
            } 
          
        }
    }

    void ZoomAnimation()
    {        
            Cross.SetActive(false);
            ZoomCross.SetActive(true);
            ZommCrossControl = true;
           myCamera.cullingMask = ~(1 << 6);
           myCamera.fieldOfView = ZoomfieldOfView;

        
    }

    void GunSettingsControl()
    {
        animator.SetBool("Zoom", false);
        myCamera.cullingMask = -1;
        myCamera.fieldOfView = MyCamfieldOfView;
        Cross.SetActive(true);
        ZoomCross.SetActive(false);
        ZommCrossControl = false;     
        
    }

    public void DoReload()
    {
        if (RemainingBullets < MagCapacity && AllBulletAmount != 0)
        {
      
            if (MagCapacity < AllBulletAmount || MagCapacity >= AllBulletAmount)
            {
                BulletControl("ThereAreBullet");

            }
        }
        BulletWriteText();
    }

  


    void BulletControl(string bulletControl)
    {
        switch (bulletControl)
        {
            
            case "ThereAreBullet":

                int Value = AllBulletAmount + RemainingBullets;
                //16                //5
                if (Value <= MagCapacity)
                {
                    RemainingBullets = Value;
                    AllBulletAmount = 0;
                    PlayerPrefs.SetInt(Pref + "_RemainingBullet", RemainingBullets);
                    PlayerPrefs.SetInt(Pref + "_AllAmount", AllBulletAmount);

                }
                else
                {
                    int Bullet2 = MagCapacity - RemainingBullets;
                    AllBulletAmount = AllBulletAmount - Bullet2;
                    RemainingBullets = MagCapacity;
                    PlayerPrefs.SetInt(Pref + "_RemainingBullet", RemainingBullets);
                    PlayerPrefs.SetInt(Pref + "_AllAmount", AllBulletAmount);

                }

                break;     
        }

    }
    void Fire()
    {
        FireSettings();
        int layerMask = 1 << 7;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(myCamera.transform.position,myCamera.transform.forward,out hit,Range,layerMask))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Instantiate(BloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Enemy>().DamageControl(Damage);
            }
            else if (hit.transform.gameObject.CompareTag("Fall"))
            {
                Rigidbody rigidbody = hit.transform.gameObject.GetComponent<Rigidbody>();
                rigidbody.AddForce(-hit.normal *50f);
            }
            else
            {
                Instantiate(BulletTrack, hit.point, Quaternion.LookRotation(hit.normal));
            }
          
        }
        
    }

    void FireSettings()
    {
        RemainingBullets--;
        Remaining_Bullet.text = RemainingBullets.ToString();
        PlayerPrefs.SetInt(Pref + "_RemainingBullet", RemainingBullets);

        GameObject obje = Instantiate(EmptyBullet, EmptyBulletPoint.transform.position, EmptyBulletPoint.transform.rotation);
        Rigidbody rg = obje.GetComponent<Rigidbody>();
        rg.AddRelativeForce(new Vector3(-10f, 1f, 0f) * 20f);

        if (!ZommCrossControl)
        {
            Instantiate(Bullet, BulletPoint.transform.position, BulletPoint.transform.rotation);
        }
       

        FireVoice.Play();
        FireEffect.Play();
        animator.Play("Fire");
    }

    void TakeBullet()
    {
        RaycastHit hit;
        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 3.5f))
        {
            if (hit.transform.gameObject.CompareTag("BulletBox"))
            {
                WhichBullet(hit.transform.gameObject.GetComponent<BulletBox>().Gun_Name, hit.transform.gameObject.GetComponent<BulletBox>().BulletBox_Amount);
                
                Destroy(hit.transform.parent.gameObject);
                BulletBoxVoice.Play();

                BulletBoxCreate.GetComponent<BulletBoxCreate>().RemoveBulletBoxPoint(hit.transform.gameObject.GetComponent<BulletBox>().BulletBoxPoint);

            }
        }
    }

    void WhichBullet(string GunName, int GunAmount)
    {
        switch (GunName)
        {
            case "Magnum":

                PlayerPrefs.SetInt("Magnum_AllAmount", PlayerPrefs.GetInt("Magnum_AllAmount") + GunAmount);

                break;

            case "Shotgun":

                PlayerPrefs.SetInt("Shotgun_AllAmount", PlayerPrefs.GetInt("Shotgun_AllAmount") + GunAmount);
                break;

            case "Sniper":

                AllBulletAmount += GunAmount;
                PlayerPrefs.SetInt(Pref + "_AllAmount", AllBulletAmount);
                break;

            case "Rifle":

                PlayerPrefs.SetInt("Rifle_AllAmount", PlayerPrefs.GetInt("Rifle_AllAmount") + GunAmount);
                break;
        }
        AllBullet_Amount.text = AllBulletAmount.ToString();
    }

    public void ChangeMag()
    {
        ChangeMagazin.Play();
    }
    public void ReloadAfterFire()
    {
        ReloadAfterFireVoice.Play();
    }

    public void BulletWriteText()
    {
        AllBullet_Amount.text = AllBulletAmount.ToString();
        Remaining_Bullet.text = RemainingBullets.ToString();
    }
}
