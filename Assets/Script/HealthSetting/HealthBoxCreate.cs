using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxCreate : MonoBehaviour
{
    
    void Start()
    {

        StartCoroutine(CreateHealth());
    }
    int RandomKey;
    public GameObject HealthBox;
    public List<GameObject> HealthBoxPoint = new List<GameObject>();
    List<int> RandomKeyHold = new List<int>();

    IEnumerator CreateHealth()
    {
       
        while (true)
        {
            yield return new WaitForSeconds(5f);
            RandomKey = Random.Range(0, HealthBoxPoint.Count);
            if (!RandomKeyHold.Contains(RandomKey))
            {
                
              
              GameObject gameObject= Instantiate(HealthBox, HealthBoxPoint[RandomKey].transform.position, HealthBoxPoint[RandomKey].transform.rotation);

                RandomKeyHold.Add(RandomKey);
            }
            else if (RandomKeyHold.Count== HealthBoxPoint.Count)
            {
                
                break;
            }
            else
            {
               
                continue;
            }
           

        }
      
    }


}
