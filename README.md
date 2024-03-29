# SpartaDungeon_TeamProject
스파르타 던전 Text 게임 (팀 과제) - 17조

## 프로젝트 소개
콘솔 Text RPG 게임

### 멤버구성
- 팀장 국지윤 : 전투 시작, 몬스터 종류 추가, 스테이지 추가, 방어력 기능
- 팀원 금경희 : 직업 선택 기능, 레벨업 기능, 경험치 보상
- 팀원 이도현 : 회피기능, 포션 및 골드 드랍, 콘솔 꾸미기, 회복아이템, 전투 중 포션 사용
- 팀원 강성원 : 캐릭터 생성 기능, 스킬 기능, 마나 관련 기능

# 기능 설명

## 필수 요구

1. 게임 시작 화면
2. 상태 보기
3. 전투 시작
   - 전투 과정
     - 행동 선택 - 대상 선택 - 플레이어 공격 - 몬스터 공격 (반복)
   - 1회 루프 후
     - 플레이어 Hp <= 0 시, Lose
     - 현재 스폰된 몬스터 전부 Dead == true 시, Victory
       
   - EnterDungeon 메서드를 호출 => 행동 선택 메뉴 진입
     - 몬스터를 랜덤한 수, 랜덤한 타입, 랜덤한 레벨 로 생성
     - 생성한 몬스터를 Monster[] 멤버에 저장
     - 플레이어가 취할 행동을 선택
       
   - 플레이어 행동 선택 후, Fight 메서드 호출 => 대상 선택 메뉴 진입
     - 현재 생성된 몬스터들의 번호를 입력 시, 해당 몬스터 공격
     - 0번 : 취소 입력 시, 행동 선택 메뉴로 돌아감
       
   - 올바른 대상 입력 시, PlayerPhase 메서드 호출 => 플레이어 공격 페이즈 진입
     - 플레이어의 공격력의 10% 만큼 오차를 가짐 (공격력 : 10 -> 9 ~ 11의 데미지)
     - 공격력 오차가 소수점이라면 올림 (오차 : 0.1 ~ 0.9 -> 1)
     - GetDamage 메서드 호출, 몬스터에게 데미지를 가함
     - 몬스터의 Hp <= 0 시, Dead 상태 : 해당 몬스터 공격 불가 및 공격 받지 않음

   - 플레이어 공격 후 => 몬스터 공격 페이즈 진입
     - 현재 스폰된 모든 몬스터 차례로 반복 검사 시작
     - 몬스터가 Dead 상태가 아니고 (&&) 플레이어 Hp > 0 일 때 플레이어를 공격
     - 1회 루프 완료

- 추가된 사항
  - Monster Class : 몬스터 정보를 총괄
  - IDamagable : 데미지를 받을 수 있는 객체는 모두 상속 받을 interface

## 선택 요구

1. 캐릭터 생성
   - 원하는 이름 입력
   - (2.)에서 이어짐
2. 직업 선택
   - console 로 직업별 스팩 안내
   - switch case 로 직업 선택 기능 구현 
   - 선택한 값에 따라 Player클래스를 상속 받은 직업 클래스의 객체를 Dungeon.cs에 생성


4. 스킬
   - 캐릭터 생성 시 직업마다 다른 스킬 생성
   - 전투 중 '스킬' 선택 시 스킬 목록 출력
   - 스킬은 '단일 타격'과 '다중 타격'으로 나뉨
   - 다중 타격 스킬은 한 번에 타격할 수 있는 몬스터 수가 정해져있음
   - 단일 타격 스킬 선택 => 대상 선택 메뉴 진입
     - 생존한 몬스터의 번호를 입력 시 (기본공격력 x 스킬 공격력 배수)를 GetDamage의 인자로 전달, 몬스터에게 데미지 가함
   - 다중 타격 스킬 선택
     - 여러 몬스터 중 스킬의 다중 타겟 수 만큼 랜덤으로 (기본공격력 x 스킬 공격력 배수)를 GetDamage의 인자로 전달, 몬스터들에게 데미지 가함

5. 치명타
    - 몬스터 공격 시, 15% 의 확률로 치명타 발생, 160% 데미지
    - ```CalculateExDamage``` 메서드에서 치명타와 회피를 동시에 계산하여 반환
      - 입력: int 원래의 데미지, bool 스킬사용여부
      - 출력: bool 치명타여부, bool 회피여부, int 실데미지
      - 배틀 장면 중 편리한 한 줄 사용이 가능하도록 설계
      - 튜플 형식을 사용하여 여러 값을 한번에 반환

6. 회피
    - 스킬공격이 아닐 시, 10% 의 확률로 회피, 0의 데미지
    - ```CalculateExDamage``` 메서드에서 치명타와 회피를 동시에 계산하여 반환
    
7. 레벨업
    - monsters의 레벨 값 및 player 객체의 Point 값 받아오는 LevelCal() 구현
    - player Point 조건별 분기처리
    - 이전 레벨, 승급 레벨 console 표시

9. 보상 추가
    - 포션 및 골드 드랍
      - 포션 : ( 0개 ~ 몬스터 수 ) 중 랜덤 갯수를 드랍
      - 골드 : ( 500 * ( 0.5 ~ 몬스터 수 ) G ) 중 랜덤 수치 드랍
    - 경험치 보상
      - Player 클래스에 Point 전역변수 추가


11. 콘솔 꾸미기
    - 유저 인터페이스와 관련된 기능을 넣은 ```UI.cs```를 마련
      - 유저 입력 전용 패널, 아스키아트 프리셋, 텍스트편의성, 파티클기능, 화면저장, 전각반각구분 등 유저가 직접 보는 콘솔 화면과 관계된 기능
    - 아스키아트
      - 모든 장면에 기본적으로 아스키아트를 마련
      - 화면의 크기에 따른 반응형 아트 배치
        - 상태창을 토글하여 펼치는 등 화면이 넓어지는 조건부에 따라 추가 아트를 배치
      - 콘솔 내 지정한 위치에 입력받은 내용을 작성하는 메서드를 마련
        - ```DrawOnSpecificPos(string s, (int x, int y) startPosition)```
        - 현재 커서의 위치를 저장 해 두고, 지정한 위치로 커서를 이동 후 여러 줄에 걸쳐 내용을 작성한 뒤, 원래 커서의 위치로 돌아간다
        - 같은 라인을 모두 차지하고 있지 않은 모든 부가적인 아트들은 해당 메서드를 사용하여 그림
      - 파티클 효과 메서드 구현
        - ```UpdateEffect( (int x, int y) startPosition, (int x, int y) endPosition, char c, int particleQuantity, int delayMs, bool useSavedBuffer=false)```
        - 기존 화면 내용을 저장하고, 스레드를 사용하여 일정주기마다 파티클을 그린 후 기존 화면 내용을 복원하기를 반복하는 방식
        - 메인스레드에서는 유저의 입력을 받아, 입력값이 들어오면 파티클을 갱신하는 스레드를 정지한다.
        - 버퍼에 기존 컨텐츠 내용을 저장 후, 해당 버퍼는 파티클이 지나간 자리의 복원에 사용한다.
    - 유저 입력 전용 패널 구성
      - 유저에게 입력을 받거나, 알림을 보여주는 패널 구성
        - 매개변수로 기본 메시지를 변경하여 호출이 가능
        - 경고메시지와 경고메시지의 속성(긍정, 부정)을 전달하여 다른 색으로 메시지를 보여줄 수 있다
        ![유저패널](https://cdn.discordapp.com/attachments/497349582494367744/1196375533685055528/img.png)
    - 개발자 편의성을 위한 Write 관련 메서드를 구현
      - ColoredWrite, ColoredWriteLine(string s, ConsoleColor color)
        - 입력받은 문자열에 지정한 색을 입혀 출력한다.
        - 아래와 같이, 특정 단어에만 색을 입히는 등의 작업을 한 줄로 구현이 가능
          ```
          Console.WriteLine($"골드를 {UI.ColoredWriteLine("500", ConsoleColor.Yellow);} 만큼 획득!");
          ```
      - RandomColoredWrite(string s)
        - 입력받은 문자열의 모든 문자들에 무작위 색을 입혀 출력하는 신나는 메서드

12. 몬스터 종류 추가해보기
    - 열거형을 관리하는 Define 클래스의 MonsterType 으로 몬스터 종류를 알 수 있음
    - Monster 클래스에서 몬스터 타입 마다의 능력치를 readonly int[] HPs / float[] ATKs 로 관리
    - Define.MonsterType.SkeletonWorrior = 0 ~ Define.MonsterType.GoblinWizard = 5 까지 구현

13. 아이템 적용

14. 회복 아이템
    - 포션 타입의 아이템 ```[회복약]```추가
      - 다른 아이템 타입들과 마찬가지로 item 클래스를 상속받아 구현
      - 소비아이템이기 때문에, 다른 타입들과 다르게 heal(회복량), count(보유 수)필드를 마련
    - 회복 전용 장면 구현
      - ```[메인메뉴 - 회복 아이템]```에 진입하여 회복약을 사용 가능
      - 회복약이 부족하거나 체력이 최대일 경우, 사용 불가와 함께 알림을 출력
      - 회복약이 1개 이상이고 체력이 최대가 아닐 경우, 회복약을 하나 사용하여 {heal=30}의 체력 회복
    - 전투 중 포션 사용 기능 구현
      - 몬스터와 전투 도중, 플레이어 턴일 경우, ```[회복]``` 선택지를 통해 회복약을 사용 가능


15. 스테이지 추가
    - 스테이지 로직
      - 던전입구 -> 최고 도달층 입장 / 다른 층 선택 (분기)
      - 선택한 층에 입장
        - 선택한 층에 따라 난이도 상이
      - 선택한 층 클리어 시, (선택층 == 최고층) 이라면 최고층 갱신
    - 난이도 구현
      - 5층 단위로 스폰되는 개체 수 1 ~ 5마리 순환
        - 예) 3층 : 최대 3마리 스폰, 6층 : 최대 1마리 스폰
      - 최대 스폰 개체 수 3마리 이상일 때, 최소 스폰 개체 수 2마리
        - 예) 4층 : 2 ~ 4마리 랜덤 스폰, 7층 : 1 ~ 2마리 랜덤 스폰
      - 5층 단위로 레벨, 생성 타입 증가.
        - 최소레벨 : 1 x [i]층
          - 예) 1층 : 최소/최대 = 1 / 2
        - 최대레벨 : 3 x [i]층 - [i]층
          - 예) 4층 : 최소/최대 = 4 / 8
        - 최소 타입 : 스켈레톤 워리어
        - 최고 타입 : 5층 마다 3가지 타입 추가 / 초과 시 현재 존재하는 마지막 타입
       
- 추가된 사항
  - DungeonInfo Class : 스테이지 및 난이도를 총괄하는 Static 클래스
    - 최고 도달 층(기록), 몬스터 소환, 최고층 갱신(최고 기록) 등은 게임 내에서 객체를 생성해 관리할 필요가 없고, 유일하므로 static class로 만듦.
14. 게임 저장하기
   - Generic을 사용하여 T 타입의 데이터를 json 파일로 저장 및 불러오기
15. 기타 커스텀 기능

- 방어력 추가 기능
  - 몬스터의 공격 단계에서 [플레이어의 방어력 * 0.5f] 만큼 경감된 데미지를 int 변수에 할당하여 Player의 GetDamage 메서드 매개변수로 전달. 공격력 - 데미지경감 <= 0 일 때, 데미지 1을 받음
- 타이틀 장면
  - 아트와 게임 타이틀을 표시, 파티클 효과 구현
    - (a) 장면의 기본 출력 정보를 저장
    - (b) 랜덤한 위치 n곳을 저장하여 해당 위치에 파티클 생성
    - (c) 일정시간 후, 해당 위치의 저장된 정보를 기반으로 원래 문자로 복원
    - a~c를 반복. 전각문자의 존재로 인한 출력 오류 등 함께 해결
  - 키 입력 시 다음 장면으로 이동
    - 저장된 플레이어 데이터가 없다면 → 새로운 플레이어 프로필 생성 장면
    - 저장된 플레이어 데이터가 있다면 → 메인메뉴(마을) 장면
