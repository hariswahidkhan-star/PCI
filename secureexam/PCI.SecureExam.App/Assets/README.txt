Place haarcascade_frontalface_default.xml here.
It ships with the OpenCvSharp4.runtime.win package (copy from the package's
runtimes folder) or download from the OpenCV repository. The .csproj copies it
to the output directory. CameraService also falls back to the OpenCvSharp
built-in cascade path if this file is absent.
