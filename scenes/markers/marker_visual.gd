extends Sprite2D

@export var markerPingParticles : GPUParticles2D = null;


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	
	scale = Vector2.ONE / CameraScale.GetCameraScale()
	
	if (markerPingParticles != null):
		if (markerPingParticles.process_material is ParticleProcessMaterial):
			var ppm : ParticleProcessMaterial = markerPingParticles.process_material as ParticleProcessMaterial
			
			ppm.scale_max = scale.x
			ppm.scale_min = scale.y
