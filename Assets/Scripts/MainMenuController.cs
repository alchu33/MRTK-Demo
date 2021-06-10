using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MainMenuController : MonoBehaviour
{
    public Material[] boundingMats;
    public GameObject handlePrefab;
    public GameObject rotatePrefab;

    private Vector3 position = new Vector3(0, 0, 0.8f);
    private Vector3 size = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 prefabSize = new Vector3(0.03f, 0.03f, 0.03f);
    private string tag = "Demo";
    private GameObject mainObject;
    private int indicator;
    private static Dictionary<PrimitiveType, Mesh> primitiveMeshes = new Dictionary<PrimitiveType, Mesh>();

    public GameObject nextMenu;

    //Method that allows the creation of a Cube from the list of primitive objects
    public void CreateObject()
    {
        ResetScene();
        if (mainObject == null)
        {
            GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObject.transform.position = position;
            newObject.transform.localScale = size;
            newObject.gameObject.tag = tag;

            indicator = 3;
            mainObject = newObject;
        }
    }

    //Method that takes all 'Demo' Objects and adds the capacity to move them with your hand
    public void AddMovement()
    {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag(tag))
        {
            if (item.GetComponent<ObjectManipulator>() == null)
                item.AddComponent(typeof(ObjectManipulator));
        }
    }

    //Method that takes all 'Demo' Objects and adds the capacity to rotate and scale using your hand
    public void AddBounds()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(tag))
        {
            if (item.GetComponent<BoundsControl>() == null)
            {
                item.AddComponent(typeof(BoundsControl));
                item.GetComponent<BoundsControl>().BoundsOverride = item.GetComponent<BoxCollider>();
                item.GetComponent<BoundsControl>().BoxDisplayConfig.BoxMaterial = boundingMats[0];
                item.GetComponent<BoundsControl>().BoxDisplayConfig.BoxGrabbedMaterial = boundingMats[1];
                item.GetComponent<BoundsControl>().ScaleHandlesConfig.HandleMaterial = boundingMats[2];
                item.GetComponent<BoundsControl>().ScaleHandlesConfig.HandleMaterial = boundingMats[3];
                item.GetComponent<BoundsControl>().ScaleHandlesConfig.HandlePrefab = handlePrefab;
                item.GetComponent<BoundsControl>().RotationHandlesConfig.HandlePrefab = rotatePrefab;
                item.GetComponent<BoundsControl>().RotationHandlesConfig.HandleSize = 0.0025f;
                item.GetComponent<BoundsControl>().RotationHandlesConfig.HandleMaterial = boundingMats[2];
                item.GetComponent<BoundsControl>().RotationHandlesConfig.HandleGrabbedMaterial = boundingMats[3];
                if (indicator == 1 || indicator == 2)
                    item.GetComponent<BoxCollider>().size = new Vector3(1, 2, 1);
                else if (indicator == 0 || indicator == 3)
                    item.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            }
                
        }
    }

    //Method that changes the shape of the object created using the create button
    public void ChangeShape()
    {
        if (mainObject != null)
        {
            MeshFilter newFilter = mainObject.GetComponent<MeshFilter>();
            int i;
            do
            {
                i = Random.Range(0, 4);
            }
            while (indicator == i);

            switch (i)
            {
                case 0:
                    newFilter.mesh = GetPrimitiveMesh(PrimitiveType.Sphere);
                    break;
                case 1:
                    newFilter.mesh = GetPrimitiveMesh(PrimitiveType.Capsule);
                    break;
                case 2:
                    newFilter.mesh = GetPrimitiveMesh(PrimitiveType.Cylinder);
                    break;
                case 3:
                    newFilter.mesh = GetPrimitiveMesh(PrimitiveType.Cube);
                    break;
            }
            indicator = i;

            if (indicator == 1 || indicator == 2)
                mainObject.GetComponent<BoxCollider>().size = new Vector3(1, 2, 1);
            else
                mainObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);

        }

    }

    //Method that deletes all 'Demo' objects from the hierarchy
    public void ResetScene()
    {
        position = new Vector3(0, 0, 0.8f);
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(tag))
        {
            Destroy(item);
        }
        mainObject = null;
    }

    //Method that takes all 'Demo' Objects and changes their colour using the materials from the resources folder
    public void ChangeColour()
    {
        Material[] mats = Resources.LoadAll<Material>("Colours");
        foreach(GameObject item in GameObject.FindGameObjectsWithTag(tag))
        {
            int i = Random.Range(0, mats.Length - 1);
            item.GetComponent<MeshRenderer>().material = mats[i];
        }

    }

    //Method that takes the object created with the Create button and gives it the ability to follow you
    public void FollowMeObject()
    {
        if (mainObject != null)
        {
            if (mainObject.GetComponent<RadialView>() == null)
            {
                if (mainObject.GetComponent<SolverHandler>() == null)
                    mainObject.AddComponent(typeof(SolverHandler));
                if (mainObject.GetComponent<FollowMeToggle>() == null)
                    mainObject.AddComponent(typeof(FollowMeToggle));
                mainObject.GetComponent<FollowMeToggle>().AutoFollowAtDistance = true;
                mainObject.GetComponent<FollowMeToggle>().AutoFollowDistance = 0.4f;
                mainObject.GetComponent<RadialView>().MinDistance = 0.3f;
                mainObject.GetComponent<RadialView>().MaxDistance = 1f;
            }
            else
            {
                Destroy(mainObject.GetComponent<FollowMeToggle>());
                Destroy(mainObject.GetComponent<RadialView>());
            }
        }
    }

    //Method that creates one object from each of four primitives. It creates all four as 'Demo' Objects
    public void PopulateScene()
    {
        if (mainObject != null)
            Destroy(mainObject);
        indicator = -1;
        position.x = position.x - 0.6f;
        GameObject[] population = new GameObject[] { GameObject.CreatePrimitive(PrimitiveType.Cube), GameObject.CreatePrimitive(PrimitiveType.Sphere), GameObject.CreatePrimitive(PrimitiveType.Capsule), GameObject.CreatePrimitive(PrimitiveType.Cylinder)};
        foreach(GameObject item in population)
        {
            item.transform.position = position;
            item.transform.localScale = size;
            item.gameObject.tag = tag;
            position.x = position.x + 0.4f;
        }

        position.x = 0;
        if (position.y < 0.8)
            position.y = position.y + 0.4f;
        else if (position.z < 1)
        {
            position.z = position.z + 0.4f;
            position.y = 0;
        }
        else if (position.z > 1)
        {
            position.z = -2;
            position.y = -1;
        }
    
            
    }

    //Method that activates the Arena menu
    public void StartArena()
    {
        ResetScene();
        nextMenu.gameObject.SetActive(!nextMenu.gameObject.activeSelf);
        this.gameObject.SetActive(false);
    }

    //Method that gives the menu the capacity to follow me
    public void FollowMeMenu()
    {
        this.GetComponent<RadialView>().enabled = !this.GetComponent<RadialView>().enabled;
    }

    //Auxiliary method that gets you the mesh belonging to one of the primitive objects
    private Mesh GetPrimitiveMesh(PrimitiveType type)
    {
        if (!primitiveMeshes.ContainsKey(type))
        {
            CreatePrimitiveMesh(type);
        }

        return primitiveMeshes[type];
    }

    //Method that adds the mesh of a primitive object to the array used in the GetPrimitiveMesh method
    private Mesh CreatePrimitiveMesh(PrimitiveType type)
    {
        GameObject gameObject = GameObject.CreatePrimitive(type);
        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        GameObject.Destroy(gameObject);

        primitiveMeshes[type] = mesh;
        return mesh;
    }


    //Actually not even used anymore. Should get rid of it at some point...
    public IEnumerable AddBoundsController()
    {
        Destroy(mainObject.GetComponent<BoundsControl>());
        yield return null;
        mainObject.AddComponent(typeof(BoundsControl));
    }

}
