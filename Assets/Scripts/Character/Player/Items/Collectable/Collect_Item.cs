using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Item : MonoBehaviour
{
    [SerializeField] private Item item = null;
    private BoxCollider2D boxCollider = null;

    private void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player")){
            UI_NotificationManager.Instance.ChangePickUpText(item);
            UI_NotificationManager.Instance.pickUpText.gameObject.SetActive(true);
            //Inventory.Instance.CreateItem(item);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag.Equals("Player")){
            UI_NotificationManager.Instance.pickUpText.gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible() {
        boxCollider.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnBecameVisible() {
        boxCollider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
