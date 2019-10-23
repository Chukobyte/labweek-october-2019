using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	public const int MAX_TIME = 3;
	public const int X_MIN_LIMIT = -10;
	public const int X_MAX_LIMIT = 10;
	public GameObject obstaclePrefab;
	public ArrayList obstacleList = new ArrayList();
	private int[] randXPositions = new int[] {-4, -3, -2, -1, 0, 1, 2, 3, 4};
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
			int obstacleCount = Random.Range(1, 3);
			ArrayList xPositions = GetRandomXPositionsIndices(obstacleCount);
            for(int i = 0; i < obstacleCount; i++)
			{
				GameObject obstacleInstance = Instantiate(obstaclePrefab, new Vector3(Random.Range(0, xPositions.Count), -2, player.position.z + 60), Quaternion.identity);
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
			int rand_x = GetRandomX();
			if (randXPositionsList.Contains(rand_x))
            {
				randXPositionsList.Add(rand_x);
				i++;
			}
		}
		return randXPositionsList;
	}

	private int GetRandomX()
	{
		return randXPositions[Random.Range(0, randXPositions.Length)];
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
