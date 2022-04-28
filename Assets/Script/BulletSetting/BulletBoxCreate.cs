using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoxCreate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(CreateBullet());
    }
    int RandomKey;
    public GameObject BulletBox;
    public List<GameObject> BulletBoxPoint = new List<GameObject>();
    List<int> RandomKeyHold = new List<int>();

    IEnumerator CreateBullet()
    {
       
        while (true)
        {
            yield return new WaitForSeconds(5f);
            RandomKey = Random.Range(0, BulletBoxPoint.Count);
            if (!RandomKeyHold.Contains(RandomKey))
            {
                
              
              GameObject gameObject= Instantiate(BulletBox, BulletBoxPoint[RandomKey].transform.position, BulletBoxPoint[RandomKey].transform.rotation);

                gameObject.transform.gameObject.GetComponentInChildren<BulletBox>().BulletBoxPoint = RandomKey;

                RandomKeyHold.Add(RandomKey);
            }
            else
            {
                RandomKey = Random.Range(0, BulletBoxPoint.Count);
                continue;
            }
           

        }
      
    }

   public void RemoveBulletBoxPoint(int value)
    {
        RandomKeyHold.Remove(value);
    }
 
}
