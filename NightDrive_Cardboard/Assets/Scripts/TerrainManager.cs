using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    // Speed at which the terrain will move
    private float z_speed = 0.0f;

    // Terrain related variables
    public GameObject terrain_prefab;
    private List<GameObject> spawned_terrain_tiles = new List<GameObject>();

    private int tile_count = 3;
    private float tile_distance = 240.0f;
    private float tile_z_pos = -240;

    // Obstacle related variables
    public List<GameObject> obstacle_prefabs;
    private List<GameObject> spawned_obstacles = new List<GameObject>();

    private int obstacle_count = 12;
    private float obstacle_distance = 30.0f;
    private float obstacle_z_pos = 120;

    void Start()
    {
        // Spawn terrain tiles
        while (spawned_terrain_tiles.Count < tile_count && terrain_prefab)
        {
            GameObject tile_clone = (GameObject)Instantiate(terrain_prefab, new Vector3(0, 0, tile_z_pos), Quaternion.identity);
            spawned_terrain_tiles.Add(tile_clone);

            tile_z_pos += 240;
        }

        // Spawn obstacles
        while (spawned_obstacles.Count < obstacle_count)
        {
            int rand_index = Random.Range(0, obstacle_prefabs.Count - 1);

            if (obstacle_prefabs[rand_index])
            {
                float rand_x_pos = Random.Range(-2.5f, 2.5f);
                float rand_y_rotation = Random.Range(0, 359);

                GameObject obstacle_clone = (GameObject)Instantiate(obstacle_prefabs[rand_index], new Vector3(rand_x_pos, -1.0f, obstacle_z_pos), Quaternion.AngleAxis(rand_y_rotation, Vector3.up));
                spawned_obstacles.Add(obstacle_clone);

                obstacle_z_pos += 30;
            }
        }
    }

    void Update()
    {
        // Update position of terrain tiles
        for (int i = 0; i < spawned_terrain_tiles.Count; i++)
        {
            spawned_terrain_tiles[i].transform.position -= new Vector3(0, 0, z_speed);

            if (spawned_terrain_tiles[i].transform.position.z <= -tile_distance * 1.5f)
            {
                spawned_terrain_tiles[i].transform.position = new Vector3(0, 0, spawned_terrain_tiles[i].transform.position.z + tile_distance * tile_count);
            }
        }

        // Update position of obstacles
        for (int i = 0; i < spawned_obstacles.Count; i++)
        {
            spawned_obstacles[i].transform.position -= new Vector3(0, 0, z_speed);

            if (spawned_obstacles[i].transform.position.z <= -120)
            {
                float rand_x_pos = Random.Range(-2.5f, 2.5f);
                float rand_y_rotation = Random.Range(0, 359);

                spawned_obstacles[i].transform.position = new Vector3(rand_x_pos, -1, 240);
                spawned_obstacles[i].transform.rotation = Quaternion.AngleAxis(rand_y_rotation, Vector3.up);
            }
        }
    }

    // Sets z_speed value
    public void SetTerrainSpeed(float _z_speed)
    {
        z_speed = _z_speed;
    }
}
