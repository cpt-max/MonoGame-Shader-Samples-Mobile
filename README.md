# MonoGame Shader Samples Overview

Each sample is in a separate branch of this repository, so don't use this branch, pick a sample from below first.<br>
The samples are based on a [custom MonoGame fork](https://github.com/cpt-max/MonoGame), that adds tesselation, geometry and compute shaders.<br>
You don't need to build the fork in order to run the samples, as they use prebuilt NuGet packages. As long as .Net 6 is installed, they should just launch.<br>

[Compute Shader Guide for MonoGame](https://github.com/cpt-max/Docs/blob/master/MonoGame%20Compute%20Shader%20Guide.md)<br>
[NuGet Packages, Platform Support and Build Requirements](https://github.com/cpt-max/Docs/blob/master/Build%20Requirements.md)<br>
[Pull Request for main MonoGame Repo](https://github.com/MonoGame/MonoGame/pull/7533)<br>
[MonoGame Forum Post](https://community.monogame.net/t/compute-tessellation-geometry-shader/16676)<br>
[Download Prebuilt Executables](https://www.dropbox.com/s/v4gg77pzbniykha/Monogame.Shader.Samples.zip?dl=1) (win-x64, linux-x64, requires .Net 5+)
<br><br>

## [Simple Tessellation & Geometry Shader](https://github.com/cpt-max/MonoGame-Shader-Samples-Mobile/tree/tesselation_geometry)
[<img align="left" width="300" src="Screenshots/TesselationGeometry.jpg">](https://github.com/cpt-max/MonoGame-Shader-Samples/tree/tesselation_geometry)
This sample uses a very simple hull and domain shader to tessellate a single input triangle into many sub triangles. Each sub triangle is then passed into a geometry shader to generate even more triangles along it's edges, which creates a wireframe-like effect.
<br clear="left"/><br>

## [Particle Compute Shader](https://github.com/cpt-max/MonoGame-Shader-Samples-Mobile/tree/compute_gpu_particles)
[<img align="left" width="300" src="Screenshots/ComputeParticles.jpg">](https://github.com/cpt-max/MonoGame-Shader-Samples/tree/compute_gpu_particles)
This sample uses a compute shader to update particles on the GPU. The particle buffer is used directly by the vertex shader that draws the particles. Since no data needs to be downloaded to the CPU, this method is very fast.
<br clear="left"/><br>










