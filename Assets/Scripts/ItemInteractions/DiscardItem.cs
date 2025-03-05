using UnityEngine;

public class DiscardItem 
{
    private const float ThrowForce = 20f;

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
        MeshCollider colliderItem = _currentItem.GetComponent<MeshCollider>();

        rbItem.isKinematic = false;
        colliderItem.enabled = true;

        _currentItem.transform.SetParent(null);

        rbItem.AddForce(rbItem.transform.forward * ThrowForce, ForceMode.Impulse);

        _currentItem = null;
    }
}
