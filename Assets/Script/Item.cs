using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Ammo, Coin, Grenade, Heart, Weapon };
    public ItemType item;

    public int index;

    private void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

}
