[< Back to overview](https://github.com/cpt-max/MonoGame-Shader-Samples-Mobile/tree/overview)
# Tessellation and Geometry Shader for MonoGame

![Screenshot](https://github.com/cpt-max/MonoGame-Shader-Samples/blob/overview/Screenshots/TesselationGeometry.jpg?raw=true)

This sample uses a very simple hull and domain shader to tessellate a single input triangle into many sub triangles. Each sub triangle is then passed into a geometry shader to generate even more triangles along it's edges, which creates a wireframe-like effect.

You can switch between different techniques, which effectively lets you disable tessellation and/or the geometry shader.


## Build Instructions
The custom MonoGame fork used in this sample is available as a NuGet package, no need to build it yourself.<br>
.NET 6 and the Android SDK needs to be installed. Either use Visual Studio 2022 or install them manually. On Linux you may also need to install the Java SDK.<br>
You can just open the csproj files in Visual Studio 2022, or launch directly from the command line:
```
dotnet run
```

Here are more details about [NuGet packages, platform support and build requirements](https://github.com/cpt-max/Docs/blob/master/Build%20Requirements.md).
<br><br>

[< Back to overview](https://github.com/cpt-max/MonoGame-Shader-Samples-Mobile/tree/overview)

