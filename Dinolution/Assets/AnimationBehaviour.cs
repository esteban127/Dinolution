using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    [SerializeField] Sprite[] spriteStatesTest;
    SpriteRenderer render;

    private void Start()
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
