﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using STranslate.Helper;
using STranslate.Log;
using STranslate.Model;
using STranslate.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace STranslate.ViewModels.Preference
{
    public partial class CommonViewModel : ObservableObject
    {
        public event Action<bool>? OnIncrementalChanged;

        [RelayCommand]
        private void Save()
        {
            if (configHelper.WriteConfig(this))
            {
                //通知增量翻译配置到主界面
                OnIncrementalChanged?.Invoke(IncrementalTranslation);
                ToastHelper.Show("保存常规配置成功", WindowType.Preference);
            }
            else
            {
                LogService.Logger.Debug($"保存常规配置失败，{JsonConvert.SerializeObject(this)}");
                ToastHelper.Show("保存常规配置失败", WindowType.Preference);
            }
        }

        [RelayCommand]
        private void Reset()
        {
            IsStartup = curConfig?.IsStartup ?? false;
            NeedAdmin = curConfig?.NeedAdministrator ?? false;
            HistorySize = curConfig?.HistorySize ?? 100;
            AutoScale = curConfig?.AutoScale ?? 0.8;
            ThemeType = curConfig?.ThemeType ?? ThemeType.Light;
            IsFollowMouse = curConfig?.IsFollowMouse ?? false;
            CloseUIOcrRetTranslate = curConfig?.CloseUIOcrRetTranslate ?? false;
            UnconventionalScreen = curConfig?.UnconventionalScreen ?? false;
            IsOcrAutoCopyText = curConfig?.IsOcrAutoCopyText ?? false;
            IsAdjustContentTranslate = curConfig?.IsAdjustContentTranslate ?? false;
            IsRemoveLineBreakGettingWords = curConfig?.IsRemoveLineBreakGettingWords ?? false;
            IsRemoveLineBreakGettingWordsOCR = curConfig?.IsRemoveLineBreakGettingWordsOCR ?? false;
            DoubleTapTrayFunc = curConfig?.DoubleTapTrayFunc ?? DoubleTapFuncEnum.InputFunc;
            CustomFont = curConfig?.CustomFont ?? ConstStr.DEFAULTFONTNAME;
            IsKeepTopmostAfterMousehook = curConfig?.IsKeepTopmostAfterMousehook ?? false;
            IsShowPreference = curConfig?.IsShowPreference ?? false;
            IsShowMousehook = curConfig?.IsShowMousehook ?? false;
            IsShowIncrementalTranslation = curConfig?.IsShowIncrementalTranslation ?? false;
            IsShowScreenshot = curConfig?.IsShowScreenshot ?? false;
            IsShowOCR = curConfig?.IsShowOCR ?? false;
            IsShowSilentOCR = curConfig?.IsShowSilentOCR ?? false;
            IsShowClipboardMonitor = curConfig?.IsShowClipboardMonitor ?? false;
            IsShowQRCode = curConfig?.IsShowQRCode ?? false;
            IsShowHistory = curConfig?.IsShowHistory ?? false;
            IsShowConfigureService = curConfig?.IsShowConfigureService ?? false;
            WordPickingInterval = curConfig?.WordPickingInterval ?? 200;
            IsHideOnStart = curConfig?.IsHideOnStart ?? false;
            ShowCopyOnHeader = curConfig?.ShowCopyOnHeader ?? false;
            IsCaretLast = curConfig?.IsCaretLast ?? false;
            ProxyMethod = curConfig?.ProxyMethod ?? ProxyMethodEnum.系统代理;
            ProxyIp = curConfig?.ProxyIp ?? string.Empty;
            ProxyPort = curConfig?.ProxyPort ?? 0;
            ProxyUsername = curConfig?.ProxyUsername ?? string.Empty;
            ProxyPassword = curConfig?.ProxyPassword ?? string.Empty;
            CopyResultAfterTranslateIndex = curConfig?.CopyResultAfterTranslateIndex ?? 0;
            IncrementalTranslation = curConfig?.IncrementalTranslation ?? false;
            IsTriggerShowHide = curConfig?.IsTriggerShowHide ?? false;
            IsShowMainPlaceholder = curConfig?.IsShowMainPlaceholder ?? true;
            ShowAuxiliaryLine = curConfig?.ShowAuxiliaryLine ?? true;
            ChangedLang2Execute = curConfig?.ChangedLang2Execute ?? false;
            OcrChangedLang2Execute = curConfig?.OcrChangedLang2Execute ?? false;
            UseFormsCopy = curConfig?.UseFormsCopy ?? false;
            ExternalCallPort = curConfig?.ExternalCallPort ?? 50020;
            DetectType = curConfig?.DetectType ?? LangDetectType.Local;
            DisableGlobalHotkeys = curConfig?.DisableGlobalHotkeys ?? false;
            MainViewMaxHeight = curConfig?.MainViewMaxHeight ?? 840;
            MainViewWidth = curConfig?.MainViewWidth ?? 460;

            LoadHistorySizeType();
            ToastHelper.Show("重置配置", WindowType.Preference);
        }

        public CommonViewModel()
        {
            // 获取系统已安装字体
            GetFontFamilys = Fonts.SystemFontFamilies.Select(font => font.Source).ToList();
            // 判断是否已安装软件字体，没有则插入到列表中
            if (!GetFontFamilys.Contains(ConstStr.DEFAULTFONTNAME))
            {
                GetFontFamilys.Insert(0, ConstStr.DEFAULTFONTNAME);
            }

            // 加载历史记录类型
            LoadHistorySizeType();
        }

        private void LoadHistorySizeType()
        {
            HistorySizeType = HistorySize switch
            {
                50 => 0,
                100 => 1,
                200 => 2,
                500 => 3,
                1000 => 4,
                long.MaxValue => 5,
                0 => 6,
                _ => 1
            };
        }

        /// <summary>
        /// ConfigHelper单例
        /// </summary>
        private static ConfigHelper configHelper = Singleton<ConfigHelper>.Instance;

        /// <summary>
        /// 当前配置实例
        /// </summary>
        private static ConfigModel? curConfig = configHelper.CurrentConfig;

        /// <summary>
        /// 是否开机启动
        /// </summary>
        [ObservableProperty]
        private bool isStartup = curConfig?.IsStartup ?? false;

        /// <summary>
        /// 是否默认管理员启动
        /// </summary>
        [ObservableProperty]
        private bool needAdmin = curConfig?.NeedAdministrator ?? false;

        /// <summary>
        /// 历史记录大小
        /// </summary>
        private long historySizeType = 1;

        public long HistorySizeType
        {
            get => historySizeType;
            set
            {
                if (historySizeType == value) return;
                OnPropertyChanging();
                historySizeType = value;

                HistorySize = value switch
                {
                    0 => 50,
                    1 => 100,
                    2 => 200,
                    3 => 500,
                    4 => 1000,
                    5 => long.MaxValue,
                    6 => 0,
                    _ => 100
                };

                OnPropertyChanged();
            }
        }

        public long HistorySize = curConfig?.HistorySize ?? 100;

        [ObservableProperty]
        private double autoScale = curConfig?.AutoScale ?? 0.8;

        /// <summary>
        /// 主题类型
        /// </summary>
        [ObservableProperty]
        private ThemeType themeType = curConfig?.ThemeType ?? ThemeType.Light;

        [ObservableProperty]
        private bool isFollowMouse = curConfig?.IsFollowMouse ?? false;

        [ObservableProperty]
        private bool closeUIOcrRetTranslate = curConfig?.CloseUIOcrRetTranslate ?? false;

        [ObservableProperty]
        private bool unconventionalScreen = curConfig?.UnconventionalScreen ?? false;

        [ObservableProperty]
        private bool isOcrAutoCopyText = curConfig?.IsOcrAutoCopyText ?? false;

        [ObservableProperty]
        private bool isAdjustContentTranslate = curConfig?.IsAdjustContentTranslate ?? false;

        [ObservableProperty]
        private bool isRemoveLineBreakGettingWords = curConfig?.IsRemoveLineBreakGettingWords ?? false;

        [ObservableProperty]
        private bool _isRemoveLineBreakGettingWordsOCR = curConfig?.IsRemoveLineBreakGettingWordsOCR ?? false;

        [ObservableProperty]
        private DoubleTapFuncEnum doubleTapTrayFunc = curConfig?.DoubleTapTrayFunc ?? DoubleTapFuncEnum.InputFunc;

        [ObservableProperty]
        private List<string> _getFontFamilys;

        private string _customFont = curConfig?.CustomFont ?? ConstStr.DEFAULTFONTNAME;

        public string CustomFont
        {
            get => _customFont;
            set
            {
                if (_customFont == value) return;
                OnPropertyChanging();

                try
                {
                    // 切换字体
                    Application.Current.Resources[ConstStr.USERDEFINEFONTKEY] = value.Equals(ConstStr.DEFAULTFONTNAME)
                        ? Application.Current.Resources[ConstStr.DEFAULTFONTNAME]
                        : new FontFamily(value);
                    _customFont = value;
                }
                catch (Exception)
                {
                    Application.Current.Resources[ConstStr.USERDEFINEFONTKEY] = Application.Current.Resources[ConstStr.DEFAULTFONTNAME];
                    _customFont = ConstStr.DEFAULTFONTNAME;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否在关闭鼠标划词后保持最前
        /// </summary>
        [ObservableProperty]
        private bool isKeepTopmostAfterMousehook = curConfig?.IsKeepTopmostAfterMousehook ?? false;

        /// <summary>
        /// 是否显示设置图标
        /// </summary>
        [ObservableProperty]
        private bool isShowPreference = curConfig?.IsShowPreference ?? false;

        /// <summary>
        /// 是否显示快速配置服务
        /// </summary>
        [ObservableProperty]
        private bool _isShowConfigureService = curConfig?.IsShowConfigureService ?? false;

        /// <summary>
        /// 是否显示打开鼠标划词图标
        /// </summary>
        [ObservableProperty]
        private bool isShowMousehook = curConfig?.IsShowMousehook ?? false;

        /// <summary>
        /// 是否显示打开增量翻译图标
        /// </summary>
        [ObservableProperty]
        private bool isShowIncrementalTranslation = curConfig?.IsShowIncrementalTranslation ?? false;

        /// <summary>
        /// 是否显示截图翻译图标
        /// </summary>
        [ObservableProperty]
        private bool isShowScreenshot = curConfig?.IsShowScreenshot ?? false;

        /// <summary>
        /// 是否显示OCR图标
        /// </summary>
        [ObservableProperty]
        private bool isShowOCR = curConfig?.IsShowOCR ?? false;

        /// <summary>
        /// 是否显示静默OCR图标
        /// </summary>
        [ObservableProperty]
        private bool isShowSilentOCR = curConfig?.IsShowSilentOCR ?? false;

        /// <summary>
        /// 是否显示监听剪贴板
        /// </summary>
        [ObservableProperty]
        private bool isShowClipboardMonitor = curConfig?.IsShowClipboardMonitor ?? false;

        /// <summary>
        /// 是否显示识别二维码图标
        /// </summary>
        [ObservableProperty]
        private bool isShowQRCode = curConfig?.IsShowQRCode ?? false;

        /// <summary>
        /// 是否显示历史记录图标
        /// </summary>
        [ObservableProperty]
        private bool isShowHistory = curConfig?.IsShowHistory ?? false;

        /// <summary>
        /// 取词时间间隔
        /// </summary>
        [ObservableProperty]
        private int wordPickingInterval = curConfig?.WordPickingInterval ?? 100;

        /// <summary>
        /// 启动时隐藏主界面
        /// </summary>
        [ObservableProperty]
        private bool isHideOnStart = curConfig?.IsHideOnStart ?? false;

        /// <summary>
        /// 收缩框是否显示复制按钮
        /// </summary>
        [ObservableProperty]
        private bool showCopyOnHeader = curConfig?.ShowCopyOnHeader ?? false;

        /// <summary>
        /// 激活窗口时光标移动至末尾
        /// </summary>
        [ObservableProperty]
        private bool isCaretLast = curConfig?.IsCaretLast ?? false;

        /// <summary>
        /// 所选代理方式
        /// </summary>
        [ObservableProperty]
        private ProxyMethodEnum _proxyMethod = curConfig?.ProxyMethod ?? ProxyMethodEnum.系统代理;

        /// <summary>
        /// 代理服务器IP
        /// </summary>
        [ObservableProperty]
        private string _proxyIp = curConfig?.ProxyIp ?? string.Empty;

        /// <summary>
        /// 代理服务器端口
        /// </summary>
        [ObservableProperty]
        private int? _proxyPort = curConfig?.ProxyPort ?? 8089;

        /// <summary>
        /// 是否启用代理认证
        /// </summary>
        [ObservableProperty]
        private bool _isProxyAuthentication = curConfig?.IsProxyAuthentication ?? false;

        /// <summary>
        /// 代理认证用户名
        /// </summary>
        [ObservableProperty]
        private string _proxyUsername = curConfig?.ProxyUsername ?? string.Empty;

        /// <summary>
        /// 代理认证密码
        /// </summary>
        [ObservableProperty]
        private string _proxyPassword = curConfig?.ProxyPassword ?? string.Empty;

        /// <summary>
        /// 显示/隐藏密码
        /// </summary>
        [JsonIgnore]
        [ObservableProperty]
        [property: JsonIgnore]
        private bool _isProxyPasswordHide = true;

        private RelayCommand<string>? showEncryptInfoCommand;

        /// <summary>
        /// 显示/隐藏密码Command
        /// </summary>
        [JsonIgnore]
        public IRelayCommand<string> ShowEncryptInfoCommand => showEncryptInfoCommand ??= new RelayCommand<string>(new Action<string?>(ShowEncryptInfo));

        private void ShowEncryptInfo(string? obj)
        {
            if (obj is nameof(ProxyPassword))
            {
                IsProxyPasswordHide = !IsProxyPasswordHide;
            }
        }

        /// <summary>
        /// 翻译后执行自动复制动作(Ctrl+1...9)
        /// </summary>
        [ObservableProperty]
        private int _copyResultAfterTranslateIndex = curConfig?.CopyResultAfterTranslateIndex ?? 0;

        /// <summary>
        /// 是否开启增量翻译
        /// </summary>
        [ObservableProperty]
        private bool _incrementalTranslation = curConfig?.IncrementalTranslation ?? false;

        /// <summary>
        /// 是否开启重复触发显示界面为显示/隐藏
        /// </summary>
        [ObservableProperty]
        private bool _isTriggerShowHide = curConfig?.IsTriggerShowHide ?? false;

        /// <summary>
        /// 是否显示主窗口提示词
        /// </summary>
        [ObservableProperty]
        private bool _isShowMainPlaceholder = curConfig?.IsShowMainPlaceholder ?? true;

        /// <summary>
        /// 截图是否显示辅助线
        /// </summary>
        [ObservableProperty]
        private bool _showAuxiliaryLine = curConfig?.ShowAuxiliaryLine ?? true;

        /// <summary>
        /// 修改语言后立即翻译
        /// </summary>
        [ObservableProperty]
        private bool _changedLang2Execute = curConfig?.ChangedLang2Execute ?? true;

        /// <summary>
        /// OCR修改语言后立即翻译
        /// </summary>
        [ObservableProperty]
        private bool _ocrChangedLang2Execute = curConfig?.OcrChangedLang2Execute ?? true;

        /// <summary>
        /// 使用windows forms库中的Clipboard尝试解决剪贴板占用问题
        /// </summary>
        [ObservableProperty]
        private bool _useFormsCopy = curConfig?.UseFormsCopy ?? true;

        /// <summary>
        /// 使用windows forms库中的Clipboard尝试解决剪贴板占用问题
        /// </summary>
        [ObservableProperty]
        private int? _externalCallPort = curConfig?.ExternalCallPort ?? 50020;

        /// <summary>
        /// 语种识别类型
        /// </summary>
        [ObservableProperty]
        private LangDetectType _detectType = curConfig?.DetectType ?? LangDetectType.Local;

        /// <summary>
        /// 禁用全局热键
        /// </summary>
        [ObservableProperty]
        private bool _disableGlobalHotkeys = curConfig?.DisableGlobalHotkeys ?? false;

        /// <summary>
        /// 主界面最大高度
        /// </summary>
        [ObservableProperty]
        private uint _mainViewMaxHeight = curConfig?.MainViewMaxHeight ?? 840;

        /// <summary>
        /// 主界面宽度
        /// </summary>
        [ObservableProperty]
        private uint _mainViewWidth = curConfig?.MainViewWidth ?? 460;

        [RelayCommand]
        private void MainViewMaxHeightWidthPlus()
        {
            MainViewMaxHeightPlus();
            MainViewWidthPlus();
        }

        [RelayCommand]
        private void MainViewMaxHeightPlus()
        {
            if (MainViewMaxHeight >= 1080) return;
            AnimationHelper.MaxHeightAnimation(MainViewMaxHeight, MainViewMaxHeight + 20);
        }

        [RelayCommand]
        private void MainViewWidthPlus()
        {
            if (MainViewWidth >= 1920) return;
            AnimationHelper.WidthAnimation(MainViewWidth, MainViewWidth + 20);
        }

        [RelayCommand]
        private void MainViewMaxHeightWidthMinus()
        {
            MainViewMaxHeightMinus();
            MainViewWidthMinus();
        }

        [RelayCommand]
        private void MainViewMaxHeightMinus()
        {
            if (MainViewMaxHeight <= 400) return;
            AnimationHelper.MaxHeightAnimation(MainViewMaxHeight, MainViewMaxHeight - 20);
        }

        [RelayCommand]
        private void MainViewWidthMinus()
        {
            if (MainViewWidth <= 400) return;
            AnimationHelper.WidthAnimation(MainViewWidth, MainViewWidth - 20);
        }
    }
}