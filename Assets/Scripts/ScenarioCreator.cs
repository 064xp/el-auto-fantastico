using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ScenarioCreator : MonoBehaviour{
    public GameObject cameraGameObject;
    public GameObject carGameObject;
    public GameObject[] streetPrefabs;
    public GameObject streetDirection;
    public GameData tempGameData;
    public List<GameObject> tempGameObjects;
    GameObject streetInstantiated;
    GameObject streetDirectionInstantiated;
    GameObject carInstantiated;
    int selectedPrefab = 0;
    int prefabsCreated = 0;
    Transform initialStreet;
    bool isPreviewing = true;
    string path;

    private void Start() {
        tempGameObjects = new List<GameObject>();
        path = Application.dataPath + "/scenario.json";
        renderPreview();  
    }

    private void Update() {
        reviewPlacement();

        if (Input.GetKeyDown(KeyCode.Q)){
            selectedPrefab++;
            if (selectedPrefab > streetPrefabs.Length - 1) {
                selectedPrefab = 0;
            }
            renderPreview();
        }

        if(Input.GetKeyDown(KeyCode.Return)){
            isPreviewing = !isPreviewing;
            startGamePlay();
        }

        if(Input.GetKeyDown(KeyCode.G)){
            saveData();
        }

        if(Input.GetKeyDown(KeyCode.L)){
            loadData();
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            resetScenario(true);
        }
    }

    private void renderPreview(){
        Destroy(streetInstantiated);
        Destroy(streetDirectionInstantiated);
        streetInstantiated = Instantiate(streetPrefabs[selectedPrefab], transform);
        streetDirectionInstantiated = Instantiate(streetDirection, transform);
    }

    private void createStreet(){
        GameObject temp = Instantiate(streetPrefabs[selectedPrefab], transform.position, streetInstantiated.transform.rotation);
        tempGameData.addData(temp.transform.position, temp.transform.rotation, selectedPrefab);
        tempGameObjects.Add(temp);
        temp.AddComponent<BoxCollider>();
        if(prefabsCreated == 0)
            initialStreet = temp.transform;
        prefabsCreated++;
    }

    private void reviewPlacement(){
        if(isPreviewing){
            int rotationIncrement = 90;
            if(Time.timeScale == 1){
                RaycastHit hit;
                Ray ray =   Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)){
                    if(hit.transform.tag == "Platform"){
                        transform.position = hit.collider.transform.position;
                        transform.position += new Vector3(0, 0.6f, 0);

                        if(Input.GetKeyDown(KeyCode.R)){
                            streetInstantiated.transform.Rotate(0, 0, rotationIncrement);
                            streetDirectionInstantiated.transform.Rotate(0, 0, rotationIncrement);
                        }

                        if(Input.GetMouseButtonDown(0))
                            createStreet();
                    }
                }
            }
        }
    }

    private void startGamePlay(){
        if(!isPreviewing){
            cameraGameObject.SetActive(false);
            streetInstantiated.SetActive(false);
            streetDirectionInstantiated.SetActive(false);
            carInstantiated = Instantiate(carGameObject, initialStreet.localPosition, Quaternion.identity);
        }
        if(isPreviewing){
            Destroy(carInstantiated);
            cameraGameObject.SetActive(true);
            streetInstantiated.SetActive(true);
            streetDirectionInstantiated.SetActive(true);
        }
    }

    private void saveData(){
        print("Saving data");
        GameData tempData = tempGameData;
        SaveJson.Save(path, tempData);
        resetScenario();
    }

    private void loadData(){
        resetScenario();
        string json = System.IO.File.ReadAllText(path);
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        tempGameObjects = new List<GameObject>();
        for(int i=0; i<gameData.position.Count; i++){
            GameObject temp = Instantiate(streetPrefabs[gameData.prefabIndex[i]], gameData.position[i], gameData.rotation[i]);
            tempGameObjects.Add(temp);
        }
    }

    private void resetScenario(bool deleteData = false){
            foreach(GameObject go in tempGameObjects){
                Destroy(go);
            }
            tempGameObjects.Clear();
            renderPreview();
            if(deleteData)
                tempGameData = new GameData();
    }
}
