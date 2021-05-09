using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Slice : MonoBehaviour
{
    public Material mat;
    public LayerMask mask;

    void Update()
    {
        // Drag
        // called while button stays pressed
        if (Input.GetMouseButton(0))
        {
            float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
            transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            gameObject.SetActive(false);
        }

        Collider[] cutObjects = Physics.OverlapBox(transform.position, new Vector3(1f, 0.1f, 0.1f), transform.rotation, mask);
        foreach (Collider objects in cutObjects)
        {
            //mat = objects.gameObject.GetComponent<Renderer>().material;
            if (objects.gameObject.tag == "Bomb")
            {
                GameManager.Instance.GameEnd();
            }

            if (objects.tag != "FxTemporaire") // dont try to cut the effect!
            {
                SlicedHull cutObject = Cut(objects.gameObject, mat);
                GameObject cutUp = cutObject.CreateUpperHull(objects.gameObject, mat);
                GameObject cutDown = cutObject.CreateLowerHull(objects.gameObject, mat);

                AddComponent(cutUp);
                AddComponent(cutDown);

                Destroy(objects.gameObject);

                Destroy(cutUp, 10);
                Destroy(cutDown, 10);

                cutUp.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse);
                cutDown.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse);

                Destroy(cutUp.GetComponent<MeshCollider>(), 0.4f);
                Destroy(cutDown.GetComponent<MeshCollider>(), 0.4f);

                GameManager.Instance.IncreaseScore();
                GameManager.Instance.SlashSound();
            }
        }
    }


    public SlicedHull Cut(GameObject obj, Material mat = null)
    {
        return obj.Slice(transform.position, transform.up, mat);
    }

    void AddComponent(GameObject obj)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.layer = 6;
    }
}
