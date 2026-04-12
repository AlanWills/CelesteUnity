#if USE_LUA
using System;
using System.Collections;
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
            functions.Add(new(kName, "dropdown", Dropdown));
            functions.Add(new (kName, "applyStyle", ApplyStyle));
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
            string fieldLabel = settings.GetString("label");
            string fieldText = settings.GetString("text");
            bool isDelayed = settings.GetBool("delayed");
            bool multiline = settings.GetBool("multiline");
            LuaFunction valueChanged = settings.GetFunction("valueChanged");
            LuaTable style = settings.GetTable("style");

            var textField = new TextField(fieldLabel) { value = fieldText, isDelayed = isDelayed, multiline = multiline};
            textField.RegisterValueChangedCallback(evt =>
            {
                UnityEngine.Debug.Assert(LuaRuntime != null, $"Lua Runtime not set on {nameof(UIToolkitLibrary)}!");
                LuaRuntime.ExecuteFunctionAsync(valueChanged, evt.newValue);
            });
            
            ApplyStyle(textField, style);
            ApplyTextFieldStyle(textField, style);
            
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
            LuaValue items = settings.GetValue("items");
            bool showAddRemoveFooter = settings.GetBool("showAddRemoveFooter", true);
            bool reorderable = settings.GetBool("reorderable", true);
            LuaTable style = settings.GetTable("style");
            LuaFunction makeItemFunction = settings.GetFunction("makeItem");
            LuaFunction bindItemFunction = settings.GetFunction("bindItem");
            LuaFunction itemAddedFunction = settings.GetFunction("itemAdded");
            LuaFunction itemRemovedFunction = settings.GetFunction("itemRemoved");

            IList listItems = null;

            if (items.TryRead(out LuaTable luaTable))
            {
                listItems = new List<LuaValue>(luaTable.ArrayLength);
                
                for (int i = 1; i <= luaTable.ArrayLength; ++i)
                {
                    if (luaTable.TryGetValue(i, out LuaValue v))
                    {
                        listItems.Add(v);
                    }
                }
            }
            else if (items.TryRead(out LuaListProxy luaListProxy))
            {
                listItems = luaListProxy.List;
            }
            
            if (listItems == null)
            {
                UnityEngine.Debug.LogAssertion("Failed to determine list items when creating UIToolkit List.");
                return new ValueTask<int>(context.Return(LuaValue.Nil));
            }
            
            var listView = new ListView
            {
                itemsSource = listItems,
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
                    // Lua Indices expect 1 indexing
                    LuaRuntime.ExecuteFunctionAsync(bindItemFunction, LuaValue.FromObject(new LuaVisualElementProxy(element)), index + 1);
                }
            };
            
            if (itemAddedFunction != null)
            {
                listView.itemsAdded += (indices) =>
                {
                    foreach (int index in indices)
                    {
                        // Lua Indices expect 1 indexing
                        LuaRuntime.ExecuteFunctionAsync(itemAddedFunction, index + 1);
                    }
                };
            }

            if (itemRemovedFunction != null)
            {
                listView.itemsRemoved += (indices) =>
                {
                    foreach (int index in indices)
                    {
                        // Lua Indices expect 1 indexing
                        LuaRuntime.ExecuteFunctionAsync(itemRemovedFunction, index + 1);
                    }
                };
            }

            ApplyStyle(listView, style);
            ApplyListStyle(listView, style);

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

        private ValueTask<int> Dropdown(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaTable settings = context.GetArgument<LuaTable>(0);
            string label = settings.GetString("label", string.Empty);
            string value = settings.GetString("value", string.Empty);
            LuaTable valuesTable = settings.GetTable("values");
            LuaTable style = settings.GetTable("style");
            LuaFunction valueChangedFunction = settings.GetFunction("valueChanged");
            
            List<string> values = new List<string>(valuesTable.ArrayLength);
            foreach (var v in valuesTable)
            {
                values.Add(v.Key.As<string>());
            }

            if (values.Count > 0 && !values.Contains(value))
            {
                // UI Toolkit does not like dropdowns that set a value to one not in the list
                UnityEngine.Debug.LogAssertion($"Failed to find current value {value} in list of available values for dropdown.");
                value = values[0];
            }
            
            DropdownField dropdown = new DropdownField(
                label,
                values,
                value);
            dropdown.RegisterValueChangedCallback(evt =>
            {
                LuaRuntime.ExecuteFunctionAsync(valueChangedFunction, evt.newValue);
            });
            
            ApplyStyle(dropdown, style);
            
            return new ValueTask<int>(context.Return(new LuaVisualElementProxy(dropdown)));
        }

        private ValueTask<int> ApplyStyle(
            LuaFunctionExecutionContext context,
            CancellationToken cancellationToken)
        {
            LuaVisualElementProxy visualElementProxy = context.GetArgument<LuaVisualElementProxy>(0);
            LuaTable style = context.GetArgument<LuaTable>(1);

            ApplyStyle(visualElementProxy.VisualElement, style);
            
            return new ValueTask<int>(context.Return());
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
            
            // Margin Left
            {
                if (luaTable.TryGetValue("marginLeft", out LuaValue marginLeftValue) &&
                    marginLeftValue.TryRead(out float marginLeft))
                {
                    visualElement.style.marginLeft = marginLeft;
                }
            }
            
            // Unity Text Align
            {
                string textAlignString = luaTable.GetString("textAlign");
                if (!string.IsNullOrEmpty(textAlignString))
                {
                    if (Enum.TryParse(textAlignString, out TextAnchor textAnchor))
                    {
                        visualElement.style.unityTextAlign = textAnchor;
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion(
                            $"Failed to parse text align string {textAlignString} as a {nameof(TextAnchor)} value.");
                    }
                }
            }
        }

        private static void ApplyTextFieldStyle(TextField textField, LuaTable luaTable)
        {
            var inputText = textField.Q(className: UnityEngine.UIElements.TextField.inputUssClassName);
            if (inputText != null)
            {
                LuaTable inputTextStyle = luaTable.GetTable("inputTextStyle");
                ApplyStyle(inputText, inputTextStyle);
            }
        }

        private static void ApplyListStyle(ListView listView, LuaTable style)
        {
            if (style.TryGetValue("itemHeight", out LuaValue itemHeightValue) &&
                itemHeightValue.TryRead(out float itemHeight))
            {
                listView.fixedItemHeight = itemHeight;
            }

            if (style.TryGetValue("allowDynamicHeights", out LuaValue allowDynamicHeightsValue) &&
                allowDynamicHeightsValue.TryRead(out bool allowDynamicHeights))
            {
                listView.virtualizationMethod = allowDynamicHeights ? CollectionVirtualizationMethod.DynamicHeight : CollectionVirtualizationMethod.FixedHeight;
            }
        }
    }
}
#endif