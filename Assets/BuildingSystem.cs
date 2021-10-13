using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    Camera cam;

    [SerializeField] int blockID;
    [SerializeField] GameObject[] Blocks;
    GameObject ghost;
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        #region keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            blockID = 0;
            if(ghost!=null)
            Destroy(ghost.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            blockID = 1;
            if (ghost != null)
                Destroy(ghost.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            blockID = 2;
            if (ghost != null)
                Destroy(ghost.gameObject);
        }
        #endregion
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                #region Spawn Obj
                GameObject go; //spawned Gameobj
                if (Input.GetKey(KeyCode.G))
                {
                     go = Instantiate(Blocks[blockID], hit.collider.transform.position + hit.normal, Quaternion.identity);
                }
                else
                {
                     go=Instantiate(Blocks[blockID], hit.point  , Quaternion.identity);
                }
                #endregion
                #region Connect Objects
                //link all stuff
                GameObject gor = hit.collider.gameObject;//GameObj from ray
            switch (blockID)
            {
                   
                    case 0:
                        go.GetComponent<PipeRemaster>().source = gor.GetComponent<FluidTransfer>();
                        break;
                    case 1:
                        go.GetComponent<pump>().source = gor.GetComponent<FluidTransfer>();
                        break;
                }
            }
            #endregion
                #region Destroy by [RMB]
            //destroyobj
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (hit.collider.tag != "ground")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            #endregion
                #region Snap Objects 
            //snap to object 
            if (Input.GetKey(KeyCode.G))
            {
                showGhost(Blocks[blockID], hit.collider.transform.position + hit.normal);
            }
            else
            {
                showGhost(Blocks[blockID], hit.point);
            }
            #endregion
        }
       
    }
    void showGhost(GameObject ghostobj, Vector3 pos)
    {
        if (ghost == null)
        {
            ghost = Instantiate(ghostobj, pos, Quaternion.identity);
            ghost.GetComponent<BoxCollider>().enabled = false;
            if (ghost == Blocks[0])
            {
                ghost.GetComponent<PipeRemaster>().enabled = false;
            }
            else if (ghost == Blocks[1])
            {
                ghost.GetComponent<pump>().enabled = false;
            }
        }
        else
        {
            ghost.transform.position = pos;
            ghost.name = "ghost";
        }
    }
}
