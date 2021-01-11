using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class BuildingsGrid : MonoBehaviour
{
    public Data data;
    public GameObject floor;
    public Building[,] grid;
    public Building[,] gridGreenCells;
    public List<Material> floorMaterials;
    public ResourcesChanger moneyChanger;
    public Vector2Int gridSize = new Vector2Int(10, 10);
    public GameObject ShopPanel;
    public GameObject equipmentContainer;
    public GameObject greenLightPrefab;
    public GameObject cellContainer;

    [SerializeField] private int cellSize = 3;
    [SerializeField] private bool isBuildingMode = false;

    private GameObject lastOrderPanel;
    private Building flyingBuilding;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        grid = new Building[gridSize.x, gridSize.y];
        floor.GetComponent<MeshRenderer>().material = floorMaterials[0];
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x * cellSize, 0, y * cellSize), new Vector3(cellSize, 1, cellSize));
            }
        }
    }

    private void SpawnGridImages()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y] == null)
                {
                    GameObject newCell = Instantiate(greenLightPrefab, new Vector3(transform.position.x + x * cellSize, 1, transform.position.y + y * cellSize), Quaternion.Euler(0, 45, 0));
                    newCell.transform.SetParent(cellContainer.transform);
                    newCell.transform.position += new Vector3(-1.5f, 1.25f, -1.1f);
                }
            }
        }
    }

    private void DestroyGridImages()
    {
        for (int i = 0; i < cellContainer.transform.childCount; i++)
        {
            Destroy(cellContainer.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        MoveFlyingBuilding();
        CancelBuilding();
        RotateBuilding();
    }

    private void RotateBuilding()
    {
        if (isBuildingMode)
        {
            if (Input.GetKeyDown("r"))
            {
                Vector2Int buildingGridSize = flyingBuilding.GetComponent<Building>().size;

                if (flyingBuilding.transform.localScale == new Vector3(1, 1, 1))
                    flyingBuilding.transform.localScale = new Vector3(-1, 1, 1);
                else
                    flyingBuilding.transform.localScale = new Vector3(1, 1, 1);

                flyingBuilding.GetComponent<Building>().size = new Vector2Int(buildingGridSize.y, buildingGridSize.x);
            }
        }
    }

    //Спавним новое здание
    public void InstantiateFlyingBuilding(Building buildingPrefab)
    {
        SpawnGridImages();
        isBuildingMode = true;
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
        flyingBuilding.transform.SetParent(equipmentContainer.transform);
        floor.GetComponent<MeshRenderer>().material = floorMaterials[1];
        ShopPanel.SetActive(false);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(ShopPanel);
    }

    public void InstantiateFlyingBuilding(Building buildingPrefab, GameObject orderPanel)
    {
        SpawnGridImages();
        isBuildingMode = true;
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);
        flyingBuilding.transform.SetParent(equipmentContainer.transform);
        floor.GetComponent<MeshRenderer>().material = floorMaterials[1];
        ShopPanel.SetActive(false);
        OpenWindowsManager.singletone.AddOrRemovePanelFromList(ShopPanel);

        lastOrderPanel = orderPanel;
    }

    //Проверяем, занято ли место
    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }
        return false;
    }

    //Передвигаем наше здание
    private void MoveFlyingBuilding()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                // Cell size here
                int x = Mathf.RoundToInt(worldPosition.x / cellSize);
                int y = Mathf.RoundToInt(worldPosition.z / cellSize);

                bool available = true;

                if (x < 0 || x > gridSize.x - flyingBuilding.size.x) available = false;
                if (y < 0 || y > gridSize.y - flyingBuilding.size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = new Vector3(x * cellSize, 0, y * cellSize); // and cell size here
                flyingBuilding.ChangeColor(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
    }

    //Проверяем, можем ли поставить здание, если можем, то ставим
    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        if (moneyChanger.IsEnoughMoney(flyingBuilding))
        {
            for (int x = 0; x < flyingBuilding.size.x; x++)
            {
                for (int y = 0; y < flyingBuilding.size.y; y++)
                {
                    grid[placeX + x, placeY + y] = flyingBuilding;
                }
            }
            floor.GetComponent<MeshRenderer>().material = floorMaterials[0];
            DestroyGridImages();
            flyingBuilding.SetDefaultColor();
            ShopEquipmentManager.singleton.availableEquipment.Add(flyingBuilding.gameObject);
            flyingBuilding = null;
            isBuildingMode = false;
            if (lastOrderPanel != null)
            {
                lastOrderPanel.gameObject.SetActive(true);
                lastOrderPanel = null;
            }
        }
    }

    //Если нажали ПКМ, отменяем строительство
    private void CancelBuilding()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (flyingBuilding != null)
            {
                floor.GetComponent<MeshRenderer>().material = floorMaterials[0];
                Destroy(flyingBuilding.gameObject);
                isBuildingMode = false;
                DestroyGridImages();
            }
        }
    }
}
