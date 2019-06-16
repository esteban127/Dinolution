using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    [SerializeField] Sprite[] spriteStatesTest = null;
    SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }
    public void Courch()
    {
        render.sprite = spriteStatesTest[1];
    }
    public void ReturnToIdle()
    {
        render.sprite = spriteStatesTest[0];
    }
}
