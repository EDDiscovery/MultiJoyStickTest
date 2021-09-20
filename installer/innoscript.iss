; MJT script

#define MyAppName "MultiJoystickTester"
#ifndef MyAppVersion
#define MyAppVersion "11.4.505"
#endif
#define MyAppPublisher "Robbyxp1 @ github.com"
#define MyAppURL "https://github.com/EDDiscovery/MultiJoyStickTest"
#define MyAppExeName "JoystickTest.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AllowUNCPath=no
AppId={{66D786F5-B09D-F1B4-6910-2983DEAD5083}
AppName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AppVerName={#MyAppName} {#MyAppVersion}
AppVersion={#MyAppVersion}
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=no
DisableWelcomePage=no
DirExistsWarning=auto
LicenseFile="{#SourcePath}\License.rtf"
OutputBaseFilename={#MyAppName}-{#MyAppVersion}
OutputDir="{#SourcePath}\installers"
SolidCompression=yes
SourceDir="{#SourcePath}\..\JoystickTest\bin\Release"
UninstallDisplayIcon={app}\{#MyAppExeName}
UsePreviousTasks=no
UsePreviousAppDir=yes

WizardImageFile="{#SourcePath}\Logo.bmp"
WizardImageStretch=no
WizardStyle=modern
WizardSizePercent=150

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "JoystickTest.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "JoystickTest.exe.config"; DestDir: "{app}"; Flags: ignoreversion

Source: "*.dll"; DestDir: "{app}"; Flags: ignoreversion;

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Messages]
SelectDirBrowseLabel=To continue, click Next.
ConfirmUninstall=Are you sure you want to completely remove %1 and all of its components? Note that all your user data is not removed by this uninstall and is still stored in your local app data


