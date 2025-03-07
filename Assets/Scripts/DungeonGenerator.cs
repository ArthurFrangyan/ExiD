using Assets.Scripts.Data;
using Assets.Scripts.Generator;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private Visualizer visualizer;
    [SerializeField]
    private int steps;

    public void RunProceduralGeneration()
    {
        visualizer.Clean();
        Room room = new Room(steps);
        visualizer.PaintFloorTiles(room);
    }
}
