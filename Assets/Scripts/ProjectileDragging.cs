using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour {

    public float maxStretch = 3.0f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;

    SpringJoint2D spring;
    Transform catapult;
    Ray rayToMouse;
    Ray leftCatapultToProjectile;
    float maxStretchSqr;
    float circleRadius;
    bool clickedOn;
    Vector2 prevVelocity;

    void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        if (spring == null)
            throw new MissingComponentException("Missing SpringJoint2D component.");

        catapult = spring.connectedBody.transform;
    }

	// Use this for initialization
	void Start () {
        LineRendererSetup();
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        CircleCollider2D circle = collider2D as CircleCollider2D;
        circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {
        // are we dragging?
        if (clickedOn)
            Dragging();
        // does the spring are still there ? Yes means we haven't release yet
        if (spring != null)
        {
            if (!rigidbody2D.isKinematic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude)
            {
                Destroy(spring);
                rigidbody2D.velocity = prevVelocity;
            }
            // if we have release, we are launching the projectile.
            if (!clickedOn)
                prevVelocity = rigidbody2D.velocity;

            LineRendererUpdate();
           
        } else {
            catapultLineBack.enabled = false;
            catapultLineFront.enabled = false;
        }
	}

    void LineRendererSetup()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;
    }

    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }

    void OnMouseUp()
    {
        spring.enabled = true;
        rigidbody2D.isKinematic = false;
        clickedOn = false;
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;

        
    }
    void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        // Point where the "rubber band" will be attached to the Asteroid. 
        // we are adding `circleRadius` to it to attach the rubber at the end, instead of the middle of the Asteroid.
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }
}
