using Bright.Serialization;
using System.Collections.Generic;

{{
    name = x.name
    namespace = x.namespace
    tables = x.tables

}}

{{cs_start_name_space_grace x.namespace}} 
public partial class {{name}}
{
    {{~for table in tables ~}}
{{~if table.comment != '' ~}}
    /// <summary>
    /// {{table.escape_comment}}
    /// </summary>
{{~end~}}

    private {{table.full_name}} m_{{table.name}};
    private string m_{{table.name}}DataName = "{{table.output_data_file}}";

    public {{table.full_name}} {{table.name}} {
        get
        {
            if (m_{{table.name}} == null)
            {
                m_{{table.name}} = new {{table.full_name}}(); 
                m_{{table.name}}.Load(m_tableDatas[m_{{table.name}}DataName]);
                m_tableDatas.Remove(m_{{table.name}}DataName);
            }
            return m_{{table.name}};
        }
    }
    {{~end~}}
	
	public List<string> TableNames { get; private set; } = new List<string>()
	{
		{{~for table in tables ~}}
		"{{table.output_data_file}}",
		{{~end~}}
	};

    private Dictionary<string, ByteBuf> m_tableDatas = new Dictionary<string, ByteBuf>();
	
    public void Load(Dictionary<string, byte[]> tableDatas)
    {
        m_tableDatas.Clear();
        foreach (var pairs in tableDatas)
        {
            m_tableDatas.Add(pairs.Key, new ByteBuf(pairs.Value));
        }
        //var tables = new System.Collections.Generic.Dictionary<string, object>();
        {{~for table in tables ~}}

        //m_{{table.name}}DataName = "{{table.output_data_file}}";
        //tables.Add("{{table.full_name}}", m_{{table.name}});

        {{~end~}}

        /* 这里项目没有用到,所以注释掉了
        PostInit();
        {{~for table in tables ~}}
        {{table.name}}.Resolve(tables); 
        {{~end~}}
        PostResolve();
        */
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        {{~for table in tables ~}}
        {{table.name}}.TranslateText(translator); 
        {{~end~}}
    }
    
    partial void PostInit();
    partial void PostResolve();
}

{{cs_end_name_space_grace x.namespace}}