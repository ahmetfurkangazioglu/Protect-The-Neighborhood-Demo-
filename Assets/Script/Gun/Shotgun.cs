using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour
{

    Animator animator;

    [Header("Settings")]
    public bool FireControl;
    float inFireFrequency;//ates etme sýklýgý
    public float outFireFrequenc; //ates etme sýklýk
    public float Range;
    public GameObject BulletBoxCreate;

    [Header("Voices")]
    public AudioSource FireVoice;
    public AudioSource[] ChangeMagazin;
    public AudioSource EmptyMagVoice;
    public AudioSource BulletBoxVoice;

    [Header("Effects")]
    public ParticleSystem FireEffect;
    public ParticleSystem BulletTrack; //kursun izi
    public ParticleSystem BloodEffect;

    [Header("Other")]
    public Camera myCamera;
    public string Pref;
    public float TremorTime; //titreme
    public float CamTremorMagnitude; //þidet

    [Header("Gun Settings")]
    public bool isThereEmptyBullet;
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
    public GameObject BulletPoint2;

    [Header("ZoomSettings")]
    public GameObject Cross;
    public GameObject SniperCross;
    public float MyCamfieldOfView;

  

    void Start()
    {
       
        if (!PlayerPrefs.HasKey("CheckSaveMag"+Pref))
        {
            PlayerPrefs.SetString("CheckSaveMag"+Pref, "");
            PlayerPrefs.SetInt(Pref + "_RemainingBullet", MagCapacity);
        }

        MyCamfieldOfView = myCamera.fieldOfView;
        RemainingBullets = PlayerPrefs.GetInt(Pref + "_RemainingBullet");
        AllBulletAmount = PlayerPrefs.GetInt(Pref + "_AllAmount");
        BulletWriteText();
        animator = gameObject.GetComponent<Animator>();
    }


   
    void Update()
    {
     

            if (Input.GetKey(KeyCode.Mouse0))
        {
            if (FireControl && Time.time > inFireFrequency && RemainingBullets != 0)
            {
                StartCoroutine( CamTremor(TremorTime, CamTremorMagnitude));
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
    }


    IEnumerator CamTremor(float tremortime, float Magnitude ) 
    {

        Vector3 OriginalCamPoz = myCamera.transform.localPosition;
        float elapsedtime = 0.0f;//geçen zaman
        while (elapsedtime< tremortime)
        {
            float x = Random.Range(-1f, 1) * Magnitude;
            myCamera.transform.localPosition = new Vector3(x, OriginalCamPoz.y, OriginalCamPoz.z);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        myCamera.transform.localPosition = OriginalCamPoz;
    }


    void GunSettingsControl()
    {
        myCamera.fieldOfView = MyCamfieldOfView;
        Cross.SetActive(true);
        SniperCross.SetActive(false);
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

        Instantiate(Bullet, BulletPoint.transform.position, BulletPoint.transform.rotation);
        Instantiate(Bullet, BulletPoint2.transform.position, BulletPoint2.transform.rotation);

        if (isThereEmptyBullet)
        {
            GameObject obje = Instantiate(EmptyBullet, EmptyBulletPoint.transform.position, EmptyBulletPoint.transform.rotation);
            Rigidbody rg = obje.GetComponent<Rigidbody>();
            rg.AddRelativeForce(new Vector3(-10f, 1f, 0f) * 20f);

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
               
                PlayerPrefs.SetInt("Sniper_AllAmount", PlayerPrefs.GetInt("Sniper_AllAmount")+ GunAmount);
                break;

            case "Rifle":
               
                AllBulletAmount += GunAmount;
                PlayerPrefs.SetInt(Pref + "_AllAmount", AllBulletAmount);
                break;
        }
        AllBullet_Amount.text = AllBulletAmount.ToString();
    }

    public void PlayMag(int id)
    {
        ChangeMagazin[id].Play();
    }

    public void BulletWriteText()
    {
        AllBullet_Amount.text = AllBulletAmount.ToString();
        Remaining_Bullet.text = RemainingBullets.ToString();
    }
}
