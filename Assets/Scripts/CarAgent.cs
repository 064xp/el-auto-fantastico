using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarAgent : MonoBehaviour
{
    public enum Actions
    {
        Forward,
        Left,
        Right,
        ForwardRight,
        LeftRight,
        None
    }

    public int NumRays;
    public float[] Rays;
    public float DeviationAngle;
    public float Speed;
    public WheelCollider[] Wheels;
    [SerializeField]
    GameObject CurrentStreetSegment;
    WheelHit wheelHit;
    public LayerMask raycastLayerMask;
    public CarController CarController;
    public int NumFeatures { get; private set; }
    public bool Crashed;

    Rigidbody rb;
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        Rays = new float[NumRays];
        rb = GetComponent<Rigidbody>();
        // Num rays + angle + speed
        NumFeatures = NumRays + 2;
        Crashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckWheelColliders();
        GetSpeed();
        CastRays();
        GetDeviationAngle();
    }

    void OnCollisionEnter(Collision collision)
    {
        Crashed = true;
    }

    public void Reset(){
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        Crashed = false;
    }
    public void TakeAction(Actions action)
    {
        float steering = 0f;
        float accel = 0f;

        switch (action)
        {
            case Actions.Forward:
                accel = 1f;
                break;
            case Actions.Left:
                steering = -1f;
                break;
            case Actions.Right:
                steering = 1f;
                break;
            case Actions.ForwardRight:
                accel = 1f;
                steering = 1f;
                break;
            case Actions.LeftRight:
                accel = 1f;
                steering = -1f;
                break;
        }
        CarController.Move(steering, accel, 0f, 0f);
    }

    public float[] GetData()
    {
        float[] data = new float[] {
            DeviationAngle,
            Speed
        };
        return Rays.Concat(data).ToArray();
    }

    public void GetSpeed()
    {
        Speed = rb.velocity.magnitude;
    }

    void CheckWheelColliders()
    {
        foreach (WheelCollider wheel in Wheels)
        {
            if (wheel.GetGroundHit(out wheelHit))
            {
                if (wheelHit.collider.gameObject.CompareTag("Street"))
                {
                    GameObject parent = wheelHit.collider.gameObject.transform.parent.gameObject;
                    if (parent != CurrentStreetSegment)
                    {
                        CurrentStreetSegment = parent;
                        break;
                    }
                }
            }
        }
    }

    private void CastRays()
    {
        float angle = -90;
        for (int i = 0; i < NumRays; i++)
        {
            Vector3 rayOrigin = transform.position;
            rayOrigin.y += 0.5f;
            RaycastHit hit;
            Ray ray = new Ray(rayOrigin, Quaternion.AngleAxis(angle, transform.up) * transform.forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
            {
                Rays[i] = hit.distance;
                Debug.DrawLine(rayOrigin, hit.point, Color.green);
            }
            else
            {
                Rays[i] = Mathf.Infinity;
                Debug.DrawRay(rayOrigin, Quaternion.AngleAxis(angle, transform.up) * transform.forward * 20, Color.cyan);
            }
            angle += 180 / (NumRays - 1);
        }
    }

    // gets the angle of the car relative to the street
    private void GetDeviationAngle()
    {
        if (CurrentStreetSegment != null)
        {
            Vector3 carDirection = transform.forward;
            Vector2 carDirection2D = new Vector2(carDirection.x, carDirection.z);
            Vector3 streetDirection = CurrentStreetSegment.transform.forward;
            Vector2 streetDirection2D = new Vector2(streetDirection.x, streetDirection.z);
            float angle = Vector2.Angle(carDirection2D, streetDirection2D);
            if (Vector3.Dot(Vector3.Cross(carDirection, streetDirection), Vector3.up) < 0)
            {
                angle = -angle;
            }
            DeviationAngle = angle;
        }
    }
}
