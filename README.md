# TetrisForm
Windows Form과 c#으로 테트리스 모작

![Alt text](/tetris.png)

실행
=====================
tet.exe를 실행하자마자 게임 시작

좌우 방향키로 블럭의 위치를 조정, 위 방향키로 회전, 아래 방향키로 빠르게 내리기

프로그램 구조
--------------------
timer.tick를 사용하여 0.5초마다 블럭들의 위치, 화면 상태 등을 업데이트함(아래 방향키를 누르면 10배 빨라짐)

가로 10칸, 세로 20칸의 그리드를 array를 통해 구현

4개의 블럭을 조합하여 만든 모양들 또한 array를 통해 데이터로 저장됨

게임이 시작되며 7가지 모양중 1개가 무작위로 선택되며, 색상도 무작위로 선택되어 그리드의 가장 위쪽에 생성된다.

그리드 바깥으로 나가지 않도록 측면과 충돌하는 상황을 인식하고 다른 블럭이나 그리드의 가장 아래쪽에 충돌하면 멈추고 새로운 모양이 위에 생성된다.

맨 아래쪽에 줄이 모든 칸이 채워지면, 해당 줄을 비우고 위의 블럭들을 한칸 내린다. 여러 층이 채워진 상태이면 한 번에 전부 처리하도록 인식함

Windows Forms를 이용해 0.5초마다 플레이어의 조작, 블럭의 충돌과 위치 등을 계산한 뒤 GUI로 상황을 보여준다.

가로 10칸, 세로 20칸의 표를 배치하여 그리드 array에서 블럭의 유무, 색상을 확인해 정확한 위치에 표시한다.
