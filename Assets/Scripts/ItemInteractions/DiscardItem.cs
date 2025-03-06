using UnityEngine;

public class DiscardItem 
{
    private GameObject _currentItem;

    private const float ThrowForce = 40f;

    public DiscardItem(DiscardItemButton discardItemButton)
    {
        discardItemButton.DropItem += DropItem;
        discardItemButton.SetDroppingItem += GetItem;
    }

    public void GetItem(GameObject item)
    {
        _currentItem = item;
    }

    public void DropItem()
    {
        if (_currentItem == null)
            return;

        Rigidbody rbItem = _currentItem.GetComponent<Rigidbody>();
        MeshCollider colliderItem = _currentItem.GetComponent<MeshCollider>();

        rbItem.isKinematic = false;
        colliderItem.enabled = true;

        _currentItem.transform.SetParent(null);

        rbItem.AddForce(rbItem.transform.forward * ThrowForce, ForceMode.Impulse);

        _currentItem = null;
    }
}
