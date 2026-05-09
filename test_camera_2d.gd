extends Camera2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

@export var allowHorizonalInput : bool = true;

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var horizontalInput : Vector2 = Vector2.ZERO;
	if allowHorizonalInput: horizontalInput = Input.get_vector("leftward","rightward","forward","backward")
	var verticalInput = Input.get_axis("upward","downward")
	
	position += horizontalInput * delta * 1000.0 / zoom.length();
	zoom += Vector2.ONE * verticalInput * delta * zoom.length();
	
	CameraScale.SetCameraScale(zoom)
