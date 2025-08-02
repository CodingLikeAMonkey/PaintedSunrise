using Flecs.NET.Core;
using Godot;
using Components.Character;

namespace Systems.Bridge
{
    public partial class PlayerAssignmentBridgeSystem : Node
    {
        [Export] public CharacterBody3D PlayerCharacter;

        private Entity _playerEntity;

        public override void _Ready()
        {
            if (PlayerCharacter != null)
            {
                AssignPlayer(PlayerCharacter);
            }
            else
            {
                GD.Print("PlayerAssignment: No PlayerCharacter assigned, skipping.");
            }
        }

        public void Setup(World world)
        {
            world.Observer<CharacterComponent>()
                .Event(Ecs.OnAdd)
                .Each((Entity entity, ref CharacterComponent character) =>
                {
                    if (_playerEntity.IsValid() && !_playerEntity.Equals(entity))
                    {
                        if (entity.Has<CharacterPlayerComponent>())
                        {
                            entity.Remove<CharacterPlayerComponent>();
                        }
                    }
                });
        }

        public void AssignPlayer(Node3D characterNode)
        {
            if (Kernel.NodeRef<Node3D>.TryGetFromNode(characterNode, out Entity entity))
            {
                entity.Add<CharacterPlayerComponent>();
                _playerEntity = entity;

                GD.Print($"PlayerAssignment: Assigned player to entity {entity.Id}");

                // Load the camera scene
                var cameraScene = GD.Load<PackedScene>("res://02_Assets/Camera/third_person_camera.tscn");
                if (cameraScene != null)
                {
                    var cameraInstance = cameraScene.Instantiate();
                    if (cameraInstance is Node3D cameraNode)
                    {
                        // Add camera as child of the player node
                        characterNode.AddChild(cameraNode);
                        GD.Print("PlayerAssignment: Third person camera added to player.");
                    }
                    else
                    {
                        GD.PushError("PlayerAssignment: Loaded camera scene root is not a Node3D.");
                    }
                }
                else
                {
                    GD.PushError("PlayerAssignment: Could not load third person camera scene.");
                }
            }
            else
            {
                GD.PushError("PlayerAssignment: Could not find entity for the given node.");
            }
        }
    }
}
