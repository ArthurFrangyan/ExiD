using Assets.Scripts.Data;
using Assets.Scripts.Generator;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private Visualizer visualizer;

    public void RunProceduralGeneration()
    {
        visualizer.Clean();
        Room room = new Room(10);
        visualizer.PaintFloorTiles(room);
    }
}
