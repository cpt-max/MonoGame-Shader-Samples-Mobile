[< Back to overview](https://github.com/cpt-max/MonoGame-Shader-Samples-Mobile/tree/overview)

# Particle Compute Shader for MonoGame

![Screenshots](https://github.com/cpt-max/MonoGame-Shader-Samples/blob/overview/Screenshots/ComputeParticles.jpg?raw=true)

This sample uses a compute shader to update particles on the GPU. 
The particle buffer is used directly by the vertex shader that draws the particles. Since no data needs to be downloaded to the CPU, this method is very fast.

## Build Instructions
The custom MonoGame fork used in this sample is available as a NuGet package, no need to build it yourself.<br>
.NET 6 and the Android SDK needs to be installed. Either use Visual Studio 2022 or install them manually. On Linux you may also need to install the Java SDK.<br>
You can just open the csproj file in Visual Studio 2022, or launch directly from the command line:
```
dotnet run
```

Here are more details about [NuGet packages, platform support and build requirements](https://github.com/cpt-max/Docs/blob/master/Build%20Requirements.md).
<br><br>

[< Back to overview](https://github.com/cpt-max/MonoGame-Shader-Samples-Mobile/tree/overview)




