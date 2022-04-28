using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoxCreate : MonoBehaviour
{
    
    void Start()
    {

        StartCoroutine(CreateBomb());
    }
    int RandomKey;
    public GameObject BombBox;
    public List<GameObject> BombBoxPoint = new List<GameObject>();
    List<int> RandomKeyHold = new List<int>();

    IEnumerator CreateBomb()
    {
       
        while (true)
        {
            yield return new WaitForSeconds(5f);
            RandomKey = Random.Range(0, BombBoxPoint.Count);
            if (!RandomKeyHold.Contains(RandomKey))
            {
                
              
              GameObject gameObject= Instantiate(BombBox, BombBoxPoint[RandomKey].transform.position, BombBoxPoint[RandomKey].transform.rotation);

                RandomKeyHold.Add(RandomKey);
            }
            else if (RandomKeyHold.Count== BombBoxPoint.Count)
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
