using UnityEngine;

public class DiscardItem 
{
    private GameObject _currentItem;

    public void GetItem(GameObject item)
    {
        Debug.Log("DiscardItem / GetItem");
        _currentItem = item;
    }

    public void DropItem()
    {
        if (_currentItem == null)
            return;

        Debug.Log("DiscardItem / DropItem");
        Rigidbody rbItem = _currentItem.GetComponent<Rigidbody>();

        rbItem.isKinematic = false;
        rbItem.AddForce(rbItem.transform.forward, ForceMode.Impulse);

        _currentItem = null;
    }
}
