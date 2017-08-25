using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Hit_Script : MonoBehaviour {
    [SerializeField]
    Texture2D DamageMap;
    Texture2D refMap;
    Collider2D myCollider;
    SpriteRenderer myRenderer;

    Sprite FixedDamageMap;
    private void Start()
    {
        refMap = new Texture2D(DamageMap.width, DamageMap.height);
        for (int i = 0; i < refMap.width; i++)
        {
            for (int j = 0; j < refMap.height; j++)
            {
                refMap.SetPixel(i, j, DamageMap.GetPixel(i, j));
            }
        }
        refMap.Apply();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        FixedDamageMap = myRenderer.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float rootX = transform.position.x - myCollider.bounds.extents.x;
        float baseX = transform.position.x + myCollider.bounds.extents.x;
        float rootY = transform.position.y - myCollider.bounds.extents.y;
        float baseY = transform.position.y + myCollider.bounds.extents.y;
        float hitPointX = transform.position.x - collision.contacts[0].point.x;
        float hitPointY = transform.position.y - collision.contacts[0].point.y;
        //print("X: " + rootX + " & Y: " + rootY + " To: X: " + baseX + " & Y: " + baseY);

        float normValX = 1 - (hitPointX - rootX) / (baseX - rootX);
        float normValY = 1 - (hitPointY - rootY) / (baseY - rootY);

        int tempX = (int)(normValX * refMap.width);
        int tempY = (int)(normValY * refMap.height) - 5;
        for (int i = -30; i < 30; i++)
        {
            for (int j = -30; j < 30; j++)
            {
                if (refMap.GetPixel(tempX + i, tempY + j).a != 0 && Vector2.Distance(new Vector2(tempX, tempY), new Vector2(tempX + i, tempY + j)) < 30)
                {
                    refMap.SetPixel(tempX + i, tempY + j, Color.white);
                }
            }
        }
        refMap.Apply();
        //Sprite myNewSprite = Sprite.Create(refMap, new Rect(0, 0, refMap.width, refMap.height), new Vector2(.5f, .5f));
        myRenderer.material.SetTexture("_Damage", refMap);
        Destroy(collision.gameObject);
    }
}
