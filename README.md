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

※요소
DOTS는 ECS(Entity Component System), C# Job Syetem, Burst Compiler로 나뉘어져 있다.
간단히 설명하면 메모리, 멀티스레딩, 컴파일러관련 속도최적화 기술들을 사용하여 고성능으로 제작 할수 있다.
(더많은 오브젝트! 더 빠른 처리스피드! 렉이 없어! 엄청나! 더많은 컨텐츠!)


-=E
Entity(Struct) 개체

-=C
IComponentData(Struct)(데이터, 프로퍼티) 속성
IAspect(readonly partial Struct)(펀션, 기능, 함수, 메서드) 행위

-=S
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

-=클래스파일안의 작성순서
1. MonoBehaviour
2. IComponentData
3. Baker 
4. IAspect
5. SystemBase or ISystem
6. IJobEntity





















나중에 테스트 진행해볼것



스트리밍장면이라는 기능이 있는데 설명 대로면 비동기씩 나오는방식같다
그러면...10개를 하면 나중에 다시 그려도 10개인지 테슽 ==========



















참고자료

https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/index.html



