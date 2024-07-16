using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI magazineSizeText;
    [SerializeField] private TextMeshProUGUI magazineCountText;
    // Start is called before the first frame update

    public void UpdateInfo(Sprite weaponIcon, int magazineSize, int magazineCount)
    {
        icon.sprite = weaponIcon;
        magazineSizeText.text = $"{magazineSize}";
        int magazineCountAmount = magazineSize * magazineCount;
        magazineCountText.text = $"{magazineCountAmount}";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
