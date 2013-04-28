using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CreateCubes : MonoBehaviour {
	public GameObject Cube1;
	public GameObject Cube2;
	public GameObject Cube3;
	public GameObject CubeSpawner;
	public float spawnWait = 1;
	
	int maxCubes, totalCubes = 0;
	float deltaSinceLastCube;
	private List<GameObject> Cube1s = new List<GameObject>();
	private List<GameObject> Cube2s = new List<GameObject>();
	private List<GameObject> Cube3s = new List<GameObject>();
	float coffA, coffB, coffC;
	string sequence;
	bool isGameOver;
	
	// Use this for initialization
	void Start () {
		reset();
	}
	
	void reset() {
		totalCubes = 0;
		deltaSinceLastCube = 0;
		maxCubes = UnityEngine.Random.Range(180, 240);
		sequence = "";
		removeCubes(Cube1s.Count, Cube1s);
		removeCubes(Cube2s.Count, Cube2s);
		removeCubes(Cube3s.Count, Cube3s);
	}
	
	// Update is called once per frame
	void Update () {
		if (Globals.AllowingInput == false && isGameOver == false)
		{
			isGameOver = true;
			findSequence();
			runSequence();
		}
		else if (Globals.AllowingInput == true && isGameOver == true)
		{
			isGameOver = false;
			reset();
		}
		
		if (totalCubes > maxCubes)
			return;
		
		if (deltaSinceLastCube > spawnWait)
		{
			makeNextCube();
			totalCubes++;
			deltaSinceLastCube = 0;
			//Debug.Log("total:" + totalCubes);
		}
		else
		{
			deltaSinceLastCube += Time.deltaTime;
		}
	}
	
	#region create cubes
	void makeNextCube()
	{
		int cubeType = UnityEngine.Random.Range(1, 4);
		switch (cubeType)
		{
		case 1: Cube1s.Add(makeNextCube(Cube1));
			break;
		case 2: Cube2s.Add(makeNextCube(Cube2));
			break;
		case 3: Cube3s.Add(makeNextCube(Cube3));
			break;
		default: throw new Exception("cubeType out of range");
			//break;
		}
	}
	
	float xPosModifier, yPosModifier;
	int xDirection = 1, yDirection = 1;
	GameObject makeNextCube(GameObject cube)
	{
		Vector3 position = CubeSpawner.transform.position;
		
		xPosModifier += bounce(6, xPosModifier, ref xDirection);
		position.x += xPosModifier;
		yPosModifier += bounce(2, yPosModifier, ref yDirection);
		position.y += yPosModifier;
		return Instantiate(cube, position, CubeSpawner.transform.rotation) as GameObject;		
	}
	
	float bounce(float size, float currentPos, ref int currentDirection)
	{
		if (currentPos > size)
			currentDirection = -1;
		else if (currentPos < -1 * size)
			currentDirection = 1;
		
		return currentDirection * .5f;
	}
	#endregion create cubes
	
	void findSequence()
	{
		int redValue = Globals.cubeValues[0],
		    greenValue = Globals.cubeValues[1],
		    blueValue = Globals.cubeValues[2];
		
		//http://mathforum.org/library/drmath/view/51538.html
		coffA = redValue;
		coffB = (2f*greenValue) - (1.5f*coffA) - (blueValue/2f);
		coffC = (blueValue - coffA - 2*coffB)/4;
	}
	
	void runSequence()
	{
		bool canContinue = true;
		int sequenceIndex = 0;
		while (canContinue)
		{
			canContinue = runIndexAt(sequenceIndex++, Cube1s);
			if (canContinue == false)
				break;
			
			canContinue = runIndexAt(sequenceIndex++, Cube2s);
			if (canContinue == false)
				break;
			
			canContinue = runIndexAt(sequenceIndex++, Cube3s);
		}
		Globals.FinalSequence = sequence.Substring(2);
		//Debug.Log("Sequence:" + sequence);
	}
	
	int getSeqAt(int n)
	{
		return (int)(coffA + coffB*n + coffC*Math.Pow(n,2));
	}
	
	bool runIndexAt(int sequenceIndex, List<GameObject> collectionToAffect)
	{
		int currentValue = getSeqAt(sequenceIndex++);
		sequence += ", " + currentValue;
		if (currentValue > 0 && collectionToAffect.Count > currentValue)
		{
			removeCubes(currentValue, collectionToAffect);
			return true;
		}
		else
		{
			return false;
		}
	}
	
	void removeCubes(int numToRemove, List<GameObject> fromCollection)
	{
		for (int i = numToRemove-1; i >= 0; i--)
		{
			GameObject.Destroy(fromCollection[i]);
			fromCollection.RemoveAt(i);
		}
	}
}
