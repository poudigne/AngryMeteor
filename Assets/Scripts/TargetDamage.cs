using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

	public int hitPoints = 2;
	public Sprite damagedSprite;
	public float damageImpactSpeed;

	private int currentHitPoints;
	private float damageImpactSqr;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		damageImpactSqr = damageImpactSpeed * damageImpactSpeed;
	}
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.tag != "Damager")
			return;

		if (collision.relativeVelocity.sqrMagnitude < damageImpactSqr)
			return;

		spriteRenderer.sprite = damagedSprite;
		currentHitPoints --;
		if (currentHitPoints <= 0)
			Kill ();
	}
	void Kill(){
		spriteRenderer.enabled = false;
		collider2D.enabled = false;
		rigidbody2D.isKinematic = true;
	}
}
