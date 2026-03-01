#if USE_LUA
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Celeste.Lua.Proxies;
using Lua;
using Lua.Standard;
using Lua.Unity;
using UnityEngine;
using UnityEngine.UIElements;

namespace Celeste.Lua
{
    public sealed class UIToolkitLibrary : ILuaLibrary
    {
        #region Properties and Fields
        
        public static readonly UIToolkitLibrary Instance = new();
        
        public string Name => kName;
        public IReadOnlyList<LibraryFunction> Functions => functions;
        public LuaRuntime LuaRuntime { private get; set; }

        private const string kName = "UIToolkit";
        private readonly List<LibraryFunction> functions = new();

        #endregion

        private UIToolkitLibrary()
        {
            functions.Add(new(kName, "label", Label));
            functions.Add(new(kName, "textField", TextField));
            functions.Add(new(kName, "intField", IntField));
            functions.Add(new(kName, "floatField", FloatField));
            functions.Add(new(kName, "toggle", Toggle));
            functions.Add(new(kName, "foldout", Foldout));
            functions.Add(new(kName, "list", List));
            functions.Add(new(kName, "visualElement", VisualElement));
            functions.Add(new(kName, "button", Button));
        }
        
        private ValueTask<int> Label(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string fieldLabel = settings.GetString("label") ?? string.Empty;
            LuaTable style = settings.GetTable("style");
            
            var label = new Label(fieldLabel);
            
            ApplyStyle(label, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(label)));
        }
        
        private ValueTask<int> TextField(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string fieldLabel = settings.GetString("label") ?? string.Empty;
            string fieldValue = settings.GetString("value") ?? string.Empty;
            bool isDelayed = settings.GetBool("delayed");
            LuaFunction valueChanged = settings.GetFunction("valueChanged");
            LuaTable style = settings.GetTable("style");

            var textField = new TextField(fieldLabel) { value = fieldValue, isDelayed = isDelayed };
            textField.RegisterValueChangedCallback(evt =>
            {
                UnityEngine.Debug.Assert(LuaRuntime != null, $"Lua Runtime not set on {nameof(UIToolkitLibrary)}!");
                LuaRuntime.ExecuteFunctionAsync(valueChanged, evt.newValue);
            });
            
            ApplyStyle(textField, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(textField)));
        }
        
        private ValueTask<int> IntField(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string fieldLabel = settings.GetString("label") ?? string.Empty;
            int fieldValue = settings.GetInt("value");
            bool isDelayed = settings.GetBool("delayed");
            LuaFunction valueChanged = settings.GetFunction("valueChanged");
            LuaTable style = settings.GetTable("style");
            
            var intField = new IntegerField(fieldLabel) { value = fieldValue, isDelayed = isDelayed };
            intField.RegisterValueChangedCallback(evt =>
            {
                UnityEngine.Debug.Assert(LuaRuntime != null, $"Lua Runtime not set on {nameof(UIToolkitLibrary)}!");
                LuaRuntime.ExecuteFunctionAsync(valueChanged, evt.newValue);
            });
            
            ApplyStyle(intField, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(intField)));
        }
        
        private ValueTask<int> FloatField(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string fieldLabel = settings.GetString("label") ?? string.Empty;
            float fieldValue = settings.GetFloat("value");
            bool isDelayed = settings.GetBool("delayed");
            LuaFunction valueChanged = settings.GetFunction("valueChanged");
            LuaTable style = settings.GetTable("style");

            var floatField = new FloatField(fieldLabel) { value = fieldValue, isDelayed = isDelayed };
            floatField.RegisterValueChangedCallback(evt =>
            {
                UnityEngine.Debug.Assert(LuaRuntime != null, $"Lua Runtime not set on {nameof(UIToolkitLibrary)}!");
                LuaRuntime.ExecuteFunctionAsync(valueChanged, evt.newValue);
            });
            
            ApplyStyle(floatField, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(floatField)));
        }
        
        private ValueTask<int> Toggle(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string fieldLabel = settings.GetString("label") ?? string.Empty;
            bool fieldValue = settings.GetBool("value");
            LuaFunction valueChanged = settings.GetFunction("valueChanged");
            LuaTable style = settings.GetTable("style");

            var toggle = new Toggle(fieldLabel) { value = fieldValue };
            toggle.RegisterValueChangedCallback(evt =>
            {
                UnityEngine.Debug.Assert(LuaRuntime != null, $"Lua Runtime not set on {nameof(UIToolkitLibrary)}!");
                LuaRuntime.ExecuteFunctionAsync(valueChanged, evt.newValue);
            });
            
            ApplyStyle(toggle, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(toggle)));
        }
        
        private ValueTask<int> Foldout(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string fieldLabel = settings.GetString("label") ?? string.Empty;
            bool fieldValue = settings.GetBool("expanded");
            LuaTable style = settings.GetTable("style");

            var foldout = new Foldout { text = fieldLabel, value = fieldValue };
            
            ApplyStyle(foldout, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(foldout)));
        }
        
        private ValueTask<int> List(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            LuaValue listProxyValue = settings.GetValue("items");
            int itemHeight = settings.GetInt("itemHeight", 40);
            bool showAddRemoveFooter = settings.GetBool("showAddRemoveFooter", true);
            bool reorderable = settings.GetBool("reorderable", true);
            LuaTable style = settings.GetTable("style");
            LuaFunction makeItemFunction = settings.GetFunction("makeItem");
            LuaFunction bindItemFunction = settings.GetFunction("bindItem");
            LuaFunction itemAddedFunction = settings.GetFunction("itemAdded");
            LuaFunction itemRemovedFunction = settings.GetFunction("itemRemoved");
            
            var listView = new ListView
            {
                itemsSource = listProxyValue.As<LuaListProxy>().List,
                fixedItemHeight = itemHeight,
                showAddRemoveFooter = showAddRemoveFooter,
                reorderable = reorderable,
                makeItem = () =>
                {
                    VisualElement visualElement = new VisualElement();
                    LuaRuntime.ExecuteFunctionAsync(makeItemFunction, LuaValue.FromObject(new LuaVisualElementProxy(visualElement)));
                    return visualElement;
                },
                bindItem = (element, index) =>
                {
                    LuaRuntime.ExecuteFunctionAsync(bindItemFunction, LuaValue.FromObject(new LuaVisualElementProxy(element)), index);
                }
            };

            listView.itemsAdded += (indices) =>
            {
                foreach (int index in indices)
                {
                    LuaRuntime.ExecuteFunctionAsync(itemAddedFunction, index); 
                }
            };
            listView.itemsRemoved += (indices) =>
            {
                foreach (int index in indices)
                {
                    LuaRuntime.ExecuteFunctionAsync(itemRemovedFunction, index); 
                }
            };
            
            ApplyStyle(listView, style);

            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(listView)));
        }

        private ValueTask<int> VisualElement(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            LuaTable style = settings.GetTable("style");
            VisualElement visualElement = new VisualElement();
            
            ApplyStyle(visualElement, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(visualElement)));
        }

        private ValueTask<int> Button(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string text = settings.GetString("text", string.Empty);
            LuaTable style = settings.GetTable("style");
            LuaFunction callback = settings.GetFunction("callback");
            Button button = new Button(() =>
            {
                LuaRuntime.ExecuteFunctionAsync(callback);
            }) { text = text };
            
            ApplyStyle(button, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(button)));
        }

        public static void ApplyStyle(VisualElement visualElement, LuaTable luaTable)
        {
            if (luaTable == null || luaTable == LuaValue.Nil)
            {
                return;
            }

            // Min Height
            {
                if (luaTable.TryGetValue("minHeight", out LuaValue minHeightValue) &&
                    minHeightValue.TryRead(out float minHeight))
                {
                    visualElement.style.minHeight = minHeight;
                }
            }

            // Flex Direction
            {
                string flexDirectionString = luaTable.GetString("flexDirection");
                if (!string.IsNullOrEmpty(flexDirectionString))
                {
                    if (Enum.TryParse(flexDirectionString, out FlexDirection flexDirection))
                    {
                        visualElement.style.flexDirection = flexDirection;
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion(
                            $"Failed to parse flex direction string {flexDirectionString} as a {nameof(FlexDirection)} value.");
                    }
                }
            }

            // Align Items
            {
                string alignItemsString = luaTable.GetString("alignItems");
                if (!string.IsNullOrEmpty(alignItemsString))
                {
                    if (Enum.TryParse(alignItemsString, out Align alignItems))
                    {
                        visualElement.style.alignItems = alignItems;
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion(
                            $"Failed to parse align items string {alignItemsString} as an {nameof(Align)} value.");
                    }
                }
            }

            // Flex Grow
            {
                if (luaTable.TryGetValue("flexGrow", out LuaValue flexGrowValue) &&
                    flexGrowValue.TryRead(out float flexGrow))
                {
                    visualElement.style.flexGrow = flexGrow;
                }
            }

            // Font Style And Weight
            {
                string fontStyleAndWeight = luaTable.GetString("fontStyleAndWeight");
                if (!string.IsNullOrEmpty(fontStyleAndWeight))
                {
                    if (Enum.TryParse(fontStyleAndWeight, out FontStyle fontStyle))
                    {
                        visualElement.style.unityFontStyleAndWeight = fontStyle;
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion(
                            $"Failed to parse align items string {fontStyleAndWeight} as an {nameof(FontStyle)} value.");
                    }
                }
            }
        }
    }
}
#endif