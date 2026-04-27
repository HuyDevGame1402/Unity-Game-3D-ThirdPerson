using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public TextMeshProUGUI ammunitionText;
    public TextMeshProUGUI magText;

    public static AmmoCount occurrence;

    private void Awake()
    {
        occurrence = this;
    }

    public void UpdateAmmoText(int presenAmmunition)
    {
        ammunitionText.text =  "AMMO. " + presenAmmunition.ToString();
    }
    public void UpdateMagText(int mag)
    {
        magText.text = "MAGAZINES. " + mag.ToString();
    }
}
