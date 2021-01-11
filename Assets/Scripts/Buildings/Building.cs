using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(3, 3);
    [HideInInspector] public Renderer buildingRenderer;

    [HideInInspector] public int buildingCost;

    private Color defaultColor;

    private void Awake()
    {
        //Получаем рендерер модельки, цвет которой будем изменять
        buildingRenderer = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        //Сохраняем обычный цвет объекта
        defaultColor = buildingRenderer.material.color;
    }

    //Изменяем цвет на зелёный или красный в зависимости от того, можем ли мы поставить здание или нет
    public void ChangeColor(bool available)
    {
        buildingRenderer.material.color = available ? Color.green : Color.red;
    }

    //Устанавливаем обычный цвет для здания
    public void SetDefaultColor()
    {
        buildingRenderer.material.color = defaultColor;
    }

    //Рисуем квадраты под объектом, чтобы видеть как он устанавливается
    private void OnDrawGizmos()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x * 3, 0, y * 3), new Vector3(3, 1, 3));
            }
        }
    }

    //Тип постройки
    public enum Type
    {
        Table,
        Option_base_1,
        Refregirator,
        Gematology_analizator,
        Virus_identificator
    }
}
