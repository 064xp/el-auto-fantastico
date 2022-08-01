using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioCreator : MonoBehaviour{
    public GameObject streetGameObject;
    public GameObject cornerStreetGameObject;
    public GameObject previewGameObject;
    public GameObject[] streetPrefabs;
    GameObject instantiated;
    int selectedPrefab = 0;
    bool isCorner = false;

    private void Start() {
       renderPreview(); 
    }

    private void Update() {
        reviewPlacement();

        if (Input.GetKeyDown(KeyCode.Q)) {
            selectedPrefab++;
            if (selectedPrefab > streetPrefabs.Length - 1) {
                selectedPrefab = 0;
            }
            renderPreview();
        }
    }

    private void renderPreview(){
        Destroy(instantiated);
        instantiated = Instantiate(streetPrefabs[selectedPrefab], transform);
    }

    private void createStreet(){
        GameObject temp = Instantiate(streetPrefabs[selectedPrefab], transform.position, instantiated.transform.rotation);
        temp.AddComponent<BoxCollider>();
    }

    private void reviewPlacement(){
        int rotationIncrement = 90;
        if(Time.timeScale == 1){
            RaycastHit hit;
            Ray ray =   Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                print(hit.transform.tag);
                if(hit.transform.tag == "Platform"){
                    transform.position = hit.collider.transform.position;
                    transform.position += new Vector3(0, 0.6f, 0);

                    if(Input.GetKeyDown(KeyCode.R)){
                        print("Rotation");
                        instantiated.transform.Rotate(0, 0, rotationIncrement);
                    }

                    if(Input.GetMouseButtonDown(0))
                        createStreet();
                }
            }
        }
    }
}
