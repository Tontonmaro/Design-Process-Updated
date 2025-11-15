using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    public string name;
    public string subtitle;
    public string description;
    public float chosenPrice;
    public int quantity = 1;
    public string[] sizes;
    public float[] priceOptions;
    public string chosenSize;
    public Sprite thumbnail;
    public BlindBoxData blindBoxData;
}
