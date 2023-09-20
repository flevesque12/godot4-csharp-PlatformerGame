using Godot;
using System;

public partial class firstscript : Node
{
	public float testfloat = 10.1f;
	public string testString = "HEllo godot ";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(testString+testfloat);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
