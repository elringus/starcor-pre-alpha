#pragma strict

var explosions:GameObject[];

function OnGUI(){

	GUI.Label(Rect(0,0,200,20),"F1 = Creation of Explode 01");
	GUI.Label(Rect(0,25,200,20),"F2 = Creation of Explode 02");
	GUI.Label(Rect(0,50,200,20),"F3 = Creation of Explode 03");
	GUI.Label(Rect(0,75,200,20),"F4 = Creation of Explode 04");
	GUI.Label(Rect(0,100,200,20),"F5 = Creation of Explode 05");

}

function Update(){

	var position:Vector3;
	
	position = transform.TransformPoint(Vector3(0,1,10));
	
	if (Input.GetKeyDown( KeyCode.F1)){
		Instantiate( explosions[0],position,Quaternion.identity);
	}
	if (Input.GetKeyDown( KeyCode.F2)){
		Instantiate( explosions[1],position,Quaternion.identity);
	}
	if (Input.GetKeyDown( KeyCode.F3)){
		Instantiate( explosions[2],position,Quaternion.identity);
	}
	if (Input.GetKeyDown( KeyCode.F4)){
		Instantiate( explosions[3],position,Quaternion.identity);
	}
	if (Input.GetKeyDown( KeyCode.F5)){
		Instantiate( explosions[4],position,Quaternion.identity);
	}
}