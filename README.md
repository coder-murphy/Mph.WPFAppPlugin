# Mph.WPFAppPlugin

一个轻量、零依赖的 **WPF 微型插件化 Window 基础设施**（类库）。它基于 MVVM 提供了一套开箱即用的宿主外壳：插件只需提供内容控件，框架会自动将其装配到主窗口的侧边栏（SideMenu）、横幅（Banner）与内容区（ContentArea）。

> 定位：在尽量少的约束下，为你的 WPF 应用搭建可扩展的界面形态。宿主与插件之间仅通过少量接口解耦，无第三方依赖。

## 功能特性

- **插件化窗口外壳**：插件按 `PluginTypes` 自动挂载到侧边栏 / 横幅 / 内容区。
- **协议极简**：宿主实现一个 `IPluginWindow`，插件继承 `PluginBase` 即可接入。
- **自带 MVVM 基础设施**：`ViewModelBase`、`RelayCommand`、`Singleton<T>` 开箱可用。
- **统一异常处理**：`PluginApp` 自动挂接 Dispatcher / Task / AppDomain 三层异常入口，可自定义处理或显示默认错误对话框。
- **内置 INI 工具**：通过 `IniUtil` 以 P/Invoke 读写 INI 配置文件。
- **默认皮肤**：附带侧边栏菜单项与横幅的默认 `Theme.xaml` 样式。
- **零第三方依赖**：仅依赖于 .NET 与 WPF 本身，`CSProj` 极简（`net8.0-windows` + `UseWPF`）。

## 项目结构

```
src/
├── Mph.WPFAppPlugin.csproj      # 类库项目（net8.0-windows）
├── PluginApp.cs                 # 宿主应用基类，负责初始化/清理/异常注册
├── PluginAppConfig.cs           # 启动配置（注册插件、启动参数、错误弹窗开关）
├── PluginRuntime.cs             # 插件运行时，装配主窗口 + 生命周期管理
├── PluginBase.cs                # 插件基类（IPlugin 的抽象实现）
├── Theme.xaml                  # 侧边栏 / 横幅默认样式
├── Interfaces/
│   ├── IPlugin.cs              # 插件接口契约
│   ├── IPluginWindow.cs        # 宿主主窗口契约
│   └── IModule.cs              # 模块扩展点标记接口
├── Basement/                   # MVVM 基础
│   ├── ViewModelBase.cs        # INotifyPropertyChanged 基类
│   ├── RelayCommand.cs         # ICommand 通用实现
│   ├── Singleton.cs            # 泛型单例
│   └── HeaderViewModel.cs      # 标题 VM 示例
├── ViewModel/
│   └── SideMenuItemViewModel.cs# 侧边栏菜单项 VM
└── Utils/
    └── IniUtil.cs              # INI 读写工具（kernel32 P/Invoke）
```

## 快速开始

### 1. 实现宿主应用

程序入口继承 `PluginApp`，重写 `OnAppStartUp` 并注册插件：

```csharp
public partial class App : PluginApp
{
    protected override void OnAppStartUp(PluginAppConfig config)
    {
        config.AddPlugin<NotePlugin>();   // 注册一个插件
        config.AddPlugin<SettingPlugin>();
        config.NeedShowDefaultDialog = false; // 关闭默认错误弹窗，自行处理
    }

    protected override void OnExceptionHandle(object exceptionObject)
    {
        // 统一异常处理入口（Dispatcher / Task / AppDomain）
        base.OnExceptionHandle(exceptionObject);
    }
}
```

### 2. 实现主窗口

主窗口需实现 `IPluginWindow`，向框架暴露 `Sidebar / Banner / ContentArea` 三个容器控件：

```csharp
public partial class MainWindow : Window, IPluginWindow
{
    // 侧边栏：ListBox
    public ListBox Sidebar => SidebarListBox;
    // 横幅：ListView（可空）
    public ListView Banner => null;
    // 内容区：Panel
    public Panel ContentArea => ContentHost;

    public void OnPluginWindowInitialize()
    {
        // 窗口装配完成后的一次性初始化
    }
}
```

### 3. 编写插件

实现接口或继承 `PluginBase`。核心是提供 `ContentType` —— 一个**派生自 `FrameworkElement` 且带无参构造函数**的内容控件：

```csharp
public class NotePlugin : PluginBase
{
    public override string Header { get; set; } = "笔记";
    public override Type ContentType => typeof(NoteView); // 某个 UserControl
    public override PluginTypes Type => PluginTypes.SideMenu;

    public override void OnLoading()
    {
        // 插件加载时机，可做数据准备
    }
}
```

启动后，框架会等待主窗口加载完成，并将 `SideMenu` 插件渲染到侧边栏；点击菜单项时，对应 `ContentType` 会被实例化（惰性、缓存复用）并填充到 `ContentArea`。

## 核心接口说明

### `IPlugin`

| 成员 | 说明 |
| ---- | ---- |
| `Guid` | 插件唯一标识 |
| `Header` | 插件显示名称 |
| `ContentType` | 内容控件类型，须为带无参构造函数的 `FrameworkElement` |
| `Icon` | 菜单图标（`ImageSource`） |
| `Type` | `PluginTypes.SideMenu` 或 `PluginTypes.Banner` |
| `OnLoading()` | 加载时回调 |

> 注意：`ContentType` 不合法（非 `FrameworkElement` 或无参构造函数）的插件会被运行时跳过，并向 `OnExceptionHandle` 推送异常。

### `IPluginWindow`

| 成员 | 说明 |
| ---- | ---- |
| `Sidebar` | `ListBox`，侧边栏容器（可为空） |
| `Banner` | `ListView`，横幅容器（可为空） |
| `ContentArea` | `Panel`，内容显示区域 |
| `OnPluginWindowInitialize()` | 装配完成后调用 |

## 扩展与自定义

- **主题样式**：默认样式 key 为 `PluginWindowSideMenuStyle`（`ListBox`）与 `PluginWindowBannerStyle`（`ListView`），可在 `Theme.xaml` 中修改，或在宿主资源字典中覆盖同 key 样式。
- **INI 配置**：`IniUtil.GetSettingValue / PutSetting` 直接读写 INI 文件（Windows 原生实现）。
- **MVVM**：View 模型继承 `ViewModelBase`，命令使用 `RelayCommand`（支持 `CanExecute` 与 `CommandManager.RequerySuggested`）。

## 环境要求

- .NET 8.0 (Windows) SDK
- Windows 桌面（WPF）

## 许可证

[Apache License 2.0](LICENSE)