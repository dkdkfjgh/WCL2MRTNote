## **WCL2MRTNote**

상위권 유저들의 쿨기를 Method Raid Tools Note 로 출력하는 프로그램입니다

 - 개발 환경 : Visual Studio 2022 / .Net Framework 4.8.1 개발 언어 : C#
 - 실행 오류시 https://dotnet.microsoft.com/ko-kr/download/dotnet-framework/net481 이동 후 런타임 설치
-  개발: 달빛프리즘오너-아즈샤라

## **사용 방법**
1. https://www.curseforge.com/wow/addons/method-raid-tools 이동 후 MRT 애드온 설치
2. https://github.com/dkdkfjgh/WCL2MRTNote/releases/tag/Release 이동 후 본 프로그램 설치
3. 프로그램 실행 후 직업/전문화, 보스, 정렬방식(DPS,HPS), 등수 입력 후 로그 추출 버튼 선택
4. https://lorrgs.io/ 기준 해당 등수 플레이어의 킬타임, 주요 쿨기와 생존기 시간을 MRT 메모로 추출
5. 해당 메모를 개인 MRT 메모로 가져와서 사용

## **주의 사항**
1. 본 프로그램 내 정보의 출처는 https://lorrgs.io/ 와 https://raid.subcreation.net/ 사이트입니다
2. 마력 주입 / 페이 수호자 등의 외부 버프에 주의하십시오!
3. 해당 메모를 맹목적으로 믿고 사용하지 마시고, 참고만 하세요!
4. 한 쿨기의 사이클은 13초 간격으로 계산됩니다! (const int OneCycleMS = 13000)
5. 보스 스킬과 묶는것을 선택할 경우, 주요 스킬 사용시 4초 내외의 보스 시전 주요 스킬과 묶어서 보여줍니다. 힐러 공생기 계산시 좋습니다.(const int BossCycleMS = 4000)
6. (주의) Lorrgs 사이트의 한계로 보스 스킬 정보는 1등 로그의 기준으로 제공됩니다! 1등 로그가 아닌 다른 로그를 선택할 시 보스 스킬 시전시간과 오차가 있을 수 있습니다!
7. 버그 제보 및 건의사항은 달빛프리즘오너-아즈샤라 서버로 인 게임 내 우편 부탁드립니다!
