# DOTSTest(확인용도)

-=목적 : 기존의 MonoBehaviour를 사용하여 작업했던걸 DOTS로 작업할수 있을만큼 알아가기

-=할려는 의도 : 2017년도 DOTS를 처음 써보고 2022년도에 Entity 1.0 버전이 나온겸. 제대로 써볼려고 확인할려는 의도를 가짐.

-=메뉴얼 : https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/DOTS_Guide

-=문제점

1. 완성품이 안되있는 느낌이다. 일반적인 API를 사용하는것처럼 하면 좋지만 따로 연구하고 알아봐야되는 사항이 많다.
2. 메모리가 많이 사용된다. (VS 2.5G, UnityEditor 2.5G.. 더많은 메모리! I need more Memory!!)
3. 알긴아는데 확실하지가 않음. 테스트를 진행해서 알아가야될 구간이 많음
4. 몇몇 구간은 갑자기 띄어져있음  이게 그건가? 연결되는건지 아닌건지 체크해야되는 이상한상태가..
5. 시대방향 : DOTS(매니저) => OOP(너가 알아서 해) => DOTS

-=소감 : dots 엄청 바뀜. 근데 릴리즈하기엔 일반API처럼 간단하게 쓸수가 없음. 따로 연구&공부해야됨.

-=주의사항 
1. 메모리용량이 작으신 컴퓨터는 사용 안하시는걸 추천드립니다.
2. 유니티를 처음사용하거나 초보개발자 분들은 사용안하신는 추천드립니다.
3. 컴퓨터가 느린건지 몰라도 IDE에서 에러뜨는게 느리다.


자- 시작하자.

※설치법

-=프로젝트 생성시

-=Entity 1.0 버전부터 필수사항

1. 2022.2.0b8 (이 버전 이상부터 지원하니 낮은버전일시 확인하기)
2. Visaul Studio 2022 (지원하는 일정이상의 IDE)

-=Mode
1. 3D URP는 에러안생김 
2. 2D URP에 지원을 안해서 에러가 생기는듯함 => 근데 난 이거해야되는데 2D..


1. 상단 Window -> PackageManager 
2. 우측 +버튼을 클릭

![image](https://user-images.githubusercontent.com/44671731/206753074-023c87b1-d013-400f-80f3-c7a087475295.png)

3. Add Package by name를 클릭

![image](https://user-images.githubusercontent.com/44671731/206753411-f46dc45a-bc0c-46e3-b66f-e145a75c0346.png)

-=설치해야될것
//3D일시 

A. com.unity.entities
B. com.unity.entities.graphics  => Hybrid Renderer였던것

//2D일시 (작동안됨)
A. com.unity.2d.entites => 2D 엔티티
B. com.unity.2d.entities.physics =>2D 엔티티는 아직 릴리즈가 아니니 별개로 작업

-=프로젝트 설정
상단 Editor -> Project Settings ->Editor

Enter play mode settings  구역을 찾는다.

아래 설정처럼 지정

![image](https://user-images.githubusercontent.com/44671731/206756140-d4eb78cd-a988-419e-a88a-f18b9a063c95.png)

Enter play mode Options => On
reload Domain => false
reload scene => false
disalbe scene backup => on

설치끝


※ECS 관련 알아가야될 것들

상단 Window -> Entities -> 엔티티관련 지원하는 창들이 존재
![image](https://user-images.githubusercontent.com/44671731/206759145-74f94241-6425-4112-9699-b462e4b7db48.png)

-=주로 쓰는창
Entity용 하이어라키
Entitiy Systems => 로직이 돌아가는 구역. 엔티티가 해당로직으로 몇개가 돌아가는지 볼수 있다. System을 껏다킬수도 있다.
Inspector (우측상단 클릭하면 모드 변경가능)
Runtime, Automatic이 있는데 Runtime으로 하면 Entity용 틀이 따로 생김

![image](https://user-images.githubusercontent.com/44671731/206759368-f4782029-7406-4235-aa75-6caa6945b3ff.png)

![image](https://user-images.githubusercontent.com/44671731/206759558-10add7cd-4830-4627-ae01-17ed5f856626.png)

-=Entity용 서브씬을 제작해야됨.

-=Hybrid Renderer는 Entities Graphics가 됨 : 이 패키지의 의미에 대한 혼란을 줄이기 위해 이름이 변경. 일부 클래스 이름 변경 외에는 리팩토링이 예상되지 않는다함.

-=게임창에서는 보이지만 씬창에서는 움직이는게 안보이는걸로 보아 별개로 작동됨.

-=편집기 데이터 모드 : 저작 시간과 런타임에 완전히 다른 표현을 가진 데이터를 사용하여 제자리에서 작업하기 위한 완전히 새로운 패러다임입니다.

-=플레이 모드에서 하위 장면 비파괴 편집 : 플레이하는 동안 안전한 방식으로 게임을 영구적으로 변경할 수 있습니다.

-=새 저널링 창 : 편집기에서 직접 ECS 이벤트를 기록하고 탐색할 수 있으므로 복잡한 디버깅 설정 없이 특정 엔터티가 거친 모든 단일 변환을 추적할 수 있습니다.

-=변환은 베이킹이 됩니다 . 이러한 API는 ECS 기반 게임 플레이를 위한 GameObject 기반 데이터의 사용을 단순화하도록 개선되었습니다.

-=Build Configs 워크플로 통합 : 이전에는 기본 Unity 워크플로에 병합된 별도의 빌드 구성 프로세스가 있었습니다.

-=새로운 변환 : API 세트를 더 단순하게 만들고 더 많은 사용 사례에 더 적합하도록 변환 관리를 위한 주요 변경 사항을 도입하고 있습니다.

-=Simpler ForEach : ECS용 게임 플레이 코드 작성 경험을 지속적으로 단순화하여 엔티티 루핑 및 중첩 루프에 대한 상용구 코드를 직접적으로 줄입니다.

-=Aspects로 더 쉬운 게임 플레이 코드 : 이 새로운 개념은 일반적으로 필요한 상용구 코드를 많이 제거하여 ECS로 게임 플레이 코드를 더 쉽게 구축할 수 있도록 합니다.

-=활성화 가능한 구성 요소로 아키타입 수 줄이기 : 효율적인 구성 요소 데이터 디자인과 시스템 전략의 균형을 맞추는 최상의 방법을 제공합니다.

-=코드의 버스트 적용 범위 증가 : 시스템 및 작업에서 관리 코드의 영향을 줄이기 위해 ISystem 및 IJobEntity 사용이 확장되었습니다.

-=Hybrid Renderer는 Entities Graphics가 됨 : 이 패키지의 의미에 대한 혼란을 줄이기 위해 이름이 변경되었으며 일부 클래스 이름 변경 외에는 리팩토링이 예상되지 않습니다.

-=고성능을 원하면 System에서 SystemI를 사용하고 Burst와 Job을 집어넣어야됨. SystemBase는 Burst가 불가능하다. GC도 존재. SystemI는 반대다.


※요소
DOTS는 ECS(Entity Component System), C# Job Syetem, Burst Compiler로 나뉘어져 있다.
간단히 설명하면 메모리, 멀티스레딩, 컴파일러관련 속도최적화 기술들을 사용하여 고성능으로 제작 할수 있다.
(더많은 오브젝트! 더 빠른 처리스피드! 렉이 없어! 엄청나! 더많은 컨텐츠!)


-=Entity
Entity(Struct) 개체

-=Component
IComponentData(Struct)(데이터, 프로퍼티) 속성
IAspect(readonly partial Struct)(펀션, 기능, 함수, 메서드) 행위

-=System
SystemBase(partial Class) 매니지드 메모리
ISystem(partial Struct) 언매니지드 메모리

-=Job
IJobEntity(partial Struct)

-=Burst
[BurstCompile] (어트리뷰트)



-=기존의 MonoBehaviour와 비교할시
Entity = GameObject(struct로 변경된)
EntityManager = GameObject.Func(기능들의 모집)
SystemAPI = UnityEngine(Time.time 같은 종류들)
float2, float3 = Vector2, Vector3 (값형식이 다름)


※현재 메뉴얼한 코드구조
유니티메뉴얼이나 유튜브에서 찾아보니 대부분 따로 클래스파일을 만들어서 처리하지만 많은 파일이 생기는걸 싫어하기떄문에
하나의 클래스파일에 넣는방법이 좋다고 생각함. (따로 분리시 6개의 파일을 생성해야한다.)

-=클래스 파일 안의 작성순서
1. MonoBehaviour          => 기본적으로 유니티를 다루던 클래스//데이터생성해주기
2. IComponentData         => 데이터만 존재하는 형태
3. Baker                  => MonoBehaviour에 있는 데이터를 IComponentData에 베이킹해주는 클래스
4. IAspect                => IComponentData는 데이터만 존재하는 형태이고 IAspect는 행동만 존재하는 형태. CPU
5. SystemBase or ISystem  => Entity안의 IComponentData를 IAspect를 사용하여 로직을 돌릴때 job 또는 메인스레드에서 돌리는 형태
6. IJobEntity             => Job으로 돌리기 위한 형태


-=각 구성품에 대해 알아둬야할것









![SystemEventOrder](https://user-images.githubusercontent.com/44671731/206775274-e1c8bda1-63fc-47bf-9344-9b37f446eb25.png)

OnCreate: Called when ECS creates a system.
OnStartRunning: Called before the first call to OnUpdate and whenever a system resumes running.
OnUpdate: Called every frame as long as the system has work to do. For more information on what determines when a system has work to do, see ShouldRunSystem.
OnStopRunning: Called before OnDestroy. Also called whenever the system stops running, which happens if no entities match the system's RequireForUpdate, or if you've set the system's Enabledproperty to false. If you've not specified a RequireForUpdate, the system runs continuously unless disabled or destroyed.
OnDestroy: Called when ECS destroys a system.






SystemAPI
데이터 반복 : 쿼리와 일치하는 엔터티당 데이터를 검색합니다.
쿼리 작성 : 작업을 예약하거나 해당 쿼리에 대한 정보를 검색하는 데 사용할 수 있는 캐시된 EntityQuery 를 가져옵니다.=> 캐시가 맞아보암
데이터 액세스 : 구성 요소 데이터, 버퍼 및 EntityStorageInfo 를 가져옵니다 .
싱글톤 액세스 : 싱글 톤 이라고도 하는 데이터의 단일 인스턴스를 찾습니다 . 
SystemAPI메서드는 시스템에 캐시되어 OnCreate호출 .Update전에 호출됩니다. 또한 이러한 메서드를 호출하면 ECS는 조회 액세스 권한을 얻기 전에 호출이 동기화되는지 확인합니다


컴포넌트 데이터반복 
-=SystemAPI.Query
 public void OnUpdate(ref SystemState state)
    {
     float deltaTime = SystemAPI.Time.DeltaTime;

     foreach (var (transform, speed) in SystemAPI.Query<RefRW<LocalToWorldTransform>, RefRO<RotationSpeed>>())
         transform.ValueRW.Value = transform.ValueRO.Value.RotateY(speed.ValueRO.RadiansPerSecond * deltaTime);
    }
-=foreach
foreach (var (transform, speed, entity) in SystemAPI.Query<RefRW<LocalToWorldTransform>, RefRO<RotationSpeed>>().WithEntityAccess())
{
    // Do stuff;
}






IJob: 작업 시스템 스케줄러가 결정하는 모든 스레드 또는 코어에서 실행되는 작업을 만듭니다.
IJobParallelFor: 의 요소를 처리하기 위해 여러 스레드에서 병렬로 실행할 수 있는 작업을 만듭니다 NativeContainer.
JobHandle: 예약된 작업에 액세스하기 위한 핸들입니다. JobHandle인스턴스를 사용하여 작업 간의 종속성을 지정할 수도 있습니다 .





//
  
  
  


-=EntityCommandBuffer
변경 사항을 즉시 수행하는 대신 엔터티 데이터 변경 사항을 대기열에 추가하려면 EntityCommandBuffer스레드로부터 안전한 명령 버퍼를 생성하는 구조체를 사용할 수 있습니다. 이는 작업이 완료되는 동안 구조 변경 을 연기하려는 경우에 유용합니다 .
고로 잡에서 특정작업을 예약하기. 작업할때 딱좋음


※나중에 테스트 및 추가연구 진행해볼것
구조처리
피직스 함수,
엔티티를 가져와서 채크하는 방식 체크 (중간 연결구역이 최소2개이상 띄어져있어서 어딘지 잘모름)
익스큐트애 들어가는 위치 체크
익스큐트랑 foreach랑 어디가 비슷해보이는데 좀더 체크
스트리밍장면이라는 기능이 있는데 설명 대로면 비동기씩 나오는방식같다
그러면...10개를 하면 나중에 다시 그려도 10개인지 테슽 ==========



















참고자료

https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/index.html



