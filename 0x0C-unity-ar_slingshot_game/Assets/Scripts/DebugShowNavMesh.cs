
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;

public class DebugShowNavMesh : MonoBehaviour
{
	public ARSurfaceManager surfaceManager;
	public Text debugText;

    private void OnEnable() {
        // ARSurfaceManager.onGamePlaneReady += ShowMesh;
    }

	private void Start() {
		ShowMesh();
	}

	// Generates the NavMesh shape and assigns it to the MeshFilter component.
	void ShowMesh()
	{

		// transform.position = ARSurfaceManager.gamePlane.transform.position;
		// transform.rotation = ARSurfaceManager.gamePlane.transform.rotation;

		// NavMesh.CalculateTriangulation returns a NavMeshTriangulation object.
		NavMeshTriangulation meshData = NavMesh.CalculateTriangulation();

		debugText.text = "";
		foreach (Vector3 vert in meshData.vertices) {
			debugText.text += vert;
		}

		// Create a new mesh and chuck in the NavMesh's vertex and triangle data to form the mesh.
		Mesh mesh = new Mesh();
		mesh.vertices = meshData.vertices;
		mesh.triangles = meshData.indices;

		// Assigns the newly-created mesh to the MeshFilter on this GameObject.
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void Update() {
		// transform.Rotate(transform.forward, 2f);

		if (Input.touchCount > 2) {
			ShowMesh();
		}
	}

    private void OnDisable() {
        // ARSurfaceManager.onGamePlaneReady -= ShowMesh;
    }
}
