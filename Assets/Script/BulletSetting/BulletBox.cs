using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBox : MonoBehaviour
{

    string[] GunName =
    {
         "Magnum",
         "Shotgun",
         "Sniper",
         "Rifle"

    };
    int[] BulletBoxAmount =
    {
        7,
        15,
        19,
        25
       
    };


    public List<Sprite> GunImages = new List<Sprite>();
    public Image GunImage;
    public string Gun_Name;
    public int BulletBox_Amount;
    public int BulletBoxPoint;
   
    // Start is called before the first frame update
    void Start()
    {
        int RandomKey = Random.Range(0, GunName.Length);
         Gun_Name = GunName[RandomKey];     
        BulletBox_Amount = BulletBoxAmount[Random.Range(0, BulletBoxAmount.Length )];

        GunImage.sprite = GunImages[RandomKey];

    }

    // Update is called once per frame
    void Update()
    {
     

    }
}
