using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationExampleEvents : MonoBehaviour
{
    public void DestroyAnimationObject()
    {
        Destroy(this.gameObject);//gameObject by itself means whatever this script is attached to. Not NEEDED here but good for learning
    }
}
