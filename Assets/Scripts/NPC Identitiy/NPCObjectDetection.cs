using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCObjectDetection : MonoBehaviour
{

    // object avoidance variables
    //public StateController controller;
    public GameObject eyes;
    public float fieldOfViewAngle = 110f;
    public float viewDistance = 10;

    public LayerMask interactableLayers;
    //public List<int> layerIndex;

    // object detection variables
    public List<GameObject> detectedObjects;
    public GameObject target;

    public List<ObjectMemory> recentlyDetectedObjects;

    [System.Serializable]
    public class ObjectMemory
    {
        public GameObject recentlyDetectedObject;
        public float memoryTimer;
    }

    private void Awake()
    {
        //for (int i = 0; i < layerIndex.Count; i++)
        //    Physics.IgnoreLayerCollision(9, layerIndex[i]);

        detectedObjects = new List<GameObject>();
    }
    void Update()
    {
        DetectObjects();

        if (detectedObjects != null && detectedObjects.Count > 0)
        {
            target = detectedObjects[0].transform.root.gameObject;

        }
        else target = null;

        //if (target != null && target.transform.parent != null)
        // target = target.transform.parent.gameObject;
    }

    //https://www.youtube.com/watch?v=znZXmmyBF-o
    //Mesh CreateWedgeMesh()
    //{
    //    Mesh mesh = new Mesh();

    //    int numTriangles = 8;
    //    int numVertices = numTriangles * 3;

    //    Vector3[] vertices = new Vector3[numVertices];
    //    int[] triangles = new int[numVertices];

    //    Vector3 bottomCenter = Vector3.zero;
    //    Vector3 bottomLeft = Quaternion.Euler(0, -fovAngle, 0) * Vector3.forward * viewDistance;
    //    Vector3 bottomRight = Quaternion.Euler(0, fovAngle, 0) * Vector3.forward * viewDistance;

    //    Vector3 topCenter = bottomCenter + Vector3.up * viewHeight;
    //    Vector3 topLeft = bottomLeft + Vector3.up * viewHeight;
    //    Vector3 topRight = bottomRight + Vector3.up * viewHeight;

    //    int vert = 0;

    //    //left side

    //    //right side

    //    //far side

    //    //top

    //    //bottom

    //    return mesh;
    //}
    public void DetectObjects()
    {

        RaycastHit hit;

        if (Physics.SphereCast(eyes.transform.position, fieldOfViewAngle, transform.forward, out hit, fieldOfViewAngle, interactableLayers)) //(Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, fieldOfViewAngle, interactableLayers) && !detectedObjects.Contains(hit.transform.gameObject)) //(Physics.SphereCast(eyes.transform.position, lookSphere, transform.forward, out hit, lookSphere, interactableLayers)
        {
            //Debug.DrawLine(eyes.transform.position, hit.point, Color.cyan);

            if (!detectedObjects.Contains(hit.transform.root.gameObject) && hit.transform.root.gameObject != this)
                detectedObjects.Add(hit.transform.root.gameObject);

            if (recentlyDetectedObjects.Count > 0)
            {
                for (int i = 0; i < recentlyDetectedObjects.Count; i++)
                {
                    if (recentlyDetectedObjects[i].recentlyDetectedObject == hit.transform.root.gameObject)
                    {
                        recentlyDetectedObjects[i].memoryTimer = 0f;
                    }
                }
            }
            //if (hit.transform.parent != null)
            //{
            //    detectedObjects.Add(hit.transform.root.gameObject);
            //}
            //else 
            //detectedObjects.Add(hit.transform.gameObject);
        }

        for (int i = 0; i < detectedObjects.Count; i++)
        {
            if (detectedObjects[i] != Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, interactableLayers) /*&& detectedObjects.Contains(hit.transform.gameObject)*/)
            {
                recentlyDetectedObjects.Add(new ObjectMemory { recentlyDetectedObject = detectedObjects[i], memoryTimer = GetComponent<NPCBrain>().npcInfo.disposition.attentionSpan});
                detectedObjects.Remove(detectedObjects[i]);
            }
        }

        detectedObjects.Sort(SortByDistanceToMe);

        RecentlyDetected();
        //return objectsAroundAI;
    }

    int SortByDistanceToMe(GameObject a, GameObject b)
    {
        float squaredRangeA = (a.transform.position - transform.position).sqrMagnitude;
        float squaredRangeB = (b.transform.position - transform.position).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    public void RecentlyDetected()
    {
        if (recentlyDetectedObjects.Count > 0)
        {
            for (int i = 0; i < recentlyDetectedObjects.Count; i++)
            {
                recentlyDetectedObjects[i].memoryTimer -= Time.deltaTime;

                if (recentlyDetectedObjects[i].memoryTimer <= 0f)
                {
                    recentlyDetectedObjects.RemoveAt(i);
                }
            }

            //foreach (ObjectMemory memory in recentlyDetectedObjects)
            //{
            //    if (memory != null)
            //    {
            //        memory.memoryTimer -= Time.deltaTime;

            //        if (memory.memoryTimer <= 0f)
            //        {
            //            recentlyDetectedObjects.Remove(memory);
            //        }
            //    }
            //}
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (eyes != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(eyes.transform.position, lookSphere);
    //    }

    //    if (target == null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawRay(eyes.transform.position, eyes.transform.forward);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(eyes.transform.position, eyes.transform.forward);
    //    }
    //}

    //    public Transform rayOrigin;
    //    public float viewDistance;

    //    [field: ReadOnlyField]public Transform selectedObject;

    //    public List<int> keyLayerIndexList;
    //    int layerMask;

    //    private void Update()
    //    {
    //        foreach (int layerIndex in keyLayerIndexList)
    //        {
    //            layerMask = 1 << layerIndex;


    //            RaycastHit hit;

    //            if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, viewDistance, layerIndex))
    //            {
    //                selectedObject = hit.transform;

    //                Debug.Log(gameObject.name + " is looking at " + gameObject.name);
    //            }
    //        }
    //    }

    private void OnDrawGizmos()
    {
        //if (eyes != null)
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(eyes.transform.position, viewDistance);
        //}

        if (detectedObjects == null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(eyes.transform.position, eyes.transform.forward * fieldOfViewAngle);
        }
        //else
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawRay(eyes.transform.position, target.transform.position);
        //}
    }
}


//[CustomEditor(typeof(NPCObjectDetection))]
//public class DetectionEditor : Editor
//{
//    private void OnSceneGUI()
//    {
//        NPCObjectDetection _detection = (NPCObjectDetection)target;
//        if (_detection == null) return;

//        //Color c = Color.green;

//        //Handles.color = new Color(c.r, c.g, c.b, 0.3f);
//        //Handles.DrawSolidDisc(_detection.transform.position, _detection.transform.up, _detection.fov);

//        Handles.color = Color.green;
//        _detection.fovAngle = Handles.ScaleValueHandle(
//            _detection.fovAngle, 
//            _detection.transform.position + _detection.transform.forward * _detection.fovAngle,
//            _detection.transform.rotation, 
//            3,
//            Handles.SphereHandleCap, 
//            1);
//    }
//}