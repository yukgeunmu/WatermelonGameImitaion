# 🍉 Suika Game Clone

Unity 기반으로 제작한 모바일 캐주얼 머지 퍼즐 게임입니다.
같은 과일을 합쳐 더 큰 과일로 진화시키고, 높은 점수를 획득하는 것을 목표로 합니다.

---

# 🎮 게임 소개

좁은 박스 안에 과일을 떨어뜨리고,
같은 종류의 과일끼리 충돌하면 더 큰 과일로 합쳐집니다.

과일이 계속 쌓이며 물리 기반으로 움직이고,
상단 게임 오버 라인을 넘기지 않도록 전략적으로 배치해야 합니다.

---

# ✨ 주요 기능

## 🍎 머지 시스템

* 동일한 과일 충돌 시 다음 단계 과일로 합성
* 연쇄 합성(콤보) 지원
* 점수 획득 시스템

## ⚡ 콤보 시스템

* 연속 합성 시 콤보 증가
* 콤보 UI 애니메이션
* 랜덤 컬러 및 펀치 효과 적용

## 🎵 사운드 시스템

* BGM / SFX 분리 관리
* AudioMixer 기반 볼륨 조절
* PlayerPrefs 저장 지원
* 버튼 클릭 사운드 이벤트 처리

## 🧩 오브젝트 풀링 시스템

* Generic Pool 기반 재사용 구조
* 과일 / 이펙트 / 팝업 UI 풀링
* 런타임 Instantiate 최소화

## 🖥 UI 시스템

* SceneUI / PopupUI 구조 분리
* 이벤트 기반 UI 갱신
* 옵션 팝업 구현
* 로딩 씬 지원

## 📱 모바일 대응

* New Input System 기반 터치 입력
* 드래그 후 손을 떼면 과일 드랍
* 해상도 및 화면 비율 대응

## 🚀 Addressables 적용

* 리소스 비동기 로드
* Sprite Atlas 관리
* 런타임 메모리 관리

---

# 🛠 사용 기술

| 기술               | 설명        |
| ---------------- | --------- |
| Unity            | 게임 엔진     |
| C#               | 게임 로직     |
| DOTween          | UI 애니메이션  |
| Addressables     | 리소스 관리    |
| AudioMixer       | 사운드 믹싱    |
| New Input System | 모바일 입력 처리 |
| Object Pooling   | 성능 최적화    |

---

# 🧱 프로젝트 구조

```text
Scripts
├── Core
├── Data
├── Effects
├── Events
├── Fruit
├── Managers
├── Pool
├── SceneController
├── UI
```
---

# 🧠 주요 설계 포인트

## 이벤트 기반 구조

UI와 게임 로직 간 결합도를 낮추기 위해
`GameEventBus` 기반 이벤트 시스템을 적용했습니다.

예:

* 과일 변경 이벤트
* 과일 합성 이벤트
* 버튼 클릭 이벤트

---

## Generic Pool 시스템

Instantiate / Destroy 비용을 줄이기 위해
범용 Generic Pool 시스템을 구현했습니다.

```csharp
Game.Get<PoolManager>().Get<Fruit>("Apple");
```

---

## SceneUI / PopupUI 분리

씬 고정 UI와 팝업 UI를 분리하여
UI 관리 구조를 단순화했습니다.

---

# 📸 게임 화면

## 시작 화면

(스크린샷 추가 예정)

## 게임 플레이

(스크린샷 추가 예정)

## 옵션 UI

(스크린샷 추가 예정)

---

# 📦 빌드 환경

| 항목            | 내용               |
| ------------- | ---------------- |
| Unity Version | 6000.x           |
| Platform      | Android          |
| Input System  | New Input System |
| Rendering     | URP              |

---

# 🚀 실행 방법

```bash
git clone [Repository URL]
```

Unity Hub에서 프로젝트를 열고 실행합니다.

---

# 📌 향후 개선 예정

* 랭킹 시스템
* 저장 기능
* 다양한 테마 추가
* 진동 피드백
* 연출 강화
* 최적화 개선

---

# 👨‍💻 개발자

Unity 클라이언트 개발 공부 및 모바일 캐주얼 게임 제작을 목적으로 개발한 프로젝트입니다.
