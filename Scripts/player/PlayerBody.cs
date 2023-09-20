using Godot;
using System;

public partial class PlayerBody : CharacterBody2D
{
	public int point = 0;
	public float moveSpeed = 100.0f;
	public float JumpVelocity = 200.0f;
	public float gravity = 500f;
	
	private float wallJumpVelocity = 200.0f;
	private int direction = 0;
	private float acceleration = 0.5f;
	private float friction = 0.1f;

	private float dashSpeed = 500f;

	private bool isDashAvailable = false;
	private bool isDashing = false;
	private bool isInAir = false;
	private bool isOnWallSlide = false;

	private float dashTimer = 0.2f;
	private float dashTimerReset = 0.2f;

	private Vector2 velocity;
	
	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;
		
		if(!isDashing && !isOnWallSlide)
			Movement();

		Jump();
		
		WallSlideWallJump();

		DashAbility((float)delta);

		ApplyGravity((float)delta);

		if(IsOnFloor())
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Offset = new Vector2(0f, 0f);
		
		Velocity = velocity;
		MoveAndSlide();		
	}

	public void ZeroVelocity(){
		Velocity = new Vector2(0f, 0f);
	}

	public void Jump(){
		if(IsOnFloor())
		{
			if(Input.IsActionJustPressed("jump"))
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Jump");
				velocity = new Vector2(velocity.X, -JumpVelocity);
				
				isInAir = true;
			}
			else
			{
				isInAir = false;
			}
			isDashAvailable = true;				
		}
		else
		{
			isInAir = false;
		}
	}

	public void ApplyGravity(float delta){
		//apply gravity here
		if(!IsOnFloor()){
			velocity.Y += gravity * delta;
			isInAir = true;
		}
	}

	public void Movement(){
		direction = 0;

		if(Input.IsActionPressed("ui_left")){
			direction = -1;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
		}
		
		if(Input.IsActionPressed("ui_right")){
			direction = 1;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
		}
		
		//movement here
		if(direction != 0){
			velocity.X = Mathf.Lerp(velocity.X, direction * moveSpeed, acceleration);
			if(!isInAir)
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("run");
		}
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, 0f, friction);
			if(!isInAir)
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Idle");
		}
	}

	public void WallSlideWallJump(){
		//wall slide and wall jump here
		if(GetNode<RayCast2D>("RaycastLeft").IsColliding() && !IsOnFloor()){
			if(Input.IsActionPressed("ui_down"))
			{
				velocity = new Vector2(velocity.X, velocity.Y);	
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wallSlide");	
			}
			else
			{
				velocity = new Vector2(velocity.X, velocity.Y * 0.7f);
				
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wallSlide");
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Offset = new Vector2(9f, 0f);

				//activate wall jump
				if(Input.IsActionJustPressed("jump")){
					velocity  = new Vector2(wallJumpVelocity, -wallJumpVelocity);
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Offset = new Vector2(0f, 0f);
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Jump");
				}
			}
		}
		else if(GetNode<RayCast2D>("RaycastRight").IsColliding() && !IsOnFloor()){
			if(Input.IsActionPressed("ui_down"))
			{
				velocity = new Vector2(velocity.X, velocity.Y);	
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wallSlide");
										
			}
			else
			{			
				velocity = new Vector2(velocity.X, velocity.Y * 0.7f);
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wallSlide");
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Offset = new Vector2(-10f, 0f);

				//activate wall jump
				if(Input.IsActionJustPressed("jump")){
					velocity = new Vector2(-wallJumpVelocity, -wallJumpVelocity);
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Offset = new Vector2(0f, 0f);
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;	
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Jump");
				}
			}
		}
	}

	public void DashAbility(float deltaTime){
		if(isDashAvailable){
			if(Input.IsActionJustPressed("dash")){
				if(Input.IsActionPressed("ui_left")){
					velocity = new Vector2(-dashSpeed, velocity.Y);
					isDashing = true;
				}

				if(Input.IsActionPressed("ui_right")){
					velocity = new Vector2(dashSpeed, velocity.Y);
					isDashing = true;
				}

				if(Input.IsActionPressed("ui_up")){
					velocity = new Vector2(velocity.X, -dashSpeed);
					isDashing = true;
				}

				if(Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up")){
					velocity = new Vector2(dashSpeed, -dashSpeed);
					isDashing = true;
				}

				if(Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up")){
					velocity = new Vector2(-dashSpeed, -dashSpeed);
					isDashing = true;
				}
				dashTimer = dashTimerReset;
				isDashAvailable = false;
			}
		}

		if(isDashing){
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
			dashTimer -= deltaTime;
			if(dashTimer <= 0f){
				isDashing = false;
				velocity = new Vector2(0f, 0f);
			}
		}
	}
	
}
