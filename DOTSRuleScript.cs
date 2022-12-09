using System;
using System.Collections;
using System.Xml;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace lLCroweTool.Test.DOTS
{
    //목적 : 기존의 MonoBehaviour를 사용하여 작업했던걸 DOTS로 작업할수 있을만큼 알아가기


    //https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/DOTS_Guide
    //DOTS//1. ECS, 2. JobSystem, 3. BurstSystem
    //메모리 많이씀 유니티에디터에서 2.5G, 비쥬얼스튜디오에서 2.5G 사용

    //-=-=정리만하고 나중에 보기//지금은 하기 힘듬
    //E => Entity(Struct)
    //C => IComponentData(Struct)(데이터 프로퍼티 속성), IAspect(readonly partial Struct)(펀션, 기능, 함수, 메서드) => MonoBehaviour > Baker > Data
    //S => SystemBase(partial Class), ISystem(partial Struct)
    //JOB => IJobEntity(partial Struct), JobHandle
    //Burst => [BurstCompile]



    //-=-=기존의 MonoBehaviour와 비교할시
    //Entity = GameObject(struct로 변경된);
    //EntityManager = GameObject(기능들이 모집)
    //SystemAPI = UnityEngine(Time.time 종류들)
    //float2 float3 = Vector2 Vector3





    //아직 잘몰라서 체크할거
    //구조처리, 
    //피직스 함수,
    //엔티티를 가져와서 채크하는 방식 체크
    //익스큐트애 들어가는 위치 체크
    //익스큐트랑 foreach랑 어디가 비슷해보이는데 좀더 체크


    public class DOTSRuleScript : MonoBehaviour
    {
        //기존의 모노비헤이비어로 만든 객체
        public GameObject targetPrefab;
        public Unity.Mathematics.Random random = new Unity.Mathematics.Random(1);
        public float min;
        public float max;
        public int index;

        public Vector2 vector2;
        public Vector3 vector3;
    }

    public struct DOTSRuleData : IComponentData, IEnableableComponent
    {
        //타겟팅할 모노비헤이비어 객체에 있는 데이터와 동일하게
        //설정할것만 집어넣기
        public Entity targetPrefab;

        public Unity.Mathematics.Random random;
        public float min;
        public float max;
        public float2 vector2;
        public float3 vector3;
        public int index;
    }


    //엔티티로 굽기
    public class DOTSRuleBaker : Baker<DOTSRuleScript>
    {
        /// <summary>
        /// 베이킹할 객체를 들고옴
        /// </summary>
        /// <param name="authoring">만들 대상MakingTarget</param>
        public override void Bake(DOTSRuleScript authoring)
        {
            //베이커는 모노비헤이비어 컴포넌트를 엔티티로 만듬
            //모노비헤이비어에서 가진 걸 데이터에 가져와서 대입
            DOTSRuleData data = new DOTSRuleData();
            data.targetPrefab = GetEntity(authoring.targetPrefab);
            data.max = authoring.max;
            data.min = authoring.min;
            data.index = authoring.index;
            data.random = new(1);
            data.vector2 = authoring.vector2;
            data.vector3 = authoring.vector3;

            //값형식만 가능//null이 가능한거는 불가
            AddComponent(data);
        }
    }

    //RefRO//오직읽기
    //RefRW//읽기쓰기
    //리드온리로만 설정할수 있음//유니티요구사항
    //IAspect는 CPU에 가장 적합한 데이터레이아웃을 유지할수 있게 해주며 유지관리할수 있는 인터페이스를 제공
    public readonly partial struct DOTSRuleAspect : IAspect
    {
        //7개의 파라미터값으로 제한되있음
        //왜냐하면 SystemAPI, Entity.ForEach의 T1,T2같은 요소들이 7개로 제한 되있기 때문에
        //여러개 쓸려면 여러 Aspect를 만들어 처리해주기
        //SystemAPI를 내부에서 사용불가한 지역

        private readonly Entity entity;
        //Aspect구조는 적어도 하나 이상의 RefRO,RefRW 형식,
        //다른 IAspect를 집어넣어야됨//안넣으면 에러 뺵-




        //private readonly TransformAspect transformAspect;
        //[Optional]//구성요소가 존재하는지 확인할수 있게 변함//다시확인해봐야됨//리드온리만
        private readonly RefRO<SpeedStructure> speed;
        private readonly RefRW<DOTSRuleData> dotsRuleData;


        // DynamicBuffer<Waypoint> 구성 요소 유형을 정의합니다.
        // Waypoint 요소의 확장 가능한 배열입니다.
        // 내부 버퍼 용량은 다음당 요소 수입니다.
        // 청크에 직접 저장된 도면요소(8에 해당)
        //잘모르겠다..//아직은 안쓸뜻함

        //[ReadOnly]//밑에 처럼하면 RefRW => RefRO로 변함
        //private readonly DynamicBuffer<DOTSRuleData> dOTSRuleDatas;

        


        //SystemAPI를 IAspect와 IJob 종류들에서
        //파라미터값으로 외부에서 받아와 저장하거나
        //사용하여
        //
        public void CheckFunc(float deltaTime)
        {
            //에스팩트내부에서SystemAPI.Time을 얻질 못함
            float3 dir = math.normalize(dotsRuleData.ValueRW.vector3 - transformAspect.Position);

            float temp1 = speed.IsValid ? speed.ValueRO.speed : 1;
            float temp2 = dotsRuleData.IsValid ? dotsRuleData.ValueRO.max : 1;
            float temp = dotsRuleData.ValueRO.max;
            transformAspect.Position += dir * deltaTime * temp;
        }

        public void CheckSystemAPIFunc(RefRW<RandomStructure> randomComponent)
        {
            dotsRuleData.ValueRW.random = randomComponent.ValueRO.random;

            
        }

    }


    ////관리되는 상태라서 추적하기 쉬움
    //public partial class DOTSRuleSystemBase : SystemBase
    //{
    //    protected override void OnUpdate()
    //    {
    //        RefRW<RandomStructure> randomComponent = SystemAPI.GetSingletonRW<RandomStructure>();

    //        //SystemAPI와 Entities.ForEach의
    //        //쿼리는 최대 7개로 제한되있음
    //        //더많은 구성요소로 작업할려면 
    //        //IJobEntity, IJobChunk로 사용해야됨

    //        foreach (MoveToPositionAspect moveToPositionAspect in SystemAPI.Query<MoveToPositionAspect>())
    //        {
    //            moveToPositionAspect.CheckFunc(SystemAPI.Time.DeltaTime);
    //        }

    //        //엔티티가 어덯게 작동되는방식인지
    //        Entities.ForEach((TransformAspect transformAspect) =>
    //        {
    //            transformAspect.Position += new float3(SystemAPI.Time.DeltaTime, 0, 0);
    //        }).ScheduleParallel();
    //    }
    //}

    //관리되지 않는 구역이라 추적하기 힘듬
    //윈도우 > 엔티티 > 시스템 에서 볼수 있음
    //[UpdateAfter(typeof(MoveISystem))]//대상후애 업데이트하기
    //[UpdateBefore(typeof(MoveISystem))]//대상전에 업데이트하기
    //[UpdateInGroup(typeof(MoveISystem))]//업데이트를 그룹으로 묶음
    [BurstCompile]//버스트는 유니티의 잡시스템과 같이 작동되도록 설계 되있음.
    //[RequireMatchingQueriesForUpdate]//시스템안에 쿼리가 비어있으면 업데이트를 안하게 만듬
    public partial struct DOTSRuleISystem : ISystem
    {
        // It's generally good practice to cache queries and type handles
        // rather than re-retrieving them every update.

        // 컴포넌트 배열 및 엔티티 ID 배열입니다.

        //보다는 업데이트할 때마다 다시 검색합니다.?
        // 일반적으로 쿼리를 캐시하고 핸들을 입력하는 것이 좋습니다.
        private EntityQuery myQuery;
        //private ComponentTypeHandle<DOTSRuleData> DOTSRuleHandle;
        //private EntityTypeHandle entityHandle;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityManager em = state.EntityManager;
            em.CreateEntityQuery(typeof(LocalToWorld));
            

            //엔티티쿼리를 여기서 캐싱
            //myQuery = GetEntityQuery(typeof(LocalToWorld));


            //var builder = new EntityQueryBuilder(Allocator.Temp);
            //builder.WithAll<DOTSRuleData>();

            //myQuery = state.GetEntityQuery(builder);
            //ComponentTypeHandle<DOTSRuleData> DOTSRuleHandle = state.GetComponentTypeHandle<DOTSRuleData>();

            //EntityTypeHandle entityHandle = state.GetEntityTypeHandle();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //각 업데이트에서 사용하기 전에 유형 핸들을 업데이트를 해야됨
            //DOTSRuleHandle.Update(ref state);
            //entityHandle.Update(ref state);


            //여러 엔터티의 구성 요소를 가져오고 설정하려면 쿼리와 일치하는 모든 엔터티를 반복하는 것이 가장 좋습니다. 아래를 참조하십시오.
            //현 시스템이 속한 월드의 엔티티 관리자입니다.//MonoBehavior라고 생각하면 편함//MonoBehavior. 에서 MonoBehavior()로 변경된느낌
            EntityManager em = state.EntityManager;//게임오브젝트와 동일//중앙관리

            //구성 요소가 없는 새 도면요소를 작성합니다.//MonoBehavior를 생성한다보면 편함
            Entity entity = em.CreateEntity();
            em.AddComponent<DOTSRuleData>(entity);//비어있는 엔티티에 컴포넌트들을 집어넣음
            
            //em.RemoveComponent<DOTSRuleData>(entity);//엔티티에서 특정컴포넌트 삭제
            //em.SetComponentData<DOTSRuleData>(entity, new());//엔티티에 특정 컴포넌트를 집어넣기
            //em.SetComponentEnabled<DOTSRuleData>(entity, false);//엔티티를 활성화, 비활성화시키기//IEnableableComponent 인터페이스가 필요함
            //DOTSRuleData dotsRulsData = em.GetComponentData<DOTSRuleData>(entity);//엔티티에서 특정 컴포넌트를 가져오기
            //bool isHas = em.HasComponent<DOTSRuleData>(entity);//엔티티에서 특정컴포넌트가 존재하는지
            em.DestroyEntity(entity);//엔티티파괴
            

            //아키텍쳐를 정의
            //Allocator => NativeArray에 대한 할당 유형을 지정하는 데 사용

            //프리팹생성과 비슷
            //var types = new NativeArray<ComponentType>(3, Allocator.Temp);//3크기에 임시할당
            //types[0] = ComponentType.ReadWrite<DOTSRuleData>();
            //EntityArchetype archetype = em.CreateArchetype(types);//아키텍쳐 생성

            //// 앞에 정의한 아키텍쳐를 사용하여 두 번째 엔티티를 작성합니다.
            //Entity entity2 = em.CreateEntity(archetype);

            //// 두번째 엔티티를 사용해서 새로운 세번째 엔티티를 생성
            //Entity entity3 = em.Instantiate(entity2);


            //경쟁조건에 문제가 생길수 있다
            //각각의 스레드가 동일한구간에 엑세스하고 수정할려고 하기 떄문
            //이럴경우할수 있는건 단순히 로직을 분할하는것
            //SystemAPI관련한건 잡으로 주기전에 가져와서 세팅해주기


            //초기에 한개씩있는건지, 베이킹되있는것중에 첫번쨰거를 가져오는건지
            //따로 한개있는지 여러개 있는 함수도 존재. 첫번째거 가져옴
            RefRW<RandomStructure> randomComponent = SystemAPI.GetSingletonRW<RandomStructure>();
            float deltaTime = SystemAPI.Time.DeltaTime;


            JobHandle jobHandle = new NormalJob
            {
                deltaTime = deltaTime
            }.ScheduleParallel(state.Dependency);

            // Complete() 메서드는 작업이 수행될 때까지 반환되지 않습니다.
            // 핸들이 실행을 마쳤습니다.
            // 경우에 따라 작업이 완료되었을 수 있습니다.
            // 처리에서 Complete()를 호출하기 전에 실행합니다.
            // 어느 쪽이든, 메인 스레드는 다음과 같이 대기합니다.
            // 작업이 완료될 때까지 호출을 완료합니다.
            jobHandle.Complete();

            new SystemAPIIJob
            {
                randomComponent = randomComponent
            }.Run();//메인스레드에서 처리


            //.Run();//작업이 실행하면 이 작업이 실행되는 메인스레드에서만 코드를 실행
            //.Schedule();멀티 단일스레드
            //.ScheduleParallel();//멀티 병렬로 예약, 여러스레드에서 코드를 실행//100개를 줫을때 스레드가 10개면 10개씩 일감을 줌
        }
    }
    //멀티스레드, 잡관련

    //일반적인 잡처리
    [BurstCompile]
    public partial struct NormalJob : IJobEntity
    {  
        public float deltaTime;
        

        public void Execute(DOTSRuleAspect dOTSRuleAspect)
        {
            //moveToPositionAspect.CheckFunc(SystemAPI.Time.DeltaTime);//엑세스안됨
            dOTSRuleAspect.CheckFunc(deltaTime);//외부에서 설정해서 집어넣을것
        }
    }

    
    //SystemAPI 잡처리
    [BurstCompile]
    public partial struct SystemAPIIJob : IJobEntity 
    {
        //외부에서 랜덤값을 설정한뒤 가져와야지 랜덤으로 가져오지
        //한번돌때 계속가져오면 하나의 값만 반환하니 안됨

        [NativeDisableUnsafePtrRestriction] public RefRW<RandomStructure> randomComponent;
        public void Execute(DOTSRuleAspect dOTSRuleAspect)
        {
            dOTSRuleAspect.CheckSystemAPIFunc(randomComponent);
        }
    }

    //테스트
    [BurstCompile]
    public partial struct TestIJob : IJobEntity
    {
        //외부에서 랜덤값을 설정한뒤 가져와야지 랜덤으로 가져오지
        //한번돌때 계속가져오면 하나의 값만 반환하니 안됨



        //IJobEntity일시 지원되는 매개변수에 제한이 있음
        //IComponentData
        //ICleanupComponentData
        //ISharedComponent
        //Managed components
        //Entity
        //DynamicBuffer<T>
        //IAspect
        //int





        public void Execute(ref DOTSRuleData data)
        {
            
        }
    }

    [BurstCompile]
    public partial struct TestIJob2 : IJobEntity
    {
        //외부에서 랜덤값을 설정한뒤 가져와야지 랜덤으로 가져오지
        //한번돌때 계속가져오면 하나의 값만 반환하니 안됨

        public void Execute(ref DOTSRuleData data)
        {
            
        }
    }

    public struct TagComponent : IComponentData
    {

    }
}