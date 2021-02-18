using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARSurfaceManager : MonoBehaviour
{
    public delegate void PlaneEvent();
    public static event PlaneEvent onGamePlaneReady;

    public Text helpLabel;
    [SerializeField] Button startButton;

    public Material unselectedMat;
    public Material selectedMat;

    public static ARPlane gamePlane = null;
    ARPlaneManager planeManager;
    ARPlane selectedPlane = null;

    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits;


    private void OnEnable() {
        planeManager = GetComponent<ARPlaneManager>();
        planeManager.planesChanged += PlanesChanged;
        GameManager.onGameSetup += ResumePlaneDetection;
    }

    void Start()
    {
        if (!startButton) { startButton = GameObject.Find("StartButton").GetComponent<Button>(); }
        startButton.interactable = false;

        raycastManager = GetComponent<ARRaycastManager>();
        hits = new List<ARRaycastHit>();
    }

    void Update()
    {
        // If...
        // 1. Touch is happening.
        // 2. Touch was begun this frame (this avoids a plane selection if touch is held after UI change.)
        // 3. Touch is not over UI element (this avoids a ray passing through a UI element).
        // 4. Plane manager is enabled.
        // 5. Raycast returns a hit.
        if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
            && planeManager.enabled && raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon)) {
            
            // Get ID nearest hit plane.
            ARPlane hitPlane = planeManager.GetPlane(hits[0].trackableId);
     
            // Mark plane as selected.
            SelectPlane(hitPlane);
        }
    }

    void SelectPlane(ARPlane hitPlane) {

        if (selectedPlane != null) {
            // Update previously selected plane to use unselected material.
            selectedPlane.gameObject.GetComponent<Renderer>().material = unselectedMat;
        }
        
        // Update selected plane to reference new selection.
        selectedPlane = hitPlane;
        
        // Set newly selected plane's material to selected.
        selectedPlane.gameObject.GetComponent<Renderer>().material = selectedMat;

        startButton.interactable = true;
    }

    public void UseSelectedPlane() {

        gamePlane = selectedPlane;

        // Deactivate unused planes.
        foreach (ARPlane plane in planeManager.trackables) {
           if (plane != selectedPlane) {
               plane.gameObject.SetActive(false);
           }
        }
       
        // Stop plane detection.
        planeManager.enabled = false;


        // Initialize NavMesh Surface on game plane AI surface.
        GetComponent<NavMeshSurface>().BuildNavMesh();

        // Disable AR plane visualization.
        gamePlane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
        gamePlane.GetComponent<MeshRenderer>().enabled = false;
        gamePlane.GetComponent<LineRenderer>().enabled = false;

        // Trigger game plane selection event.
        if (onGamePlaneReady != null) {
            onGamePlaneReady();
        }
    }

    void ResumePlaneDetection() {

        // Clear NavMesh data.
        NavMesh.RemoveAllNavMeshData();

        // Change previous game plane's material to unselected.
        selectedPlane.gameObject.GetComponent<Renderer>().material = unselectedMat;

        gamePlane = null;
        selectedPlane = null;

        planeManager.enabled = true;

        // Re-activate detected planes.
        foreach (ARPlane plane in planeManager.trackables) {
            plane.gameObject.SetActive(true);
        }

        startButton.interactable = false;
    }

    void PlanesChanged(ARPlanesChangedEventArgs planesEvents) {

        // if (planesEvents.added.Count != 0) {
        //     helpLabel.text = "Planes Added!";
        // } else if (planesEvents.updated.Count != 0) {
        //     helpLabel.text = "Planes Updated!";
        // } else if (planesEvents.removed.Count != 0) {
        //     helpLabel.text = "Planes Removed!";
        // } else {
        //     helpLabel.text = "Searching for planes...";
        // }
    }

    private void OnDisable() {
        planeManager.planesChanged -= PlanesChanged;
        GameManager.onGameSetup -= ResumePlaneDetection;
    }
}
