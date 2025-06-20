using Godot;
using System;
using System.Collections.Generic;

public partial class CelestialBody : Node3D
{
    // General info
    public string name;
    public double mass;
    public double geeASL;
    public double radius;

    // Orbital info
    public string parentName;
    public Orbit orbit;
    public CartesianData cartesianData;

    public List<CelestialBody> childPlanets = [];

    // Procedural info
    public TerrainGen pqsSphere;
    public List<Node> pqsMods;

    // Miscellaneous info
    public bool isRoot; // only ONE body per save should ever have this be true!
    public string configPath;

    // DEBUG
    public MeshInstance3D debugOrb;

    public void CreateDebugOrb(Node3D parent)
    {
        debugOrb = new MeshInstance3D();
        debugOrb.Mesh = new SphereMesh();
        parent.AddChild(debugOrb);
    }

    public override void _Process(double delta)
    {
        // Propagate the cBody's orbit
        if (orbit != null)
        {
            orbit.trueAnomaly = PatchedConics.TimeToTrueAnomaly(orbit, SaveManager.Instance.saveTime, 0) + orbit.trueAnomalyAtEpoch;
            (Double3 position, Double3 velocity) = PatchedConics.KOEtoECI(orbit);
            cartesianData.position = position;
            cartesianData.velocity = velocity;
            //GD.Print(SaveManager.Instance.saveTime);
            //GD.Print($"{cartesianData.position.X}, {cartesianData.position.Y}, {cartesianData.position.Z}");
            Position = cartesianData.position.GetPosYUp().ToFloat3();
        }
    }
}
