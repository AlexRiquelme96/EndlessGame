using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUImanager : MonoBehaviour
{
    public Image hpBar;
    public Text hpText;
    // Start is called before the first frame update
    void Start()
    {
        UnidadesScript unidadesScript = GetComponent<UnidadesScript>();
        this.hpBar.rectTransform.localScale = new Vector3(1, (float)unidadesScript.actualHP1 / unidadesScript.maxHP1, 1);
        this.hpText.text = unidadesScript.actualHP1 + "/" + unidadesScript.maxHP1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateHpBar(float actualHP1, float maxHP1)
    {
        this.hpBar.rectTransform.localScale = new Vector3(1, actualHP1/maxHP1, 1);
        this.hpText.text = actualHP1 + "/" + maxHP1;
    }
}
