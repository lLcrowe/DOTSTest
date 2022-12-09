using UnityEngine;
using System.Collections;

using System;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;

namespace Assets.DOTS.DOTSTest
{
    public class UnitMover : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    }

    //Job System을 사용할때 주의해야할 점은 Job을 정의한다고 그게 자동으로 실행되지 않는다는 점이다.
    //언제나 Schedule 함수를 통해 예약을 잡아두면 System이 그 Job을 해치운다는 느낌이다.
    //이러한 구조를 통해 Job을 실행시킬지 안시킬지를 유연하게 설정할 수 있다.

    [Serializable]
    public struct DirectionData2D : IComponentData
    {
        public float2 direction;
    }


    [Serializable]
    public struct HeadingData2D : IComponentData
    {
        public float Heading;
    }

    [Serializable]
    public struct TargetPositionData : IComponentData
    {
        public float2 TargetPosition;
    }

    [BurstCompile]
    public partial class UnitMoverEntity : SystemBase
    {
        private EntityQuery entityQuery;//캐싱
        protected override void OnCreate()
        {
            base.OnCreate();
            var query = new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<DirectionData2D>(),
                    ComponentType.ReadWrite<HeadingData2D>(),
                    ComponentType.ReadOnly<TargetPositionData>()
                },
            };
            entityQuery = GetEntityQuery(query);
        }
        protected override void OnUpdate()
        {
            //업데이트내용
        }
    }

}