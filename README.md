# DOTSTest(확인용도)

목적 : 기존의 MonoBehaviour를 사용하여 작업했던걸 DOTS로 작업할수 있을만큼 알아가기


메뉴얼 : https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/DOTS_Guide


-=Entity 1.0 버전부터 필수사항

1. 2022.2.0b8

2. Visaul Studio 2022 (지원하는 일정이상의 IDE)




문제점
1. 완성품이 안되있는 느낌이다. 일반적인 API를 사용하는것처럼 하면 좋지만 따로 연구하고 알아봐야되는 사항이 많다.
2. 메모리가 많이 사용된다. (VS 2.5G, UnityEditor 2.5G.. 더많은 메모리! I need more Memory!!)
3. 알긴아는데 확실하지가 않음. 테스트를 진행해서 알아가야될 구간이 많음
4. 몇몇 구간은 갑자기 띄어져있음  이게 그건가? 연결되는건지 아닌건지 체크해야되는 이상한상태가..
5. DOTS(매니저) => OOP(너가 알아서 해) => DOTS


※설치법

-=프로젝트 생성시
3D URP는 에러안생김 
2D URP에 지원을 안해서 에러가 생기는듯함 => 근데 난 이거해야되는데 2D..


1. Window -> PackageManager 
2. 우측

package Manager
Add Package by name
1. com.unity.entities
2. com.unity.entities.graphics  => Hybrid Renderer였던것
3. com.unity.2d.entites => 2D 엔티티
4. com.unity.2d.entities.physics =>2D 엔티티는 아직 릴리즈가 아니니 별개로 작업

프로젝트 설정 Enter play mode settings
Enter play mode Options => On
reload Domain => false
reload scene => false
disalbe scene backup => on 









스트리밍



















참고자료

https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/index.html



