using System.Collections;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;


public class RandomComponent : MonoBehaviour
{
    public Unity.Mathematics.Random random = new Unity.Mathematics.Random(1);
    public float min;
    public float max;
}

public class RandomBaker : Baker<RandomComponent>
{
    public override void Bake(RandomComponent authoring)
    {
        RandomStructure randomStructrue = new();
        randomStructrue.random = new(1);
        randomStructrue.min = authoring.min;
        randomStructrue.max = authoring.max;


        //null값들어가는건 안됨
        AddComponent(randomStructrue);
    }
}
public struct RandomStructure : IComponentData
{
    public Unity.Mathematics.Random random;
    public float min;
    public float max;
}