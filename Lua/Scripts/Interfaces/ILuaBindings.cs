namespace Celeste.Lua
{
    public interface ILuaBindings
    {
        void Bind(LuaRuntime luaRuntime);
        void Unbind(LuaRuntime luaRuntime);
    }
}