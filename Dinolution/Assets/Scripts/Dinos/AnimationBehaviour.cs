using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    [SerializeField] Sprite[] spriteStatesTest = null;
    SpriteRenderer render;
    int dinoStage = 0;
    public int DinoStage { set { dinoStage = value; } }
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }
    public void Courch()
    {
        render.sprite = spriteStatesTest[1];
    }
    public void Die()
    {
        render.sprite = spriteStatesTest[3];
    }
    public void Shoot()
    {
        render.sprite = spriteStatesTest[2];
    }
    public void ReturnToIdle()
    {
        render.sprite = spriteStatesTest[0];
    }
}
