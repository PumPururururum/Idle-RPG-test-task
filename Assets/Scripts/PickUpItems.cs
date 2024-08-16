using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Vector3 touch = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            Collider2D hit = Physics2D.OverlapPoint(new Vector2(touch.x, touch.y));
            if (hit != null && hit.tag == "Item")   
            {
                //Debug.Log("hit " + hit.gameObject.name);
                Item item = hit.GetComponent<Item>();
                item.PickUpItem();

            }
        }

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(new Vector2(touch.x, touch.y));
            if (hit != null && hit.tag == "Item")
            {
                Item item = hit.GetComponent<Item>();
                item.PickUpItem();
               // Debug.Log("hit " + hit.gameObject.name);

            }
        }
#endif
    }
}
