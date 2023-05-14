using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField]
    private Slider navigationYOffset;
    [SerializeField]
    private Camera ArCamera;

    private NavMeshAgent agent; // NavMeshAgent component
    public NavMeshPath path; //current calculated path
    private LineRenderer line; //linerenderer to display path
    public Vector3 targetPosition = Vector3.zero;

    private bool lineToggle = false;
    [SerializeField]
    private int currentFloor = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to the same object
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;

        //Debug.Log(NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 100, NavMesh.AllAreas));
        //Debug.Log(NavMesh.SamplePosition(targetPosition, out NavMeshHit hit2, 100, NavMesh.AllAreas));
    }

    private void Update()
    {
        /*if((Input.touchCount>0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            lineToggle = !lineToggle;
        }*/

        Debug.Log("Update called");
        Debug.Log("currentTarget: " + navigationTargetObjects);
        Debug.Log("targetPosition: " + targetPosition);


        if (lineToggle && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

            Debug.Log(path.status);

            line.positionCount = path.corners.Length;
            Vector3[] calculatedPathAndOffest = AddLineOffset();
            line.SetPositions(calculatedPathAndOffest);

            Debug.DrawLine(agent.transform.position, targetPosition, Color.red);

            //line.enabled = true;
        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        Debug.Log("SetCurrentNavigationTarget called with selectedValue: " + selectedValue);

        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(selectedText.ToLower()));

        if (currentTarget != null)
        {
            //Debug.Log("currentTarget is not null");
            //Debug.Log("position object of currentTarget is not null");

            if (!line.enabled)
            {
                ToggleVisibility();
            }

            targetPosition = currentTarget.PositionObject.transform.position;

            //Debug.Log("New target position set to: " + targetPosition);
            agent.SetDestination(targetPosition); // Set the agent's destination to the target position

        }

    }


    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
    }

    public void ChangeActiveFloor(int floorNumber)
    {
        currentFloor = floorNumber;
        SetNavigationTargetDropDownOptions(currentFloor);
    }

    private Vector3[] AddLineOffset()
    {
        if (navigationYOffset.value == 0)
        {
            return path.corners;
        }

        Vector3[] calculatedLine = new Vector3[path.corners.Length];
        for (int i = 0; i < path.corners.Length; i++)
        {
            calculatedLine[i] = path.corners[i] + new Vector3(0, navigationYOffset.value, 0);
        }
        return calculatedLine;
    }

    private void SetNavigationTargetDropDownOptions(int floorNumber)
    {
        navigationTargetDropDown.ClearOptions();
        navigationTargetDropDown.value = 0;

        if (line.enabled)
        {
            ToggleVisibility();
        }
        if (floorNumber == 0)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("MainEntrance"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("GroundFloorAuditorium"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ESB0007"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ESB0011FirstDoor"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ESB0011SecondDoor"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("GFRightElevators"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("GFLeftElevators"));


        }
        if (floorNumber == 1)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ESB1043"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ESB1009"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ESB1055"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("FFRightElevators"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("FFLeftElevators"));


        }
        if (floorNumber == 2)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("SFRightElevators"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("SFLeftElevator"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("DrImransOffice"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("DeansOffice"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("DrHichamsOffice"));

        }
    }

    //trying a different method to warp between floors
    public void ChangeFloorRepositioning(GameObject recenterTarget)
    {
        ArCamera.transform.position = recenterTarget.transform.position;
        ArCamera.transform.rotation = recenterTarget.transform.rotation;
    }

}