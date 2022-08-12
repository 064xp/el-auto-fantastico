using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAgent : MonoBehaviour
{
    public int NumRays;
    public float[] Rays;
    public float DeviationAngle;
    public WheelCollider[] Wheels;
    [SerializeField]
    GameObject CurrentStreetSegment;
    WheelHit wheelHit;
    public LayerMask raycastLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        Rays = new float[NumRays];        
    }

    // Update is called once per frame
    void Update()
    {
        CheckWheelColliders();
        CastRays();
        GetDeviationAngle();

    }

    public float[] GetData(){
        return Rays.Concat(new float[] { DeviationAngle }).ToArray();
    }


    void CheckWheelColliders(){
        foreach(WheelCollider wheel in Wheels){
            if(wheel.GetGroundHit(out wheelHit)){
                if(wheelHit.collider.gameObject.CompareTag("Street")){
                    GameObject parent = wheelHit.collider.gameObject.transform.parent.gameObject;
                    if(parent != CurrentStreetSegment){
                        CurrentStreetSegment = parent;
                        break;
                    }
                }
            }
        }
    }

    private void CastRays(){
        float angle = -90;
        for(int i=0; i<NumRays; i++){
            Vector3 rayOrigin = transform.position;
            rayOrigin.y += 0.5f;
            RaycastHit hit;
            Ray ray = new Ray(rayOrigin, Quaternion.AngleAxis(angle, transform.up) * transform.forward);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask)){
                Rays[i] = hit.distance;
                Debug.DrawLine(rayOrigin, hit.point, Color.green);
            } else {
                Rays[i] = Mathf.Infinity;
                Debug.DrawRay(rayOrigin,  Quaternion.AngleAxis(angle, transform.up) * transform.forward * 20, Color.cyan);
            }
            angle += 180 / (NumRays - 1);
        }
    }
    
    // gets the angle of the car relative to the street
    private void GetDeviationAngle(){
        if(CurrentStreetSegment != null){
            Vector3 carDirection = transform.forward;
            Vector2 carDirection2D = new Vector2(carDirection.x, carDirection.z);
            Vector3 streetDirection = CurrentStreetSegment.transform.forward;
            Vector2 streetDirection2D = new Vector2(streetDirection.x, streetDirection.z);
            float angle = Vector2.Angle(carDirection2D, streetDirection2D);
            if(Vector3.Dot(Vector3.Cross(carDirection, streetDirection), Vector3.up) < 0){
                angle = -angle;
            }
            DeviationAngle = angle;
        }
    }
}
