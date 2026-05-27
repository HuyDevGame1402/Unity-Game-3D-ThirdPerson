# 🧟 Zombie Apocalypse: Last Stand
> **3D Third-Person Shooter (TPS) | Unity | 100% Offline**

Chào mừng bạn đến với **Zombie Apocalypse: Last Stand** — một tựa game bắn súng góc nhìn thứ ba được phát triển trên Unity. Kết hợp giữa sinh tồn, bắn súng, chọn nhân vật, lái xe và thực hiện chuỗi nhiệm vụ kịch tính trong một thế giới mở hoang tàn đầy thây ma.

---

## 📌 Tính Năng Nổi Bật (Features)

### 🎯 Gameplay Cốt Lõi
- **Góc nhìn thứ ba (TPS):** Camera mượt mà bám theo nhân vật, hỗ trợ ngắm bắn, chạy nhanh, thay đạn và né đòn
- **Chế độ ngoại tuyến (100% Offline):** Toàn bộ tiến trình lưu trực tiếp trên thiết bị qua `PlayerPrefs` hoặc JSON

### 🧍 Hệ Thống Nhân Vật
- Giao diện chọn nhân vật trước khi vào trận (Soldier / Medic / Survivor)
- Mỗi nhân vật có chỉ số máu, tốc độ di chuyển và loại súng khởi đầu riêng biệt

### 🚗 Hệ Thống Xe Cộ
- Tương tác lên/xuống xe bằng phím `E`
- Di chuyển nhanh hoặc đâm xuyên qua đám zombie bằng phương tiện

### 🎯 Hệ Thống Nhiệm Vụ (Mission System)
- Thu thập nhu yếu phẩm từ các thùng gỗ (`boxes_pack`)
- Sống sót trong khoảng thời gian quy định (Survival Countdown)
- Tiêu diệt Zombie Boss để mở cửa thoát hiểm

### 🔊 Hệ Thống Âm Thanh
- Hiệu ứng âm thanh 3D sống động (tiếng súng, động cơ xe, tiếng gầm zombie)
- Nhạc nền (BGM) thay đổi theo trạng thái game qua `AudioMixer`

---

## 📁 Cấu Trúc Thư Mục (Assets Structure)

```text
Assets/
├── Audio/               # Nhạc nền (BGM) và hiệu ứng âm thanh (SFX)
├── DelthorGames/        # Asset Store: hệ thống điều khiển súng / UI menu bổ trợ
├── Fonts/               # Phông chữ tùy chỉnh cho game
├── Ilumisoft/           # Công cụ AI / hệ thống tương tác (mở hòm, nhặt đồ)
├── JMO Assets/          # Particle Effects: Muzzle Flash, máu bắn, khói xe
├── Player/              # Model 3D, Textures, Materials, Animations nhân vật
├── Prefabs/             # Zombie, Xe cộ, Súng, Điểm hồi sinh (kéo thả vào Scene)
├── Scenes/              # MainMenu, CharacterSelection, CityMap, GameOver
├── Scripts/             # Toàn bộ mã nguồn C# điều khiển logic game
├── Sprites/             # UI 2D: Icon vũ khí, thanh HP, Crosshair, Minimap
├── TextMesh Pro/        # Cấu hình TextMesh Pro — hiển thị chữ UI sắc nét
├── TerrainAutoUpgrade/  # Công cụ tối ưu địa hình tương thích Unity mới
└── boxes_pack/          # Model 3D thùng gỗ / hòm tiếp tế cho nhiệm vụ
```

---

## 🛠️ Kiến Trúc Mã Nguồn (Scripts Architecture)

### 🗂️ Managers — Hệ Thống Cốt Lõi
| Script | Chức năng |
|---|---|
| `GameManager.cs` | Quản lý luồng game (Start / Pause / Win / Lose), điểm số và nhiệm vụ |
| `AudioManager.cs` | Điều phối BGM và SFX qua AudioMixer |
| `CharacterSelector.cs` | Ghi nhận lựa chọn nhân vật từ UI và Instantiate vào bản đồ chính |

### 🧍 Player & Combat — Điều Khiển & Chiến Đấu
| Script | Chức năng |
|---|---|
| `PlayerController.cs` | Di chuyển vật lý, xoay camera theo chuột, nhảy |
| `PlayerCombat.cs` | Bắn súng, trừ đạn, nạp đạn, tính sát thương (Raycast) lên zombie |

### 🚗 Vehicle System — Hệ Thống Xe Cộ
| Script | Chức năng |
|---|---|
| `CarController.cs` | Vật lý xe (WheelColliders), tăng tốc, phanh, drift |
| `VehicleInteraction.cs` | Xử lý vào/ra xe, chuyển đổi camera giữa nhân vật và xe |

### 🧟 AI & Enemies
| Script | Chức năng |
|---|---|
| `ZombieAI.cs` | `NavMeshAgent` tuần tra, phát hiện tiếng động / tầm nhìn, đuổi theo tấn công |

### 🎯 Mission & Interactors
| Script | Chức năng |
|---|---|
| `MissionZone.cs` | Kích hoạt mục tiêu khi người chơi bước vào vùng nhiệm vụ |
| `CollectibleItem.cs` | Xử lý logic nhặt hộp `boxes_pack`, cập nhật tiến độ nhiệm vụ |

---

## 🕹️ Hướng Dẫn Điều Khiển (Controls)

### 🚶 Khi Đi Bộ (On Foot)
| Phím | Hành động |
|---|---|
| `W` `A` `S` `D` | Di chuyển nhân vật |
| `Mouse` | Xoay camera |
| `Chuột Trái` | Bắn súng |
| `Chuột Phải` | Ngắm bắn (Zoom) |
| `R` | Thay đạn (Reload) |
| `Left Shift` | Chạy nhanh (Sprint) |
| `E` | Tương tác (Mở hòm / Lên xe) |

### 🚗 Khi Lái Xe (In Vehicle)
| Phím | Hành động |
|---|---|
| `W` / `S` | Tiến / Lùi |
| `A` / `D` | Rẽ trái / Rẽ phải |
| `Space` | Phanh tay (Handbrake / Drift) |
| `E` | Xuống xe |

---

## 🚀 Hướng Dẫn Cài Đặt (Installation & Setup)

### 🖥️ Yêu Cầu Hệ Thống (Prerequisites)
- **Unity Editor:** `2021.3 LTS` trở lên
- **Render Pipeline:** Built-in hoặc Universal Render Pipeline (URP)

### 📋 Các Bước Thực Hiện

**1. Clone mã nguồn từ GitHub**
```bash
git clone https://github.com/HuyDevGame1402/Unity-Game-3D-ThirdPerson.git
```

**2. Mở dự án bằng Unity Hub**
- Mở **Unity Hub** → **Add → Add project from disk**
- Trỏ đến thư mục vừa clone, chọn phiên bản `2021.3 LTS` và mở dự án

**3. Cấu hình môi trường**
- Nếu có thông báo TextMesh Pro → chọn **Import TMP Essentials**
- Vào **Window → AI → Navigation** → tab **Bake** → nhấn **Bake** để quét lại đường đi cho Zombie

**4. Cấu hình Build Settings**
- Vào **File → Build Settings**, thêm các Scene theo đúng thứ tự:

```
Assets/Scenes/MainMenu.unity
Assets/Scenes/CharacterSelection.unity
Assets/Scenes/CityMap.unity
```

---

## 📝 Bản Quyền & Ghi Chú (License & Notes)

- **Trạng thái:** Hoàn thành các tính năng cốt lõi (Core Gameplay Loop)
- **Asset bên thứ ba** (Ilumisoft, JMO Assets, TextMesh Pro) thuộc bản quyền tác giả trên Unity Asset Store — vui lòng không thương mại hóa khi chưa được cấp phép
- **Mã nguồn Scripts** được phát triển nội bộ bởi đội ngũ dự án

> 🧟 Chúc bạn có những trải nghiệm sinh tồn tuyệt vời cùng **Zombie Apocalypse: Last Stand**!

---

## 👤 Tác Giả (Author)

| | |
|---|---|
| **Họ và Tên** | Nguyễn Đức Huy |
| **Email** | [huyco14022004@gmail.com](mailto:huyco14022004@gmail.com) |
| **LinkedIn** | [nguyễn-đức-huy](https://www.linkedin.com/in/nguy%E1%BB%85n-%C4%91%E1%BB%A9c-huy-081a73411/) |
