!define PRODUCT_NAME "Taq-e Bostan CMS"
!define PRODUCT_VERSION "v1.0"
!define PRODUCT_PUBLISHER "Babaei.net"
!define PRODUCT_WEB_SITE "http://www.Babaei.net/"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\taqebostan.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define PRODUCT_STARTMENU_REGVAL "NSIS:StartMenuDir"

SetCompressor /SOLID lzma
BrandingText /TRIMLEFT "http://www.Babaei.net/"

; MUI 1.67 compatible ------
!include "MUI.nsh"
;!include "Library.nsh"

;For installing font - You must install FontName-0.7.exe before use this feature
!include FontRegAdv.nsh
!include FontName.nsh

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "install.ico"
!define MUI_UNICON "uninstall.ico"

; Language Selection Dialog Settings
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
;!insertmacro MUI_PAGE_LICENSE "agreement.txt"
 !insertmacro MUI_PAGE_LICENSE $(MUILicense)
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Start menu page
var ICONS_GROUP
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "ÊÇÞ ÈÓÊÇä"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${PRODUCT_STARTMENU_REGVAL}"
!insertmacro MUI_PAGE_STARTMENU Application $ICONS_GROUP
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\taqebostan.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Farsi"
!insertmacro MUI_LANGUAGE "Kurdish"

;For installing font - You must install FontName-0.7.exe before use this feature
;!insertmacro FontName "BTraffic.ttf"
;or
;!insertmacro FontNameVer
!define FontBackup Reg\key\To\Backup\Fonts\entries\To

;--------------------------------

;License Language String

  LicenseLangString MUILicense ${LANG_ENGLISH} "agreement.en.txt"
  LicenseLangString MUILicense ${LANG_FARSI} "agreement.fa.txt"
  LicenseLangString MUILicense ${LANG_KURDISH} "agreement.en.txt"

; A multilingual message

  LangString MessageSuccess ${LANG_ENGLISH} "$(^Name) was successfully removed from your computer."
  LangString MessageSuccess ${LANG_FARSI} "$(^Name) ÈÇ ãæÝÞíÊ ÇÒ ÓíÓÊã ÔãÇ ÍÐÝ ÔÏ."
  LangString MessageSuccess ${LANG_KURDISH} "$(^Name) was successfully removed from your computer."

  LangString MessageConfirm ${LANG_ENGLISH} "Are you sure you want to completely remove $(^Name) and all of its components?"
  LangString MessageConfirm ${LANG_FARSI} "ÂíÇ ãÇíá Èå ÍÐÝ ßÇãá $(^Name) æ ÊãÇãí ãæáÝå åÇí Âä åÓÊíÏ(ãØãÆäíÏ)¿"
  LangString MessageConfirm ${LANG_KURDISH} "Are you sure you want to completely remove $(^Name) and all of its components?"
#For Multi Language Icons
#  LangString MUI_STARTMENUPAGE_DEFAULTFOLDER ${LANG_ENGLISH} "KUMS - The Assistant of Hygiene"
#  LangString MUI_STARTMENUPAGE_DEFAULTFOLDER ${LANG_FARSI} "ÊÇÞ ÈÓÊÇä"
#  LangString MUI_STARTMENUPAGE_DEFAULTFOLDER ${LANG_KURDISH} "KUMS - The Assistant of Hygiene"
;--------------------------------


; Reserve files
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "taqebostanSetup.exe"
InstallDir "$PROGRAMFILES\Babaei.net\Taqebostanir\cmsv10"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Function .onInit
  SetSilent normal

  StrCpy "$LANGUAGE" "1065"
  !insertmacro MUI_LANGDLL_DISPLAY

 System::Call 'kernel32::CreateMutexA(i 0, i 0, t "myMutex") i .r1 ?e'
 Pop $R0
 
 StrCmp $R0 0 +3
   MessageBox MB_OK|MB_ICONEXCLAMATION "The installer is already running."
   Abort
FunctionEnd

#For Multi Language Icons
#Var ICONMAIN
#Var ICONUNINST

Section "MainSection" SEC01
  ExecWait '"Components\WindowsXP-KB942288-v3-x86.exe" /quiet /norestart'
  ExecWait '"Components\NetFx20SP1_x86.exe" /q /norestart /lang:ENU'
  ExecWait '"Components\mdac_typ.exe" /Q:A /C:"dasetup /Q /N"'
  ExecWait '"Components\MDAC281-KB911562-x86-ENU.exe" /quiet /norestart'
  ExecWait '"Components\MDAC281-KB927779-x86-ENU.exe" /quiet /norestart'

;  SetOutPath "$WINDIR\Fonts\"
;  SetOverwrite on
;  File "BTraffic.ttf"
;  ExecWait "$WINDIR\explorer.exe $WINDIR\Fonts\"

  SetOutPath "$INSTDIR"
  SetOverwrite on
  File "taqebostan.exe"
;  File "taqebostan.XmlSerializers.dll"
  File "MagicLibrary.dll"
  File "Microsoft.mshtml.dll"
  File "Microsoft.ReportViewer.Common.dll"
  File "Microsoft.ReportViewer.ProcessingObjectModel.dll"
  File "Microsoft.ReportViewer.WinForms.dll"
  File "reports.rpt"
  ;!insertmacro InstallLib REGDLL NOTSHARED REBOOT_NOTPROTECTED Microsoft.mshtml.dll $INSTDIR\Microsoft.mshtml.dll $INSTDIR
;  RegDLL $INSTDIR\Microsoft.mshtml.dll
;  RegDLL $INSTDIR\Microsoft.ReportViewer.Common.dll
;  RegDLL $INSTDIR\Microsoft.ReportViewer.ProcessingObjectModel.dll
;  RegDLL $INSTDIR\Microsoft.ReportViewer.WinForms.dll

; Shortcuts
#For Multi Language Icons
#StrCmp $LANGUAGE "1065" 0 EnKr
#  StrCpy "$ICONS_GROUP" "ÊÇÞ ÈÓÊÇä"
#  StrCpy "$ICONMAIN" "ÊÇÞ ÈÓÊÇä.lnk"
#  StrCpy "$ICONUNINST" "ÍÐÝ.lnk"
#  Goto Over
#EnKr:
#  StrCpy "$ICONS_GROUP" "KUMS - The Assistant of Hygiene"
#  StrCpy "$ICONMAIN" "KUMS - The Assistant of Hygiene.lnk"
#  StrCpy "$ICONUNINST" "Uninstall.lnk"
#Over:

#For Multi Language Icons
#  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
#  CreateDirectory "$SMPROGRAMS\$ICONS_GROUP"
#  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\$ICONMAIN" "$INSTDIR\taqebostan.exe"
#  CreateShortCut "$DESKTOP\$ICONMAIN" "$INSTDIR\taqebostan.exe"
#  CreateShortCut "$STARTMENU\$ICONMAIN" "$INSTDIR\taqebostan.exe"
#  CreateShortCut "$SMPROGRAMS\$ICONMAIN" "$INSTDIR\taqebostan.exe"
#  CreateShortCut "$QUICKLAUNCH\$ICONMAIN" "$INSTDIR\taqebostan.exe"
#  !insertmacro MUI_STARTMENU_WRITE_END

  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  CreateDirectory "$SMPROGRAMS\$ICONS_GROUP"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\ÊÇÞ ÈÓÊÇä.lnk" "$INSTDIR\taqebostan.exe"
  CreateShortCut "$DESKTOP\ÊÇÞ ÈÓÊÇä.lnk" "$INSTDIR\taqebostan.exe"
  CreateShortCut "$STARTMENU\ÊÇÞ ÈÓÊÇä.lnk" "$INSTDIR\taqebostan.exe"
  CreateShortCut "$SMPROGRAMS\ÊÇÞ ÈÓÊÇä.lnk" "$INSTDIR\taqebostan.exe"
  CreateShortCut "$QUICKLAUNCH\ÊÇÞ ÈÓÊÇä.lnk" "$INSTDIR\taqebostan.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -AdditionalIcons
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
#For Multi Language Icons
#  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\$ICONUNINST" "$INSTDIR\uninst.exe"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\ÍÐÝ.lnk" "$INSTDIR\uninst.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\taqebostan.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\taqebostan.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK $(MessageSuccess)
FunctionEnd

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 $(MessageConfirm) IDYES +2
  Abort
FunctionEnd

Section Uninstall
  ;!insertmacro UnInstallLib REGDLL NOTSHARED REBOOT_NOTPROTECTED $INSTDIR\Microsoft.mshtml.dll
;  UnRegDLL $INSTDIR\Microsoft.mshtml.dll
;  UnRegDLL $INSTDIR\Microsoft.ReportViewer.Common.dll
;  UnRegDLL $INSTDIR\Microsoft.ReportViewer.ProcessingObjectModel.dll
;  UnRegDLL $INSTDIR\Microsoft.ReportViewer.WinForms.dll

#For Multi Language Icons
#  !insertmacro MUI_STARTMENU_GETFOLDER "Application" $ICONS_GROUP
#  Delete "$SMPROGRAMS\$ICONS_GROUP\$ICONUNINST"
#  Delete "$STARTMENU\$ICONMAIN"
#  Delete "$SMPROGRAMS\$ICONMAIN"
#  Delete "$QUICKLAUNCH\$ICONMAIN"
#  Delete "$DESKTOP\$ICONMAIN"
#  Delete "$SMPROGRAMS\$ICONS_GROUP\$ICONMAIN"

  !insertmacro MUI_STARTMENU_GETFOLDER "Application" $ICONS_GROUP
  Delete "$SMPROGRAMS\$ICONS_GROUP\ÍÐÝ.lnk"
  Delete "$STARTMENU\ÊÇÞ ÈÓÊÇä.lnk"
  Delete "$SMPROGRAMS\ÊÇÞ ÈÓÊÇä.lnk"
  Delete "$QUICKLAUNCH\ÊÇÞ ÈÓÊÇä.lnk"
  Delete "$DESKTOP\ÊÇÞ ÈÓÊÇä.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\ÊÇÞ ÈÓÊÇä.lnk"
  
  Delete "$INSTDIR\taqebostan.exe"
;  Delete "$INSTDIR\taqebostan.XmlSerializers.dll"
  Delete "$INSTDIR\MagicLibrary.dll"
  Delete "$INSTDIR\Microsoft.mshtml.dll"
  Delete "$INSTDIR\Microsoft.ReportViewer.Common.dll"
  Delete "$INSTDIR\Microsoft.ReportViewer.ProcessingObjectModel.dll"
  Delete "$INSTDIR\Microsoft.ReportViewer.WinForms.dll"
  Delete "$INSTDIR\reports.rpt"
  Delete "$INSTDIR\uninst.exe"

  RMDir "$SMPROGRAMS\$ICONS_GROUP"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd



################### Fonts - See Above ####################
Section "Fonts"
; Alternate for older versions of NSIS: pre NSIS v2.0rc1
;  push $1
;  System::Call "Shell32::SHGetSpecialFolderLocation(i $HWNDPARENT, i ${CSIDL_FONTS}|${CSIDL_FLAG_CREATE}, *i .r0)"
;  System::Call "Shell32::SHGetPathFromIDList(i r0, t .r1)"
;  System::Call 'shell32::SHGetMalloc(*i . r2)' ; IMalloc
;  System::Call '$2->5(i r0)' ; ->Free
;  System::Call '$2->2()' ; ->Release
;  StrCpy $FONT_DIR $1
;  pop $1

  StrCpy $FONT_DIR $FONTS

  !insertmacro InstallTTF 'BTraffic.ttf'
  !insertmacro InstallTTF 'BTrafcBd.ttf'

;  SendMessage ${HWND_BROADCAST} ${WM_FONTCHANGE} 0 0 /TIMEOUT=5000
SectionEnd

Section "un.Fonts"
; Alternate for older versions of NSIS: pre NSIS v2.0rc1
;  push $1
;  System::Call "Shell32::SHGetSpecialFolderLocation(i $HWNDPARENT, i ${CSIDL_FONTS}|${CSIDL_FLAG_CREATE}, *i .r0)"
;  System::Call "Shell32::SHGetPathFromIDList(i r0, t .r1)"
;  System::Call 'shell32::SHGetMalloc(*i . r2)' ; IMalloc
;  System::Call '$2->5(i r0)' ; ->Free
;  System::Call '$2->2()' ; ->Release
;  StrCpy $FONT_DIR $1
;  pop $1

  StrCpy $FONT_DIR $FONTS

;  !insertmacro RemoveTTF 'BTraffic.ttf'
;  !insertmacro RemoveTTF 'BTrafcBd.ttf'

;  SendMessage ${HWND_BROADCAST} ${WM_FONTCHANGE} 0 0 /TIMEOUT=5000
SectionEnd
################### Fonts - See Above ####################

Section "EndInstall"
  SetOutPath "$INSTDIR"
;  Sleep 3000
SectionEnd