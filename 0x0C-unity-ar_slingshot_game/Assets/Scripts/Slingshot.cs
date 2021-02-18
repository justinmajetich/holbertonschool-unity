using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public delegate void SlingshotEvent();
    public static event SlingshotEvent onAmmoDepleted;

    [SerializeField] Transform sessionSpace;
    [SerializeField] Camera m_Camera;
    [SerializeField] AmmoDisplay ammoDisplay;
    SlingshotVisualizations trajectoryRenderer;

    // Ammo management.
    public GameObject ammoPrefab;
    GameObject ammo;
    int ammoCapacity = 7;
    int ammoRemaining = 0;
    
    bool isLoaded = false;

    // Drag mechanics.
    Vector2 lastTouchPos;
    bool ammoIsBeingDragged = false;
    [SerializeField] float dragSpeedModifierX = 0.1f;
    [SerializeField] float dragSpeedModifierZ = 0.4f;

    [SerializeField] float velocityModifier = 5f;
    float potentialVelocity = 0f;


    void OnEnable() {
        GameManager.onGameSetup += ClearAmmo;
        GameManager.onStartGame += InitializeAmmo;
        Ammo.onAmmoDestroyed += DecreaseAmmo;
        GameManager.onGameOver += ClearAmmo;
    }

    void Start()
    {
        if (!m_Camera) { m_Camera = GameObject.Find("AR Camera").GetComponent<Camera>(); }
        if (!sessionSpace) { sessionSpace = GameObject.Find("AR Session Origin").transform; }
        if (!ammoDisplay) { ammoDisplay = GameObject.Find("AmmoDisplay").GetComponent<AmmoDisplay>(); }

        trajectoryRenderer = GetComponent<SlingshotVisualizations>();
        trajectoryRenderer.Hide();
    }

    void Update() {

        // If user is touching screen, check for ammo drag.
        if (isLoaded && Input.touchCount > 0) {

            // If not already dragging, check if touch is begun on ammo instance.
            if (!ammoIsBeingDragged) {
                CheckForAmmoDrag();

            // If drag is already initiated, continue dragging ammo.
            } else {
                DragAmmo();
                
                // Calculate new potential velocity given new position of ammo relative to slingshot pivot.
                Vector3 offset = transform.position - ammo.transform.position;
                potentialVelocity = offset.magnitude * velocityModifier;

                trajectoryRenderer.Visualize(ammo.transform, potentialVelocity);
            }

        // No drag is currently taking place.
        } else {

            if (ammoIsBeingDragged) {
                
                trajectoryRenderer.Hide();
                Fire();
            }

            ammoIsBeingDragged = false;
        }
    }

    void CheckForAmmoDrag() {

        Touch touch = Input.GetTouch(0);

        // Create ray from touch position on phone screen.
        Ray ray = m_Camera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            // If ray hits this ammo object...
            if (hit.transform == ammo.transform) {

                if (touch.phase == TouchPhase.Began) {

                    // Begin tracking last touch position.
                    lastTouchPos = new Vector2 (touch.position.x / Screen.width, touch.position.y / Screen.height);

                    // Initiate dragging.
                    ammoIsBeingDragged = true;
                }
            }
        }
    }

    void DragAmmo() {

        Touch touch = Input.GetTouch(0);

        // Get direction of drag.
        Vector2 touchPos = new Vector2 (touch.position.x / Screen.width, touch.position.y / Screen.height);
        Vector2 direction = touchPos - lastTouchPos;

        // Only apply drag occurs behind starting point on Z axis.
        if ((ammo.transform.localPosition.z + direction.y * dragSpeedModifierX) < 0) {

            // Apply drag direction to ammo position.
            ammo.transform.localPosition = new Vector3(ammo.transform.localPosition.x + direction.x * dragSpeedModifierX,
                                                ammo.transform.localPosition.y,
                                                ammo.transform.localPosition.z + direction.y * dragSpeedModifierZ);
        }

        lastTouchPos = touchPos;
    }

    void Fire() {

        // Get rigidbody, enable gravity and apply force.
        Rigidbody rb = ammo.GetComponent<Rigidbody>();
        rb.useGravity = true;
        // rb.AddForce((transform.position - ammo.transform.position) * 1000f);
        rb.velocity = transform.TransformDirection(new Vector3(0f, 0f, potentialVelocity));
        rb.AddTorque(ammo.transform.right * 10f);

        // Re-parent ammo to AR session space.
        ammo.transform.parent = sessionSpace;

        isLoaded = false;
    }

    void InitializeAmmo() {

        // Make sure slingshot is cleared of existing ammo instance.
        ClearAmmo();
        
        ammoRemaining = ammoCapacity;
        ammoDisplay.SetAmmoCount(ammoRemaining);

        LoadAmmo();
    }

    void DecreaseAmmo() {

        // Reduce remaining ammo.
        ammoRemaining--;
        ammoDisplay.SetAmmoCount(ammoRemaining);

        // If ammo depleted, invoke event; else, load next ammo instance.
        if (ammoRemaining <= 0) {
            onAmmoDepleted();
        } else {
            LoadAmmo();
        }
    }

    void LoadAmmo() {
        ammo = Instantiate(ammoPrefab, transform.position, transform.rotation, transform);
        isLoaded = true;
    }

    void ClearAmmo() {
        if (ammo) {
            ammo.GetComponent<Ammo>().Clear();
        }
    }

    void OnDisable() {
        GameManager.onGameSetup -= ClearAmmo;
        GameManager.onStartGame -= InitializeAmmo;
        Ammo.onAmmoDestroyed -= DecreaseAmmo;
        GameManager.onGameOver -= ClearAmmo;
    }
}
