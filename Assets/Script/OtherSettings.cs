using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OtherSettings : MonoBehaviour
{

    public TextMeshProUGUI HealthBoxAmount;
    public TextMeshProUGUI BombBoxAmount;

    private void Start()
    {
        
    }
 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("HealthBox"))
        {
            PlayerPrefs.SetInt("HealthBoxAmount", PlayerPrefs.GetInt("HealthBoxAmount") + 1);
            HealthBoxAmount.text = PlayerPrefs.GetInt("HealthBoxAmount").ToString();
            Destroy(other.transform.gameObject);            
        }
        
        if (other.gameObject.CompareTag("BombBox"))
        {
            PlayerPrefs.SetInt("BombBoxAmount", PlayerPrefs.GetInt("BombBoxAmount") + 1);
            BombBoxAmount.text = PlayerPrefs.GetInt("BombBoxAmount").ToString();
            Destroy(other.transform.gameObject);          
        }

    }


}
