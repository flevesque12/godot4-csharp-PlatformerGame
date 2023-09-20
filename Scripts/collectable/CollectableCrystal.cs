using Godot;
using System;
using System.Diagnostics;


public partial class CollectableCrystal : Area2D
{
    public int point = 100;

	private void OnBodyEntered(PhysicsBody2D body2D){
        //GD.Print("Entred collider");
        //body2D.Scale *= new Vector2(1.5f, 1.5f);
        
        QueueFree();
    }

    public int GetPoint(){
        return this.point;
    }
}
