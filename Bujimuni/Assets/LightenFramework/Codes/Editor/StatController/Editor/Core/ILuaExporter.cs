using System;
using System.Collections.Generic;


public interface ILuaExporter
{
    bool GenerateFile(StatController statController, string filePath);

    List<Type> GetCanExportTyps();

    List<Type> GetForceIgnoreTyps();

}