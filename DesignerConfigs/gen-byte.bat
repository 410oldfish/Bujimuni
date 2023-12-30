set GEN_CLIENT=dotnet ..\Tools\Luban\Luban.ClientServer\Luban.ClientServer.dll

set OUTPUT_DATA=..\Source\Assets\_GameClient\Raw\Configs
set OUTPUT_CODE=..\Source\Assets\_GameClient\Scripts\Runtime\Hotfix\GameConfig\Gen

%GEN_CLIENT% -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_data_dir %OUTPUT_DATA% ^
 --output_code_dir %OUTPUT_CODE% ^
 --gen_types code_cs_unity_bin,data_bin ^
 -s all

cd %OUTPUT_DATA%
dir /b *.bytes >configlist.txt

pause