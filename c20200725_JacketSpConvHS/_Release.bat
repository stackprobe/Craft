C:\Factory\Tools\RDMD.exe /RC out

COPY Conv\Conv\bin\Release\*.exe out
COPY Conv\Conv\bin\Release\*.dll out

COPY JacketSpConv\JacketSpConv\bin\Release\*.exe out
COPY JacketSpConv\JacketSpConv\bin\Release\*.dll out

C:\Factory\SubTools\makeDDResourceFile.exe ConvGenVideo\Resource out\Resource.dat ConvGenVideo\Tools\MaskGZData.exe

rem C:\Factory\SubTools\CallConfuserCLI.exe ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\ConvGenVideo.exe out\ConvGenVideo.exe
COPY /B ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\ConvGenVideo.exe out
COPY /B ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\Chocolate.dll out
COPY /B ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\DxLib.dll out
COPY /B ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\DxLib_x64.dll out
COPY /B ConvGenVideo\ConvGenVideo\ConvGenVideo\bin\Release\DxLibDotNet.dll out

C:\Factory\Tools\xcp.exe doc out
C:\Factory\Tools\xcp.exe C:\Dev\Fairy\Donut2\doc out

C:\Factory\SubTools\zip.exe /O out ConvGenVideoHS

PAUSE
