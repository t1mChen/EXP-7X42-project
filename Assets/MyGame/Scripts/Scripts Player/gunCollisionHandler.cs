using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class gunCollisionHandler : MonoBehaviour
{
    // When colliding with player, bullet or enermy, will not trigger gun rotation
    [SerializeField] private string tagToIgnore_1;
    [SerializeField] private string tagToIgnore_2;
    [SerializeField] private string tagToIgnore_3;
    [SerializeField] private string tagToIgnore_4;
    [SerializeField] private string tagToIgnore_5;

    // Defining rotator, to avoid gun pass through wall
    [SerializeField] GameObject rotator;

    private string props = "Prop";
    private gunRotator rota;
    Quaternion rotationOrigin;
    private bool collided = false;
    // Start is called before the first frame update
    void Start()
    {
        rotationOrigin = rotator.transform.rotation;
        rota = rotator.GetComponent<gunRotator>();
    }

    // if the collider of gun collides with wall, then the gun rotates up
    // avoid gun passes through wall

    private void OnTriggerEnter(Collider col)
    {        
        // Identify if the collider met is not triggering gun rotation
        if (col.gameObject.tag != this.tagToIgnore_1 && col.gameObject.tag != this.tagToIgnore_2 
            && col.gameObject.tag != this.tagToIgnore_3 && col.gameObject.tag != props
            && col.gameObject.tag != this.tagToIgnore_4 && col.gameObject.tag != this.tagToIgnore_5) 
        {
            rota.rotateUp();
            collided = true;
        }
    }

    // if the collider of leaves collision with wall, then the gun rotates down
    // avoid gun passes through wall
    private void OnTriggerExit(Collider col)
    {
        // Identify if the collider met is not triggering gun rotation
        if (col.gameObject.tag != this.tagToIgnore_1 && col.gameObject.tag != this.tagToIgnore_2 
            && col.gameObject.tag != this.tagToIgnore_3 && col.gameObject.tag != props && col.gameObject.tag != this.tagToIgnore_4
             && col.gameObject.tag != this.tagToIgnore_5) 
        {
            rota.rotateforward();
            collided = false;
        }
    }

    // Getter for collision status
    public bool getCollied()
    {
        return collided;
    }

}
