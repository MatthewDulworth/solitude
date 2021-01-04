using UnityEngine;


public class PlayerAfterImage : MonoBehaviour {
    // ---------- Editor Vars ---------- //
    public float maxTime = 0.5f;
    public float initialAlpha = 0.8f;
    public float alphaFade = 0.85f;

    // ---------- Private Vars ---------- //
    private float alpha;
    private float activeTime;
    private SpriteRenderer sr;

    private Transform player;
    private SpriteRenderer playerSr;

    // ---------- On Enable ---------- //
    private void OnEnable() {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSr = player.GetComponent<SpriteRenderer>();

        alpha = initialAlpha;
        sr.sprite = playerSr.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;

        activeTime = 0;
    }

    // ----------- Update ---------- //
    private void Update() {
        // fade 
        alpha *= alphaFade;
        sr.color = new Color(1, 1, 1, alpha);

        // return to pool
        activeTime += Time.deltaTime;
        if (activeTime >= maxTime) {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}