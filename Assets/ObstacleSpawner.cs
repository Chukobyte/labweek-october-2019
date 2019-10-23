using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	public const int MAX_TIME = 3;
	public const int X_MIN_LIMIT = -10;
	public const int X_MAX_LIMIT = 10;
	public const int Y_SPAWN_POSITION = -2;
	public GameObject obstaclePrefab;
	public ArrayList obstacleList = new ArrayList();
	private float[] randXPositions = new float[] {-4.0f, -3.0f, -2.0f, -1.0f, 0.0f, 1.0f, 2.0f, 3.0f, 4.0f};
	public Transform player;
	public float timeLeft = MAX_TIME;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
		timeLeft -= Time.deltaTime;
        if (timeLeft <= 0 && player.position.z + 60 <= 1000)
		{
			//spawn obstacle
			int obstacleCount = Random.Range(2, 4);
			ArrayList xPositions = GetRandomXPositionsIndices(obstacleCount);
            for(int i = 0; i < obstacleCount; i++)
			{
				float randZPosition = Random.Range(player.position.z + 60, player.position.z + 80);
				GameObject obstacleInstance = Instantiate(obstaclePrefab, new Vector3((float)xPositions[i], Y_SPAWN_POSITION, randZPosition), Quaternion.identity);
				obstacleList.Add(obstacleInstance);
			}
			timeLeft = MAX_TIME;
		}
    }

    private ArrayList GetRandomXPositionsIndices(int count)
	{
		ArrayList randXPositionsList = new ArrayList();
		int i = 0;
        while (i < count)
        {
			int rand_x = Random.Range(0, randXPositions.Length);
			if (!randXPositionsList.Contains(randXPositions[rand_x]))
            {
				//Debug.Log("rand_x = " + rand_x);
				//Debug.Log("rand_x list = " + randXPositions[rand_x]);
				randXPositionsList.Add(randXPositions[rand_x]);
				i++;
			}
		}
		return randXPositionsList;
	}

	public void ResetSpawner()
	{
        foreach (GameObject obstacle in obstacleList)
		{
			Destroy(obstacle);
		}
		obstacleList.Clear();
		timeLeft = MAX_TIME;
	}
}
