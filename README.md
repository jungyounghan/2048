# 2048 퍼즐 병합 게임
 ## 제작기간 2024/12/03 ~ 2024/12/04 (총 1일)

## 게임 개요
### 상하좌우로 화면의 숫자들을 이동시켜 같은 숫자들끼리 합쳐 공간이 없을 때 까지 점수를 얻어야 합니다. 

## 기능 목록
게임 시작, 맵 크기 설정, 종료
사용자 입력 처리(방향키, 숫자 패드(1, 2, 3), Esc)
이벤트 발생(2048 이상 점수 도달 혹은 더 이상 게임 진행이 불가하면 메인 화면으로 회귀)

## 게임 흐름
1. 메인 화면 출력(시작, 종료)
2. 4 X 4 크기의 2차원 int형 배열 안에서 랜덤한 위치 2곳에 최소값 2를 대입함
3. 방향키 상하좌우 중 한 곳으로 배열 안의 숫자들을 전부 이동시킴
4. 이동 중에 같은 숫자가 서로 맞물려 있을 경우 그 숫자들이 합쳐짐
5. 배열 안의 비어있는 위치 중 한 곳에 최소값 2를 대입하는데 그런 유효 공간이 없다면 패배
6. 합쳐진 숫자가 2048 이상이면 승리

## 사용한 외부 라이브러리와 상수 정의
### 프로젝트에서 사용했던 외부 라이브러리와 그 용도
[DllImport("kernel32.dll", ExactSpelling = true)] private static extern IntPtr GetConsoleWindow();
현재 콘솔 창의 핸들을 가져오는 Win32 API 함수

[DllImport("user32.dll")] public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
특정 메뉴 핸들에서 원하는 항목을 삭제해주는 Win32 API 함수

[DllImport("user32.dll")] private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
특정 윈도우의 시스템 메뉴 핸들을 반환하는 Win32 API 함수

###상수 정의
const int MF_BYCOMMAND = 0x00000000; 	//메뉴 삭제 시 명령 ID로 항목을 지정
const int SC_MINIMIZE = 0xF020;		     //최소화 명령
const int SC_MAXIMIZE = 0xF030;		     //최대화 명령
const int SC_SIZE = 0xF000;			        //크기 조절 명령

DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

콘솔 창의 시스템 메뉴에서 최소화, 최대화, 창 크기 조절을 삭제하여 콘솔의 크기 조절을 비활성화

int width = Console.WindowWidth;
int height = Console.WindowHeight;
Console.BufferHeight = height;

현재 콘솔 창의 너비와 높이를 가져와 BufferHeight를 고정함
스크롤 막대를 없애기 위해 버퍼 높이를 창 높이와 같게 설정함
