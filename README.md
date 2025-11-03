# Usage
#### Initialize Engine:
```csharp
Engine.Init();
```
#### Main Fuctionals:
```csharp
SceneManager.Current = new Video(@".../video.mp4", pixelsPerCell: 12, colors: DefaultValues.Colors4, useAscii: false);
//SceneManager.Current = new VideoFromText(@".../videoText.txt", height: 24, width: 60, fps: 24);
```
#### Game Tests:
```csharp
SceneManager.Current = new GameTest_AlgCircle(height: 30, width: 30, DefaultValues.Colors2);
SceneManager.Current = new GameTest_AlgRectangle(height: 30, width: 30, DefaultValues.Colors2);
SceneManager.Current = new GameTest_Glitch(height: 30, width: 30, DefaultValues.Colors4);
```

//SceneManager.Current = new GameTest_AlgCircle(height: 30, width: 30, DefaultValues.Colors4);
SceneManager.Current = new GameTest_AlgRectangle(height: 30, width: 30, DefaultValues.Colors4);
Engine.fpsMax = 100;

#### (Optional) Override Passer (rendering method) to 1 of:
```csharp
Renderer.Passer = new PasserColorChangeSets();
Renderer.Passer = new PasserFull();
```

#### Start
```csharp
Engine.Start();
```

# Installation
```
git clone -b old https://github.com/DocStep/ConsoleRenderer.git
```
```
cd ConsoleRenderer
```
```
dotnet build
```