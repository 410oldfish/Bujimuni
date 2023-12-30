using System.Collections.Generic;
using System.Text;
using Lighten;
using Newtonsoft.Json;

[GameSave(true, "AccountData")]
public class GameSaveTemplateScript : GameSave
{
    public class Data
    {
        public int a;
        public string b;
        public bool c;
        public List<int> d;
    }
    
    // 生成二进制数据
    public override byte[] Generate()
    {
        //从LightenEntry.GameDataManager取你要存储的数据
        var data = new Data();
        data.a = 1;
        data.b = "hello world";
        data.c = true;
        data.d = new List<int>() { 1, 2, 3, 4, 5 };
        var json = JsonConvert.SerializeObject(data);
        return Encoding.UTF8.GetBytes(json);
    }

    // 载入二进制数据
    public override void Load(byte[] datas)
    {
        var json = Encoding.UTF8.GetString(datas);
        var data = JsonConvert.DeserializeObject<Data>(json);
        //设置到LightenEntry.GameDataManager的数据中
    }
}
