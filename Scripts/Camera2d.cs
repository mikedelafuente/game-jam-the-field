using Godot;
using System;

public partial class Camera2d : Camera2D
{
    // Define the zoom step for each action
    private readonly Vector2 ZoomStepIn = new Vector2(1.1f, 1.1f);
    private readonly Vector2 ZoomStepOut = new Vector2(0.9f, 0.9f);

    public override void _Ready()
    {
        // Set initial zoom
        this.SetZoom(new Vector2(5.0f, 5.0f));
    }

    public override void _Process(double delta)
    {
        // Check for the "zoom_in" action
        if (Input.IsActionPressed("zoom_in"))
        {
            Zoom *= ZoomStepIn;
        }
        
        // Check for the "zoom_out" action
        if (Input.IsActionPressed("zoom_out"))
        {
            Zoom *= ZoomStepOut;
        }
    }

 
}
