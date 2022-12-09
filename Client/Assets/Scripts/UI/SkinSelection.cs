using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelection : MonoBehaviour
{
    public static int skinIndex = 0;

    [SerializeField] private GameObject skin0, skin1;
    [SerializeField] private GameObject _playerPreview;
    public static SkinSelection instance;

    private void Start()
    {
        instance = this;
    }

    public void SelectedSkin(int index)
    {
        //sets skinIndex to be the passed in int index from buttons its connected to
        skinIndex = index;
        //Update Player Skin
        UpdatePreviewSkin();
    }

    public void UpdatePreviewSkin()
    {
        //checks skinIndex value
        switch (skinIndex)
        {
            case 0: //case 0
                //Destroys all gameObjects under the playerPreview gameObject. Making way for the replacement skin
                foreach(Transform t in _playerPreview.transform)
                {
                    Destroy(t.gameObject);
                }
                //Instantiates skin 0 at the set position + rotation and places it under playerPreview as the parent
                GameObject newModel = Instantiate(skin0, _playerPreview.transform.position, Quaternion.Euler(new Vector3(0, 180, 0)), _playerPreview.GetComponentInParent<Transform>());
                break;
            case 1: //case 1
                //Destroys all gameObjects under the playerPreview gameObject. Making way for the replacement skin
                foreach (Transform t in _playerPreview.transform)
                {
                    Destroy(t.gameObject);
                }
                //Instantiates skin 1 at the set position + rotation and places it under playerPreview as the parent
                GameObject newModel1 = Instantiate(skin1, _playerPreview.transform.position, Quaternion.Euler(new Vector3(0, 180, 0)), _playerPreview.GetComponentInParent<Transform>());
                break;
        }
    }
}
