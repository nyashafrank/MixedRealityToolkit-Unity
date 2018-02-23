﻿using System.Collections;
using System.Collections.Generic;
using MixedRealityToolkit.InputModule.Utilities.Interations;
using MixedRealityToolkit.UX.BoundingBoxes;
using MixedRealityToolkit.Examples.InputModule;
using MixedRealityToolkit.UX.Buttons;
using UnityEngine;

public class BoundingRig : MonoBehaviour
{
    public BoundingBox Box;

    private GameObject objectToBound;
    private CompoundButton appBarButton;
    private GameObject[] rotateHandles;
    private GameObject[] cornerHandles;
    private List<Vector3> handleCentroids;
    private bool isActive = false;
    private System.Int64 updateCount;
    private GameObject transformRig;
    private Vector3 scaleHandleSize = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 rotateHandleSize = new Vector3(0.05f, 0.05f, 0.05f);
    private bool showRig;

    public bool ShowRig
    {
        get
        {
            return showRig;
        }
        set
        {
            if (showRig != value)
            {
                showRig = value;
            }
        }
    }

    public GameObject ObjectToBound
    {
        get
        {
            return objectToBound;
        }
        set
        {
            if (objectToBound != value )
            {
                objectToBound =  value ?? objectToBound;
                this.Box.Target = objectToBound;
                Box.gameObject.SetActive(value != null);
                isActive = value != null;
                if (objectToBound != null)
                {
                   BuildRig();
                   //Activate();
                }
                else
                {
                   //Deactivate();
                }
            }
        }
    }

    public CompoundButton AppBarButton
    {
        get
        {
            return appBarButton;
        }

        set
        {
            appBarButton = value;
        }
    }

    private void Start()
    {
        appBarButton = this.gameObject.GetComponent<CompoundButton>();
        appBarButton.OnButtonClicked += AppBarButton_OnButtonPressed;
    }

    private void AppBarButton_OnButtonPressed(GameObject obj)
    {
        if (showRig == true)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    private void Update()
    {
        if (isActive == true)
        {
            UpdateBoundsPoints();
            UpdateAppBar();

            if (ShowRig)
            {
                if (updateCount == 2)
                {
                    CreateHandles();
                }
                else if (updateCount > 2)
                {
                    UpdateHandles();
                }
                updateCount++;
            }
        }
    }

    public void Activate()
    {
        ShowRig = true;
        updateCount = 1;
        isActive = true;

        if (transformRig == null)
        {
            transformRig = BuildRig();
        }
    }
    public void Deactivate()
    {
        ShowRig = false;
        updateCount = 0;
        ClearHandles();
        transformRig = null;
    }

    public void FocusOnHandle(GameObject handle)
    {
        if (handle != null)
        {
            GameObject textMesh = GameObject.Find("textOut");
            textMesh.GetComponent<TextMesh>().text = handle.name.ToString();

            for (int i = 0; i < rotateHandles.Length; ++i)
            {
                rotateHandles[i].SetActive(rotateHandles[i].name == handle.name);
            }
            for (int i = 0; i < cornerHandles.Length; ++i)
            {
                cornerHandles[i].SetActive(cornerHandles[i].name == handle.name);
            }
        }
        else
        {
            GameObject textMesh = GameObject.Find("textOut");
            textMesh.GetComponent<TextMesh>().text = "None";

            for (int i = 0; i < rotateHandles.Length; ++i)
            {
                rotateHandles[i].SetActive(true);
            }
            for (int i = 0; i < cornerHandles.Length; ++i)
            {
                cornerHandles[i].SetActive(true);
            }
        }
    }

    private GameObject BuildRig()
    {
        Vector3 scale = ObjectToBound.transform.localScale;

        GameObject rig = new GameObject();
        rig.name = "center";
        rig.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        rig.transform.localScale = new Vector3(1.0f / scale.x, 1.0f / scale.y, 1.0f / scale.z);

        GameObject upperLeftFront = new GameObject();
        upperLeftFront.name = "upperleftfront";
        upperLeftFront.transform.SetPositionAndRotation(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
        upperLeftFront.transform.localScale = new Vector3(1, 1, 1);
        upperLeftFront.transform.parent = rig.transform;

        GameObject upperLeftBack = new GameObject();
        upperLeftBack.name = "upperleftback";
        upperLeftBack.transform.SetPositionAndRotation(new Vector3(0.5f, 0.5f, -0.5f), Quaternion.identity);
        upperLeftBack.transform.localScale = new Vector3(1, 1, 1);
        upperLeftBack.transform.parent = rig.transform;

        GameObject lowerLeftFront = new GameObject();
        lowerLeftFront.name = "lowerleftfront";
        lowerLeftFront.transform.SetPositionAndRotation(new Vector3(0.5f, -0.5f, 0.5f), Quaternion.identity);
        lowerLeftFront.transform.localScale = new Vector3(1, 1, 1);
        lowerLeftFront.transform.parent = rig.transform;

        GameObject lowerLeftBack = new GameObject();
        lowerLeftBack.name = "lowerleftback";
        lowerLeftBack.transform.SetPositionAndRotation(new Vector3(0.5f, -0.5f, -0.5f), Quaternion.identity);
        lowerLeftBack.transform.localScale = new Vector3(1, 1, 1);
        lowerLeftBack.transform.parent = rig.transform;

        GameObject upperRightFront = new GameObject();
        upperRightFront.name = "upperrightfront";
        upperRightFront.transform.SetPositionAndRotation(new Vector3(-0.5f, 0.5f, 0.5f), Quaternion.identity);
        upperRightFront.transform.localScale = new Vector3(1, 1, 1);
        upperRightFront.transform.parent = rig.transform;

        GameObject upperRightBack = new GameObject();
        upperRightBack.name = "upperrightback";
        upperRightBack.transform.SetPositionAndRotation(new Vector3(-0.5f, 0.5f, -0.5f), Quaternion.identity);
        upperRightBack.transform.localScale = new Vector3(1, 1, 1);
        upperRightBack.transform.parent = rig.transform;

        GameObject lowerRightFront = new GameObject();
        lowerRightFront.name = "lowerrightfront";
        lowerRightFront.transform.SetPositionAndRotation(new Vector3(-0.5f, -0.5f, 0.5f), Quaternion.identity);
        lowerRightFront.transform.localScale = new Vector3(1, 1, 1);
        lowerRightFront.transform.parent = rig.transform;

        GameObject lowerRightBack = new GameObject();
        lowerRightBack.name = "lowerrightback";
        lowerRightBack.transform.SetPositionAndRotation(new Vector3(-0.5f, -0.5f, -0.5f), Quaternion.identity);
        lowerRightBack.transform.localScale = new Vector3(1, 1, 1);
        lowerRightBack.transform.parent = rig.transform;

        transformRig = rig;

        return rig;
    }
    private void UpdateBoundsPoints()
    {
        handleCentroids = GetBounds();
    }
    private void CreateHandles()
    {
        ClearHandles();
        UpdateCornerHandles();
        UpdateRotateHandles();
        ParentHandles();
        UpdateHandles();
    }
    private void UpdateCornerHandles()
    {
        if (ShowRig)
        {
            handleCentroids = handleCentroids ?? GetBounds();

            if (cornerHandles == null)
            {
                cornerHandles = new GameObject[handleCentroids.Count];
                for (int i = 0; i < handleCentroids.Count; ++i)
                {
                    cornerHandles[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cornerHandles[i].GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1);
                    cornerHandles[i].GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
                    cornerHandles[i].transform.localScale = scaleHandleSize;
                    BoxCollider collider = cornerHandles[i].AddComponent<BoxCollider>();
                    collider.transform.localScale.Scale(new Vector3(3, 3, 3));
                    cornerHandles[i].AddComponent<BoundingBoxGizmoHandle>();
                    cornerHandles[i].GetComponent<BoundingBoxGizmoHandle>().Rig = this;
                    cornerHandles[i].GetComponent<BoundingBoxGizmoHandle>().ObjectToAffect = ObjectToBound;
                    cornerHandles[i].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Y;
                    cornerHandles[i].GetComponent<BoundingBoxGizmoHandle>().AffineType = BoundingBoxGizmoHandle.TransformType.Scale;
                    cornerHandles[i].name = "Corner " + i.ToString();
                }
            }

            for (int i = 0; i < handleCentroids.Count; ++i)
            {
                cornerHandles[i].transform.localPosition = handleCentroids[i];
                cornerHandles[i].transform.localRotation = ObjectToBound.transform.rotation;
            }
        }
    }
    private void UpdateRotateHandles()
    {
        if (ShowRig)
        {
            handleCentroids = handleCentroids ?? GetBounds();

            if (rotateHandles == null)
            {
                rotateHandles = new GameObject[12];

                for (int i = 0; i < rotateHandles.Length; ++i)
                {
                    rotateHandles[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    rotateHandles[i].GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1);
                    rotateHandles[i].GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                    rotateHandles[i].GetComponent<Collider>().transform.localScale *= 2.0f;
                    rotateHandles[i].transform.localScale = rotateHandleSize;
                    SphereCollider collider = rotateHandles[i].AddComponent<SphereCollider>();
                    collider.transform.localScale.Scale(new Vector3(3, 3, 3));
                    rotateHandles[i].AddComponent<BoundingBoxGizmoHandle>();
                    rotateHandles[i].GetComponent<BoundingBoxGizmoHandle>().Rig = this;
                    rotateHandles[i].GetComponent<BoundingBoxGizmoHandle>().ObjectToAffect = ObjectToBound;
                    rotateHandles[i].GetComponent<BoundingBoxGizmoHandle>().AffineType = BoundingBoxGizmoHandle.TransformType.Rotation;
                    rotateHandles[i].name = "Middle " + i.ToString();
                }
            }

            rotateHandles[0].transform.localPosition = (handleCentroids[2] + handleCentroids[0]) * 0.5f;
            rotateHandles[1].transform.localPosition = (handleCentroids[3] + handleCentroids[1]) * 0.5f;
            rotateHandles[2].transform.localPosition = (handleCentroids[6] + handleCentroids[4]) * 0.5f;
            rotateHandles[3].transform.localPosition = (handleCentroids[7] + handleCentroids[5]) * 0.5f;
            rotateHandles[4].transform.localPosition = (handleCentroids[0] + handleCentroids[1]) * 0.5f;
            rotateHandles[5].transform.localPosition = (handleCentroids[2] + handleCentroids[3]) * 0.5f;
            rotateHandles[6].transform.localPosition = (handleCentroids[4] + handleCentroids[5]) * 0.5f;
            rotateHandles[7].transform.localPosition = (handleCentroids[6] + handleCentroids[7]) * 0.5f;
            rotateHandles[8].transform.localPosition = (handleCentroids[0] + handleCentroids[4]) * 0.5f;
            rotateHandles[9].transform.localPosition = (handleCentroids[1] + handleCentroids[5]) * 0.5f;
            rotateHandles[10].transform.localPosition = (handleCentroids[2] + handleCentroids[6]) * 0.5f;
            rotateHandles[11].transform.localPosition = (handleCentroids[3] + handleCentroids[7]) * 0.5f;

            rotateHandles[0].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Y;
            rotateHandles[1].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Y;
            rotateHandles[2].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Y;
            rotateHandles[3].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Y;

            rotateHandles[4].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Z;
            rotateHandles[5].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Z;
            rotateHandles[6].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Z;
            rotateHandles[7].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.Y;

            rotateHandles[8].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.X;
            rotateHandles[9].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.X;
            rotateHandles[10].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.X;
            rotateHandles[11].GetComponent<BoundingBoxGizmoHandle>().Axis = BoundingBoxGizmoHandle.AxisToAffect.X;
        }
    }
    private void ParentHandles()
    {
        transformRig.transform.position = Box.transform.position;
        transformRig.transform.rotation = Box.transform.rotation;

        Vector3 invScale = ObjectToBound.transform.localScale;

        transformRig.transform.localScale = new Vector3(0.5f / invScale.x, 0.5f / invScale.y, 0.5f / invScale.z);

        transformRig.transform.parent = ObjectToBound.transform;
    }

    private void UpdateHandles()
    {
        if (ShowRig)
        {
            UpdateCornerHandles();
            UpdateRotateHandles();
        }
    }

    private void UpdateAppBar()
    {
        if (handleCentroids != null)
        {
            appBarButton.transform.position = (handleCentroids[0] + handleCentroids[4]) * 0.5f;
        }
    }

    private void ClearCornerHandles()
    {
        if (cornerHandles != null)
        {
            for (int i = 0; i < cornerHandles.Length; ++i)
            {
                GameObject.Destroy(cornerHandles[i]);
            }
            cornerHandles = null;
            handleCentroids = null;
        }

        cornerHandles = null;
        handleCentroids = null;
    }
    private void ClearRotateHandles()
    {
        if (rotateHandles != null && rotateHandles.Length > 0 && rotateHandles[0] != null)
        {
            for (int i = 0; i < rotateHandles.Length; ++i)
            {
                if (rotateHandles[i] != null)
                {
                    Destroy(rotateHandles[i]);
                    rotateHandles[i] = null;
                }
            }
        }

        rotateHandles = null;
    }
    private void ClearHandles()
    {
        ClearCornerHandles();
        ClearRotateHandles();
    }

    private List<Vector3> GetBounds()
    {
        if (ObjectToBound != null)
        {
            List<Vector3> bounds = new List<Vector3>();
            LayerMask mask = new LayerMask();
            GameObject clone = GameObject.Instantiate(Box.gameObject);
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.position = new Vector3(0, 0, 0);
            BoundingBox.GetRenderBoundsPoints(clone, bounds, mask);
            GameObject.Destroy(clone);

            Matrix4x4 m = Matrix4x4.Rotate(ObjectToBound.transform.rotation);

            for (int i = 0; i < bounds.Count; ++i)
            {
                bounds[i] = m.MultiplyPoint(bounds[i]);
                bounds[i] += ObjectToBound.transform.position;
            }

            return bounds;
        }

        return null;
    }


}