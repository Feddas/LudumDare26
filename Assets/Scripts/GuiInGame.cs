using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class GuiInGame : MonoBehaviour {
	const string intro = "Enter a number for each color, each must be between 0-9. Your numbers create an Arithmetic, Square, or Triangular Number Sequence. This sequence will be used to remove and minimize the colored cubes. The goal is to game the sequences to create the most minimalism.";
	
	void Start () {
		foreach (var color in colors) {
			mystyle.Add(addStyle(color));
		}
	}
	
	GUIStyle addStyle(Color color)
	{
		GUIStyle style = new GUIStyle();
		style.normal.textColor = color;
		
		return style;
	}
	
	void Update () {
	
	}
	
	List<string> sequence = new List<string>() { "1", "2", "3" };
//	List<int> selection = new List<int>() { 0, 1, 2 };
//	List<int> previous = new List<int>() { 0, 1, 2 };
//    string[] selStrings = new string[] {"1st", "2nd", "3rd"};
	List<Color> colors = new List<Color>() { Color.red, Color.green, Color.blue };
	List<string> colorNames = new List<string>() { "Red", "Green", "Blue" };
	List<GUIStyle> mystyle = new List<GUIStyle>();
	
	const int selectionUiHeight = 130;
	const int totalUiWidth = 120, totalUiHeight = selectionUiHeight + 80;
	const int margin = 10;
	int xPos = (Screen.width/2) - totalUiWidth/2;
	int yPos = (Screen.height/2) - totalUiHeight/2;
	bool firstRun = true;
	
    void OnGUI() {
		if (firstRun)
		{
		    GUI.skin.box.wordWrap = true;
			firstRun = false;
		}
		
		if (Globals.AllowingInput == false)
		{
			GUI.Box(new Rect(xPos-totalUiWidth, yPos, 3*totalUiWidth, 60)
				,"Number sequence: " + Globals.FinalSequence);
			if(GUI.Button(new Rect(xPos+80/2, yPos+selectionUiHeight+25, totalUiWidth-80/2+margin, 30), "New Game"))
			{
				Globals.AllowingInput = true;
			}
		}
		else
		{
			GUI.Box(new Rect(xPos-totalUiWidth, yPos-110-margin, 3*totalUiWidth, 100),intro);
			GUI.Box(new Rect(xPos-margin, yPos-margin, totalUiWidth+2*margin, selectionUiHeight+2*margin),"");
			addSelectionGrid(0);
			addSelectionGrid(1);
			addSelectionGrid(2);
			if(GUI.Button(new Rect(xPos+80/2, yPos+selectionUiHeight+25, totalUiWidth-80/2+margin, 30), "Go"))
			{
				setGlobals();
			}

//        checkSelection(0, 1, 2);
//        checkSelection(1, 2, 0);
//        checkSelection(2, 0, 1);
		}
    }
	
	void setGlobals()
	{
		Globals.AllowingInput = false;
		
//		Globals.cubePositions[0] = selection[0];
//		Globals.cubePositions[1] = selection[1];
//		Globals.cubePositions[2] = selection[2];
		
		Globals.cubeValues[0] = convertToInt(sequence[0]);
		Globals.cubeValues[1] = convertToInt(sequence[1]);
		Globals.cubeValues[2] = convertToInt(sequence[2]);
	}
	
	int convertToInt(string input)
	{
		int output;
		if(int.TryParse(input, out output) == false)
			output = 0;
		return output;
	}
	
	void addSelectionGrid(int index)
	{
		int curXPos = xPos;
		GUI.Label(new Rect(curXPos, yPos+(50*index), 60, 30), colorNames[index], mystyle[index]);
		
		curXPos += 80;
		sequence[index] = GUI.TextField(new Rect(curXPos, yPos+(50*index), 40, 30),sequence[index], 1);
        sequence[index] = Regex.Replace(sequence[index], @"[^0-9]", "");
		
//		xPos += 60;
//		previous[index] = selection[index];
//        selection[index] = GUI.SelectionGrid(new Rect(xPos, 25+(50*index), 200, 30), selection[index], selStrings, 3);
	}
	
//    void checkSelection(int index, int against1, int against2)
//    {
//        if (previous[index] != selection[index])
//        {
//            if (selection[against1] == selection[index])
//                selection[against1] = previous[index];
//            else if (selection[against2] == selection[index])
//                selection[against2] = previous[index];
//        }
//    }
}
