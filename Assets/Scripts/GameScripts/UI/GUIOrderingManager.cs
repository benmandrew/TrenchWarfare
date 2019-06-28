using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIOrderingManager : MonoBehaviour {

    public bool isActive;
    public float totalDisabledTime;
    private Image overlay;
    public GameObject underButton;

    private bool isAbove;

	// Use this for initialization
	void Start () {
        overlay = gameObject.GetComponent<Image>();
        isAbove = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            overlay.fillAmount -= 1.0f / totalDisabledTime * Time.deltaTime;
            if (!isAbove)
            {
                isAbove = true;
                shiftOrderInHierarchy(1);
            }
        }
        if (overlay.fillAmount <= 0)
        {
            isActive = false;
            isAbove = false;
            overlay.fillAmount = 1;
            shiftOrderInHierarchy(-1);
        }
    }

    public void resetAsActive()
    {
        setActive();
        overlay.fillAmount = 1;
    }

    public void setActive()
    {
        isActive = true;
    }

    void shiftOrderInHierarchy(int delta)
    {
        int index = transform.GetSiblingIndex();
        transform.SetSiblingIndex(index + delta);
    }
}
