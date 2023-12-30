
namespace Lighten
{
    /*
     * 可为空的值类型
     * 与C# Nullable一致,为了避免直接使用mscorlib.dll中泛型,企图避免HybridClr的补元
     */
    public struct NullableValue<T> where T : struct
    {
        private bool m_hasValue;
        private T m_value;

        public bool IsNull => !this.m_hasValue;
        
        public NullableValue(T value)
        {
            this.m_hasValue = true;
            this.m_value = value;
        }
        
        public static implicit operator NullableValue<T>(T value)
        {
            return new NullableValue<T>(value);
        }

        public static implicit operator T(NullableValue<T> v)
        {
            return v.m_value;
        }
    }
}