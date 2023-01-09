# DOTSTest(확인용도)

## 목적 

기존의 MonoBehaviour를 사용하여 작업했던걸 DOTS로 작업할수 있을만큼 알아가기.

## 할려는 의도
2017년도 DOTS를 처음 써보고 2022년도에 Entity 1.0 버전이 나온겸. 제대로 써볼려고 확인할려는 의도를 가짐.

## 메뉴얼
https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/DOTS_Guide


## 문제점

1. 완성품이 안되있는 느낌이다. 일반적인 API를 사용하는것처럼 하면 좋지만 따로 연구하고 알아봐야되는 사항이 많다.
2. 메모리가 많이 사용된다. (VS 2.5G, UnityEditor 2.5G.. 더많은 메모리! I need more Memory!!)
3. 알거 같은데 확실하지가 않음. 테스트를 진행해서 알아가야될 구간이 많음. 특성을 알아서 그 특성에 맞게 처리해줘야되는데 말이죠.
4. 몇몇 구간은 갑자기 띄어져있음  이게 그건가? 연결되는건지 아닌건지 체크해야되는 이상한상태가..
5. 시대방향 : DOTS(매니저기반 중앙처리) => OOP(너가 알아서 해) => DOTS(헛 다시 돌아왔나)
6. OOP만 다루었던분은 머리가 아플수도.. ~~(봉합중)~~

## 소감 

1. 2017년도 DOTS 작동원리에 비해 많이 바뀜. 
2. 릴리즈에 쓰기엔 일반API처럼 간단하게 쓸수가 없음. 따로 추가연구&공부해야됨.
3. 제대로 못쓸거 같음. 이해 안되는 구간이 좀 있음. 몇구간은 내부에서 두번이상 건너뛰어서 작동되는거 때문에 유추하기가 힘듬.
4. 성능 위해서는 Isystem을 채택하고 작업하기. GC안 쌓이고 속도도 높으며, burst로 컴파일러 최적화도 가능하다.
5. DOTS Guide 깃허브를 보는걸 추천. 지금 정리한건 필요한 종류만 빠르게 쓸려고 정리해놓은 상태이며 정확하지 않은게 존재.
6. 메뉴얼을 무조건 봐라. 따로 정리를 해났지만 가이드라인을 안보고 현 글을 의존하는건 위험.
+ ~~아 MonoBehaviour를 ECSMonoBehaviour같은 클래스를 상속받으면 자동적으로 처리하게 만들어달라~~


## 주의사항 
1. 메모리용량이 작으신 컴퓨터는 사용 안하시는걸 추천드립니다.
2. 유니티를 처음사용하거나 초보개발자 분들은 사용 안하시는 걸 추천드립니다.
3. 컴퓨터가 느린건지 몰라도 IDE에서 에러 표시되는게 엄청 느리다.
4. 유니티의 몇 버전이상부터 네임스페이스가 UnityEngine에서 Unity로 변경됨

---

자- 시작하자. ~~(삽질을 좀 했다.)~~

# 설치법

## 프로젝트 생성시

### Entity 1.0 버전부터 필수사항

1. 2022.2.0b8 (이 버전 이상부터 지원하니 낮은버전일시 확인하기)
2. Visaul Studio 2022 (지원하는 일정이상의 IDE)

### Mode
1. 3D URP는 에러 안 생김
2. 2D URP에 지원을 안해서 에러가 생기는듯함 => 근데 난 이거해야되는데 2D개발자라구. 

(패키지가서 코드수정해도 수정안되니 안심해라. ~~먼저 삽질했다.~~)

## 프로젝트 생성 후 설정
1. 상단 Window -> PackageManager 
2. 우측 +버튼을 클릭

![image](https://user-images.githubusercontent.com/44671731/206753074-023c87b1-d013-400f-80f3-c7a087475295.png)

3. Add Package by name를 클릭

![image](https://user-images.githubusercontent.com/44671731/206753411-f46dc45a-bc0c-46e3-b66f-e145a75c0346.png)

---
### PackageManager로 설치해야될것

#### 3D일시 
1. com.unity.entities
2. com.unity.entities.graphics  => Hybrid Renderer였던것

#### 2D일시 (작동안됨)
1. com.unity.2d.entites => 2D 엔티티
2. com.unity.2d.entities.physics =>2D 엔티티는 아직 릴리즈가 아니니 별개로 작업

---
### 프로젝트 설정
1. 상단 Editor -> Project Settings ->Editor
2. Enter play mode settings  구역을 찾는다.
3. 아래 설정처럼 지정

![image](https://user-images.githubusercontent.com/44671731/206756140-d4eb78cd-a988-419e-a88a-f18b9a063c95.png)

> Enter play mode Options => On

> reload Domain => false

> reload scene => false

> disalbe scene backup => on

---

# ECS 관련 알아가도 좋을것들

상단 Window -> Entities -> 엔티티관련 지원하는 창들이 존재

![image](https://user-images.githubusercontent.com/44671731/206759145-74f94241-6425-4112-9699-b462e4b7db48.png)

## 주로 쓰는창
1. Entity용 하이어라키
2. Entitiy Systems => 로직이 돌아가는 구역. 엔티티가 해당로직으로 몇개가 돌아가는지 볼수 있다. System을 껏다킬수도 있다.
3. Inspector (우측상단 클릭하면 모드 변경가능)Runtime, Automatic이 있는데 Runtime으로 하면 Entity용 틀이 따로 생김 

![image](https://user-images.githubusercontent.com/44671731/206759368-f4782029-7406-4235-aa75-6caa6945b3ff.png)

![image](https://user-images.githubusercontent.com/44671731/206759558-10add7cd-4830-4627-ae01-17ed5f856626.png)

### Entity용 서브씬을 제작해야됨.

![image](https://user-images.githubusercontent.com/44671731/206838108-662e0bda-8bdd-4779-b611-d8f1c846bdde.png)


## Hybrid Renderer는 Entities Graphics가 됨
이 패키지의 의미에 대한 혼란을 줄이기 위해 이름이 변경. 일부 클래스 이름 변경 외에는 리팩토링이 예상되지 않는다함.

## 게임창에서는 보이지만 씬창에서는 움직이는게 안보이는걸로 보아 별개로 작동됨.

## 편집기 데이터 모드 
저작 시간과 런타임에 완전히 다른 표현을 가진 데이터를 사용하여 제자리에서 작업하기 위한 완전히 새로운 패러다임입니다.

## 플레이 모드에서 하위 장면 비파괴 편집 
플레이하는 동안 안전한 방식으로 게임을 영구적으로 변경할 수 있습니다.

## 새 저널링 창
편집기에서 직접 ECS 이벤트를 기록하고 탐색할 수 있으므로 복잡한 디버깅 설정 없이 특정 엔터티가 거친 모든 단일 변환을 추적할 수 있습니다.

## Convert는 Baker로 됩니다 . 이러한 API는 ECS 기반 게임 플레이를 위한 GameObject 기반 데이터의 사용을 단순화하도록 개선되었습니다.

## Build Configs 워크플로 통합
이전에는 기본 Unity 워크플로에 병합된 별도의 빌드 구성 프로세스가 있었습니다.

## 새로운 변환
API 세트를 더 단순하게 만들고 더 많은 사용 사례에 더 적합하도록 변환 관리를 위한 주요 변경 사항을 도입하고 있습니다.

## Simpler ForEach
ECS용 게임 플레이 코드 작성 경험을 지속적으로 단순화하여 엔티티 루핑 및 중첩 루프에 대한 상용구 코드를 직접적으로 줄입니다.

## Aspects로 더 쉬운 게임 플레이 코드
이 새로운 개념은 일반적으로 필요한 상용구 코드를 많이 제거하여 ECS로 게임 플레이 코드를 더 쉽게 구축할 수 있도록 합니다.

## 활성화 가능한 구성 요소로 아키타입 수 줄이기
효율적인 구성 요소 데이터 디자인과 시스템 전략의 균형을 맞추는 최상의 방법을 제공합니다.

## 코드의 버스트 적용 범위 증가
시스템 및 작업에서 관리 코드의 영향을 줄이기 위해 ISystem 및 IJobEntity 사용이 확장되었습니다.

## 고성능을 원하면 System에서 SystemI를 사용하고 Burst와 Job을 집어넣어야됨. SystemBase는 Burst가 불가능하다. GC도 존재. SystemI는 반대다.

---

# 요소
DOTS는 ECS(Entity Component System), C# Job Syetem, Burst Compiler로 나뉘어져 있다.

간단히 설명하면 메모리, 멀티스레딩, 컴파일러관련 속도최적화 기술들을 사용하여 고성능으로 제작 할수 있다.

(더 많은 오브젝트! 더 빠른 처리스피드! 렉이 없어! 엄청나! 더많은 컨텐츠!)


## Entity
Entity(Struct) 개체

## Component
IComponentData(Struct)(데이터, 프로퍼티) 속성

IAspect(readonly partial Struct)(펀션, 기능, 함수, 메서드) 행위

## System
SystemBase(partial Class) 매니지드 메모리 GC쌓임. Burst 불가

ISystem(partial Struct) 언매니지드 메모리 GC안 쌓임. Burst 가능

## Job
IJobEntity(partial Struct)

## Burst
[BurstCompile] (어트리뷰트) System, System안 함수들, Job에 부착

자세한건 https://docs.unity3d.com/Packages/com.unity.burst@1.8/manual/index.html

---

## 기존의 MonoBehaviour와 비교할시
Entity = GameObject(class가 struct로 변경. 내부는 ID 저장용 인덱스말고는 없기 때문에 태그용 IComponentData를 제작해줘야됨)

EntityManager = GameObject.Func(기능들의 모집. 매니저로 컴포넌트변환)

SystemAPI = UnityEngine(Time.time 같은 메인스레드에서 돌리는 종류들이 있는 집합소)

float2, float3 = Vector2, Vector3 (형식이 달라짐)

Mathf = math (System Math하고 이름이 같기때문에 소문자로 한듯)

---

> # 현재 메뉴얼한 코드구조
+ 유니티메뉴얼이나 유튜브에서 찾아보니 대부분 따로 클래스파일을 만들어서 작업한다.
+ 하지만 난 많은 파일이 생기는걸 싫어하기떄문에 하나의 클래스파일에 넣는방법이 좋다고 생각함.
+ (따로 분리시 6개의 파일을 생성해야한다.)

---
> ## 클래스 파일 안의 작성순서
1. MonoBehaviour          => 기본적으로 유니티를 다루던 클래스//데이터생성해주기
2. IComponentData         => 데이터만 존재하는 형태
3. Baker                  => MonoBehaviour에 있는 데이터를 IComponentData에 베이킹해주는 클래스
4. IAspect                => IComponentData는 데이터만 존재하는 형태이고 IAspect는 행동만 존재하는 형태. CPU
5. SystemBase or ISystem  => Entity안의 IComponentData를 IAspect를 사용하여 로직을 돌릴때 job 또는 메인스레드에서 돌리는 형태
6. IJobEntity             => Job으로 돌리기 위한 형태

---

## 각 구성품에 대해 알아둬야할것 (※코드첨부)

> ### MonoBehaviour
+ 베이킹할 대상이다. 데이터만 선언해주기
```C#
public class DOTSRuleScript : MonoBehaviour
{
     public GameObject targetPrefab;
     public Unity.Mathematics.Random random = new Unity.Mathematics.Random(1);
     public float min;
     public float max;

     public Vector2 vector2;
     public Vector3 vector3;
}
```

> ### IComponentData
+ MonoBehavior안의 데이터를 Baker를 통해 집어넣는 데이터만 존재하는 형태이다. 함수쓰지 말기
```C#
public struct DOTSRuleData : IComponentData, IEnableableComponent
{
    public Entity targetPrefab;
    public Unity.Mathematics.Random random;
    public float min;
    public float max;
    
    public float2 vector2;
    public float3 vector3;
}
```

> ### Baker
+ MonoBehavior안의 데이터를 IComponentData에 매핑하는 과정을 한다. 수동적으로 일일히 만듬
+ 모노비헤이비어에서 가진 걸 데이터에 가져와서 대입
+ 마지막에 컴포넌트 추가할때 Null이 가능한 객체는 불가
```C#
public class DOTSRuleBaker : Baker<DOTSRuleScript>
{
    /// <summary>
    /// 베이킹할 객체를 들고옴
    /// </summary>
    /// <param name="authoring">만들 대상 MakingTarget</param>
    public override void Bake(DOTSRuleScript authoring)
    {
        DOTSRuleData data = new DOTSRuleData();
        data.targetPrefab = GetEntity(authoring.targetPrefab);
        data.random = new(1);
        data.max = authoring.max;
        data.min = authoring.min;        
        
        data.vector2 = authoring.vector2;
        data.vector3 = authoring.vector3;

        //값형식만 가능//null이 가능한거는 불가
        AddComponent(data);
    }
}
```

> ### IAspect
+ IComponentData가 데이터만 집어넣는다면 IAspect는 기능만 집어넣는다. (기능, 펑션, 메세지, 메소드)
+ IAspect는 CPU에 가장 적합한 데이터레이아웃을 유지할수 있게 해주며 유지관리할수 있는 인터페이스를 제공
+ IAspect는 시스템에서 컴포넌트 코드를 구성하고 쿼리를 단순화하는 데 유용.
+ 룰1) 파라미터값은 최대 7개로 제한. (T1, T2, ~~~, T7)(여러개 쓸려면 여러 다른 Aspect를 만들어  담아주기)
+ 룰2) RefRO//오직읽기 RefRW//읽기쓰기
+ 룰3) readonly partial
+ 룰4) SystemAPI를 내부에서 사용불가한 지역
+ 룰5) IAspect구조는 적어도 하나 이상의 RefRO,RefRW 형식, 다른 IAspect를 집어넣어야됨. 안넣으면 에러
+ 룰6) IAspect내부에서 SystemAPI.Time은 작동하지 않음 (고로 파라미터로 외부에서 받아와 캐싱을 권장)
+ 룰7) [ReadOnly] 밑에 처럼하면 RefRW => RefRO로 변함
+ 룰8) [Optional] 구성요소가 존재하는지 확인할수 있게 변함(리드온리)
```C#
public readonly partial struct DOTSRuleAspect : IAspect
{
    private readonly Entity entity;
    private readonly TransformAspect transformAspect;
    [Optional]//구성요소가 존재하는지 확인할수 있게 변함//리드온리만
    //private readonly RefRO<SpeedStructure> speed;
    private readonly RefRW<DOTSRuleData> dotsRuleData;
    
    public void CheckFunc(float deltaTime)
    {
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
```

> ### SystemBase, SystemI
+ 로직이 관리되는 구역이다.
+ 룰1) partial
+ 룰2) 메인스레드에서 작동.
+ 이 구역에서 Job을 생성해서 처리 가능. (추천 Run, Schedule, ScheduleParallel)
+ state.EntityManager 안에는 GameObject의 함수들이 존재한다. Add, Remove, Set, Enalbe 등 (MonoBehavior. 에서 MonoBehavior()로 변경된느낌)
+ SetComponentEnabled는 컴폰너트를 추가 및 제거하는 것과 달리 구조적변화를 일으키지 않음.(성능에 문제)
+ ECS는 엔터티가 엔터티쿼리와 일치하는지 확인할 때 비활성화된 컴포넌트를 엔터티에 해당 컴포넌트가 없는 것처럼 처리
+ 엔티티 구조변경했을시 성능이 떨어짐. (Create, Destroy, Add, Remove)
+ state.EntityManager는 DOTS 특성으로 매니저에서 모든걸 가지며 모든걸 처리하는 과정을 담당함.

> ### System이벤트 생명주기

![SystemEventOrder](https://user-images.githubusercontent.com/44671731/206775274-e1c8bda1-63fc-47bf-9344-9b37f446eb25.png)

1. OnCreate: ECS가 시스템에서 생성될때 호출
2. OnStartRunning: OnUpdate에 대한 첫 번째 호출 전과 시스템 실행이 재개될 때마다 호출
3. OnUpdate: 모든 프레임 시스템에서 할일이 있으면 호출. 자세한 내용은 DOTS가이드 참조
4. OnStopRunning: On Destroy 전에 호출되었습니다. 시스템의 RequireForUpdate와 일치하는 엔티티가 없거나 시스템의 Enabled 속성을 false로 설정한 경우 발생하는 시스템 실행이 중지될 때도 호출됩니다. RequireForUpdate를 지정하지 않은 경우 비활성화하거나 삭제하지 않는 한 시스템이 계속 실행됩니다.
5. OnDestroy: ECS가 시스템에서 파괴될때 호출

```C#
//[UpdateAfter(typeof(MoveISystem))]//대상후애 업데이트하기
//[UpdateBefore(typeof(MoveISystem))]//대상전에 업데이트하기
//[UpdateInGroup(typeof(MoveISystem))]//업데이트를 그룹으로 묶음
[BurstCompile]//버스트는 유니티의 잡시스템과 같이 작동되도록 설계 되있음.
//[RequireMatchingQueriesForUpdate]//시스템안에 쿼리가 비어있으면 업데이트를 안하게 만듬
public partial struct DOTSRuleISystem : ISystem
{
    //컴포넌트 배열 및 엔티티 ID 배열입니다.
    //보다는 업데이트할 때마다 다시 검색합니다.?
    //일반적으로 쿼리를 캐시하고 핸들을 입력하는 것이 좋습니다.
    private EntityQuery myQuery;//쿼리
    //private ComponentTypeHandle<DOTSRuleData> dOTSRuleHandle;
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
        //dOTSRuleHandle = state.GetComponentTypeHandle<DOTSRuleData>();
        //EntityTypeHandle entityHandle = state.GetEntityTypeHandle();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state){}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //각 업데이트에서 사용하기 전에 유형 핸들을 업데이트를 해야됨
        //DOTSRuleHandle.Update(ref state);
        //entityHandle.Update(ref state);


        //여러 엔터티의 구성 요소를 가져오고 설정하려면 쿼리와 일치하는 모든 엔터티를 반복하는 것이 가장 좋다.
        //현 시스템이 속한 월드의 엔티티 관리자입니다.
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


        //아키텍쳐를 정의//여기도 캐싱할 수있으면 하는게 괜찮아보임
        //프리팹생성과 비슷//Allocator => NativeArray에 대한 할당 유형을 지정하는 데 사용
        //var types = new NativeArray<ComponentType>(3, Allocator.Temp);//3크기에 임시할당
        //types[0] = ComponentType.ReadWrite<DOTSRuleData>();
        //EntityArchetype archetype = em.CreateArchetype(types);//아키텍쳐 생성

        //// 앞에 정의한 아키텍쳐를 사용하여 두 번째 엔티티를 작성합니다.
        //Entity entity2 = em.CreateEntity(archetype);

        //// 두번째 엔티티를 사용해서 새로운 세번째 엔티티를 생성
        //Entity entity3 = em.Instantiate(entity2);


        //경쟁조건에 문제가 생길수 있다
        //각각의 스레드가 동일한구간에 엑세스하고 수정할려고 하기 떄문
        //이럴 경우 할수 있는건 단순히 로직을 분할하는것
        //SystemAPI관련한건 잡으로 주기전에 가져와서 세팅해주기

        //초기에 한개씩있는건지, 베이킹되있는것중에 첫번쨰거를 가져오는건지
        //따로 한개있는지 여러개 있는 함수도 존재. 첫번째거 가져옴
        RefRW<RandomStructure> randomComponent = SystemAPI.GetSingletonRW<RandomStructure>();
        float deltaTime = SystemAPI.Time.DeltaTime;

        //JobHandle: 예약된 작업에 접근하기 위한 핸들입니다. JobHandle인스턴스를 사용하여 작업 간의 종속성을 지정할 수도 있습니다.
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

        //.Run();//작업이 실행하면 이 작업이 실행되는 메인스레드에서 코드를 실행
        //.Schedule();멀티 단일스레드
        //.ScheduleParallel();//멀티 병렬로 예약, 여러스레드에서 코드를 실행//100개를 줫을때 스레드가 10개면 10개씩 일감을 줌
    }
}
```

> ### IJob, IJobEntity(추천), IJobChunck
+ Job시스템에 일감을 주기 위한 형태
+ 이리저리봐도 JobEntity가 제일 괜찮아보임
+ 룰1) 파라미터값은 최대 7개로 제한. (T1, T2, ~~~, T7)(여러개 쓸려면 여러 다른 Aspect를 만들어  담아주기)
+ 룰2) Job 내부에서 SystemAPI.Time은 작동하지 않음 (고로 파라미터로 외부에서 받아와 캐싱을 권장)
+ 룰3) partial
+ 룰4) SystemAPI를 내부에서 사용불가한 지역
+ 성능을 더 원하면 IJobChunk를 쓸것. struct형태가 16kb 초과되면 자체적으로 돌아가면서 힙영역에 들어가는것때문에 16kb 이하로 제한.
+ Execute 함수 한개만 존재

```C#
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
```

---
# 그외에 알아도 좋은 것들 (정리안된 구역)

## 게임오브젝트에 있는 태그처럼 하는방법. 비어있는 IComponentData 제작
1. 태그 컴포넌트 IComponentData를 상속하되 안에는 아무것도 안씀
2. 개념적으로 태그 구성 요소는 GameObject 태그 와 유사한 목적을 수행하며 태그 컴포넌트가 있는지 여부에 따라 엔터티를 필터링
```C#
public struct TagComponent : IComponentData
{
}
```

> ## 월드(세계)(중요)
+ 월드는 엔티티들의 집합체이다. 엔티티의 ID 번호는 월드에서 유니크하게 존재함
+ 월드는 엔티티매니저 구조를 가지고 있다. 월드에서 엔티티매니저를 사용하여 생성, 파괴, 수정을 할수 있다.
+ 월드는 여러 시스템들을 소유하며, 보통 동일한 월드내의 엔티티에만 접근한다.
+ 추가적으로 월드내의 엔티티 집합에서 동일한 컴포넌트 타입들은 하나의 아키텍쳐에 함께 저장되고, 프로그램의 컴포넌트가 메모리에서 구성되는 방식을 결정.

> ## 아키(원형)(중요)
+ 동일한 컴포넌트들 집합을 가진 월드의 모든 엔티티는 아키텍쳐에 함께 저장.
+ 엔티티에서 컴포넌트를 추가, 제거하면 월드가 EntityManager의 엔티티를 적절한 아키텍처로 이동.
+ 예를 들어 엔티티에 컴포넌트 A, B, C가 있고 해당 B 컴포넌트를 제거하면 EntityManager에서 해당 엔티티를 컴포넌트 유형 A, C가 있는 아키턱쳐로 이동.
+ 이러한 아키텍쳐가 없으면 EntityManager에서 생성합니다.
+ 월드의 엔티티 중에 A, B를 가지고 있는 컴포넌트가 다수일 경우. 컴포넌트 A, B가 있는 엔티티들의 아키텍처는 하나이며 같은 아키턱쳐를 공유함.
+ 아키텍처는 월드가 파괴될 때만 파괴됩니다.
+ 엔터티의 원형 기반 구성은 구성 요소 유형별로 엔터티를 쿼리하는 것이 효율적이라는 것을 의미한다. 
+ 예를 들어 컴포넌트들이 A와 B인 모든 엔터티를 찾으려는 경우 해당 구성 요소 유형이 있는 모든 원형을 찾을 수 있다. 
+ 이는 모든 개별 엔터티를 검색하는 것보다 더 효율적임. 
+ 세계의 기존 원형 세트는 프로그램 수명 초기에 안정화되는 경향이 있으므로 더 빠른 성능을 얻기 위해 쿼리를 캐시를 추천.


> ## SystemAPI
+ 데이터 반복 : 쿼리와 일치하는 엔터티당 데이터를 검색합니다.
+ 쿼리 작성 : 작업을 예약하거나 해당 쿼리에 대한 정보를 검색하는 데 사용할 수 있는 캐시된 EntityQuery 를 가져옵니다.=> 캐싱이 맞아보임
+ 데이터 액세스 : 구성 요소 데이터, 버퍼 및 EntityStorageInfo 를 가져옵니다 .
+ 싱글톤 액세스 : 싱글 톤 이라고도 하는 데이터의 단일 인스턴스를 찾습니다 . 
+ SystemAPI메서드는 시스템에 캐시되어 OnCreate호출 .Update전에 호출됩니다. 또한 이러한 메서드를 호출하면 ECS는 조회 액세스 권한을 얻기 전에 호출이 동기화되는지 확인합니다
+ 시스템에 돌아가는 모든건 메인스레드에서 돌아가고 시스템을 사용하여 작업을 예약할수 있고 해당작업은 스레드에서 실행됨
+ 시스템의 메서드는 프레임당 한 번 실행
+ 시스템을 생성하는 부트스트래핑 프로세스라고 하는 별개의 프로세스가 존재
+ 시스템 위치도 조절가능 UpdateAfter
+ 시스템은 2가지로 구성요소데이터를 수정, 논리를 저장.
+ 버스트는 밸류타입에만 가능하고 참조타입은 불가능
+ 시스템은 한 세계 의 엔터티만 처리할 수 있으므로 시스템은 특정 세계와 연결됩니다. 속성을 사용 World하여 시스템이 연결된 세계를 반환할 수 있습니다.
+ 한 개의 월드는 하나의 엔티티매니저 구조를 가지고 있다.

## 엔터티를 만들거나 삭제할 때 이는 애플리케이션의 성능에 영향을 미치는 구조적 변경 입니다. 자세한 내용은 구조 변경 문서를 참조.

> ## 컴포넌트 데이터반복 
```C#
//SystemAPI.Query
public void OnUpdate(ref SystemState state)
{
    float deltaTime = SystemAPI.Time.DeltaTime;

    foreach (var (transform, speed) in SystemAPI.Query<RefRW<LocalToWorldTransform>, RefRO<RotationSpeed>>())
        transform.ValueRW.Value = transform.ValueRO.Value.RotateY(speed.ValueRO.RadiansPerSecond * deltaTime);
}
//foreach
foreach (var (transform, speed, entity) in SystemAPI.Query<RefRW<LocalToWorldTransform>, RefRO<RotationSpeed>>().WithEntityAccess())
{
    // Do stuff;
}
```  

> ## EntityCommandBuffer
+ 변경 사항을 즉시 수행하는 대신 엔터티 데이터 변경 사항을 대기열에 추가하려면 EntityCommandBuffer스레드로부터 안전한 명령 버퍼를 생성하는 구조체를 사용할 수 있습니다. 
+ 이는 작업이 완료되는 동안 구조변경을 연기하려는 경우에 유용합니다.
+ 고로 잡에서 특정작업을 예약하기. 작업할때 딱좋음

# 나중에 테스트 및 추가연구 진행해볼것
1. 구조처리
2. 피직스 함수
3. 엔티티를 가져와서 채크하는 방식 체크 (중간 연결구역이 최소2개이상 띄어져있어서 어딘지 잘모름)
4. 익스큐트애 들어가는 위치 체크
5. 익스큐트랑 foreach랑 어디가 비슷해보이는데 좀더 체크
6. 스트리밍장면이라는 기능이 있는데 설명 대로면 비동기씩 나오는방식같다
7. 그러면...10개를 하면 나중에 다시 그려도 10개인지 테슽 ==========



## 참고자료
https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/index.html



